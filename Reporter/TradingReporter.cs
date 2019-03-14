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

        public void MakeReportSafe(DateTime utcTime)
        {
            try
            {
                MakeReport(utcTime);
                _logger.Log(LogLevel.Debug, $"Make Report({utcTime})");
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"Exception: {ex.Message} {ex.StackTrace}");
            }
        }

        public void MakeReport(DateTime utcTime)
        {
            DataAcquisition da = new DataAcquisition();
            AggregatedData ad = da.GetAggregatedTrades(utcTime);

            ReportBuilder rb = new ReportBuilder();
            string reportFileName = Path.Combine(_config.ReportingDirrectory, rb.GetCsvReportFileName(utcTime));
            CsvData csvData = rb.CreateCsvData(ad);

            CsvWriter w = new CsvWriter();
            w.Write(reportFileName, csvData);
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
                _executionTask = Task.Run(() => MakeReportSafe(DateTime.UtcNow));
            else
                _logger.Log(LogLevel.Warning, $"Skip report at {DateTime.UtcNow:u}. Reason: previous report is running");
        }

        #endregion

    }
}
