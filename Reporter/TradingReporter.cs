using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Utils.Csv;
using Utils.Logger;

namespace Reporter
{
    public class TradingReporter
    {
        private readonly TradingReporterConfiguration _config;
        private readonly ILogger _logger;
        private readonly Timer _timer;
        private volatile Task _executionTask;

        public TradingReporter(TradingReporterConfiguration config, ILogger logger)
        {
            _config = config;
            _logger = logger;
            _executionTask = new Task(() => { }); // Set _executionTask != null
            _executionTask.RunSynchronously(); // Make _executionTask.IsCompleted = true
            _timer = new Timer(new TimerCallback(TimerProc));
        }

        public void MakeReportSafe()
        {
            DateTime utcNow = DateTime.UtcNow;
            try
            {
                string reportName = MakeReport(utcNow);
                _logger.Log(LogLevel.Debug, $"Report created: {reportName}");
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"try to Make Report ({utcNow}): Exception: {ex.Message} {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Creates report and save it to file
        /// </summary>
        /// <param name="utcTime"></param>
        /// <returns>File name of report</returns>
        public string MakeReport(DateTime utcTime)
        {
            DataAcquisition da = new DataAcquisition();
            DateTime tradingDate = da.GetTradingDay(utcTime, _config.SessionInfo);
            AggregatedData ad = da.GetAggregatedTrades(tradingDate, _config.SessionInfo);

            ReportBuilder rb = new ReportBuilder();
            Directory.CreateDirectory(_config.ReportingDirrectory);
            string reportFileName = Path.Combine(_config.ReportingDirrectory, rb.GetCsvReportFileName(utcTime));
            CsvData csvData = rb.CreateCsvData(ad);

            CsvWriter w = new CsvWriter();
            w.Write(reportFileName, csvData);

            return reportFileName;
        }



        #region OnSomething

        public void OnStart() => _timer.Change(0, GetTimerInterval());

        public void OnStop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            Task.WaitAll(new[] { _executionTask });
        }

        public void OnContinue() => _timer.Change(0, GetTimerInterval());

        public void OnPause() => _timer.Change(Timeout.Infinite, Timeout.Infinite);

        #endregion

        #region private

        private int GetTimerInterval()
        {
            _config.UpdateFromAppConfig();
            return (int)_config.ReportingInterval.TotalMilliseconds;
        }

        private void TimerProc(object state)
        {
            if (_executionTask.IsCompleted)
                _executionTask = Task.Run(() => MakeReportSafe());
            else
                _logger.Log(LogLevel.Warning, $"Skip report at {DateTime.Now:G}. Reason: previous report is running");
        }

        #endregion

    }
}
