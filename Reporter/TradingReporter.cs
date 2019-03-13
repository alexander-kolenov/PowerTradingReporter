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
        private volatile Task _currentTask;

        public TradingReporter(TradingReporterConfiguration config, ILogger logger)
        {
            _config = config;
            _logger = logger;
            _timer = new Timer(new TimerCallback(TimerProc));
            _currentTask = Task.Run(() => { });
        }

        public void MakeReportSafe(DateTime localTime)
        {
            try
            {
                _logger.Log(LogLevel.Debug, $"MakeReport({localTime})");
                MakeReport(localTime);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"Exception: {ex.Message}");
            }
        }

        public void MakeReport(DateTime localTime)
        {
            DataAcquisition da = new DataAcquisition();
            AggregatedData ad = da.GetAggregatedTrades(localTime);

            ReportBuilder rb = new ReportBuilder();
            string reportFileName = Path.Combine(_config.ReportingDirrectory, rb.GetCsvReportFileName(localTime));
            CsvData csvData = rb.CreateCsvData(ad);

            CsvWriter w = new CsvWriter();
            w.Write(reportFileName, csvData);
        }



        #region OnSomething

        public void OnStart() => _timer.Change(0, GetTimerInterval());

        public void OnStop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            Task.WaitAll(new[] { _currentTask });
        }

        public void OnContinue() => _timer.Change(0, GetTimerInterval());

        public void OnPause() => _timer.Change(Timeout.Infinite, Timeout.Infinite);

        #endregion

        #region private

        private int GetTimerInterval()
        {
            _config.UpdateFromAppConfig();
            return _config.ReportingIntervalInMinutes * 60 * 1000;
        }

        private void TimerProc(object state)
        {
            if (_currentTask.IsCompleted)
                _currentTask = Task.Run(() => MakeReportSafe(DateTime.Now));
            else
                _logger.Log(LogLevel.Warning, $"Skip report at {DateTime.Now}. Reason: previous report is running");
        }

        #endregion

    }
}
