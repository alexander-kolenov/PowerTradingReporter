using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Utils.Csv;
using Utils.Logger;

namespace Reporter
{
    public class TradingReporter : IDisposable
    {
        public TradingReporterConfiguration Config { get; set; }
        public ILogger Logger { get; set; }
        private Timer _timer;
        private volatile Task _executionTask;


        public TradingReporter(TradingReporterConfiguration config, ILogger logger)
        {
            Config = config;
            Logger = logger;
            _executionTask = new Task(() => { }); // Set _executionTask != null
            _executionTask.RunSynchronously(); // Make _executionTask.IsCompleted = true
            _timer = new Timer(new TimerCallback(TimerProc));
        }

        public void MakeReportAnyway()
        {
            if (MakeReportSafe() != null)
                return;

            // shedule repeat reading
            _executionTask = Task.Run(() => MakeReportAnyway());
        }

        /// <summary>
        /// Creates report and save it to file
        /// </summary>
        /// <param name="utcTime"></param>
        /// <returns>File name of report</returns>
        public string MakeReportSafe()
        {
            DateTime utcNow = DateTime.UtcNow;
            string reportName = null;
            try
            {
                reportName = MakeReport(utcNow);
                Logger.Log(LogLevel.Debug, $"Report created: {reportName}");
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Error, $"try to Make Report ({utcNow}): Exception: {ex.Message} {ex.StackTrace}");
            }
            return reportName;
        }

        /// <summary>
        /// Creates report and save it to file
        /// </summary>
        /// <param name="utcTime"></param>
        /// <returns>File name of report</returns>
        public string MakeReport(DateTime utcTime)
        {
            DataAcquisition da = new DataAcquisition();
            DateTime tradingDate = da.GetTradingDay(utcTime, Config.SessionInfo);
            AggregatedData ad = da.GetAggregatedTrades(tradingDate, Config.SessionInfo);

            ReportBuilder rb = new ReportBuilder();
            Directory.CreateDirectory(Config.ReportingDirrectory);
            string reportFileName = Path.Combine(Config.ReportingDirrectory, rb.GetCsvReportFileName(utcTime));
            CsvData csvData = rb.CreateCsvData(ad);

            CsvWriter w = new CsvWriter();
            w.Write(reportFileName, csvData);

            return reportFileName;
        }



        #region OnSomething

        public void OnStart()
        {
            _timer?.Change(0, (int)Config.ReportingInterval.TotalMilliseconds);
        }

        public void OnStop()
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            Task.WaitAll(new[] { _executionTask });
        }

        public void OnPause()
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void OnContinue()
        {
            _timer?.Change(0, (int)Config.ReportingInterval.TotalMilliseconds);
        }


        #endregion

        #region private

        private void TimerProc(object state)
        {
            if (_executionTask.IsCompleted)
                _executionTask = Task.Run(() => MakeReportAnyway());
            else
                Logger.Log(LogLevel.Warning, $"Skip report at {DateTime.Now:G}. Reason: previous report is running");
        }

        #endregion


        public void Dispose()
        {
            if (_timer != null)
            {
                OnStop();
                Logger.Log(LogLevel.Debug, $"{GetType().Name} Disposed");
                _timer.Dispose();
                _timer = null;
            }
        }
    }
}
