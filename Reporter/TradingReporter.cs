using Csv;
using NLog;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Reporter
{
    public class TradingReporter : IDisposable
    {
        public TradingReporterConfiguration Config { get; set; }


        private ReportProcessor _reportProcessor;

        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private Timer _timer;
        private volatile Task _executionTask;


        public TradingReporter(TradingReporterConfiguration config)
        {
            Config = config;
            _reportProcessor = new ReportProcessor(Config);

            _executionTask = new Task(() => { }); // Set _executionTask != null
            _executionTask.RunSynchronously(); // Make _executionTask.IsCompleted = true
            _timer = new Timer(new TimerCallback(TimerProc));
        }

        public void MakeReportAnyway()
        {
            Task task = _reportProcessor.MakeReportTask();
            task.ContinueWith(t =>
            {
                var ex = t.Exception.InnerException;
                _logger.Log(LogLevel.Error, $"Make Report Exception: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                _executionTask = Task.Run(() => MakeReportAnyway());
            }, TaskContinuationOptions.NotOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously);
            task.RunSynchronously();
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
                _logger.Log(LogLevel.Warn, $"Skip report at {DateTime.Now:G}. Reason: previous report is running");
        }

        #endregion


        public void Dispose()
        {
            if (_timer != null)
            {
                OnStop();
                _logger.Log(LogLevel.Debug, $"{GetType().Name} Disposed");
                _timer.Dispose();
                _timer = null;
            }
        }
    }
}
