using NLog;
using Reporter;
using System;
using System.Threading.Tasks;
using Unity;

namespace DebugMe
{
    public class Test3 : ITest
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        [Dependency]
        public ReportProcessor ReportProcessor { get; set; }

        public void Run()
        {

            for (int i = 0; i < 10; i++)
            {
                try
                {
                    _logger.Log(LogLevel.Debug, $"Step {i}    RunSynchronously()");
                    MakeReportAnyway().RunSynchronously();
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Debug, $"Step EXception {i}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                }
            }
        }

        public Task MakeReportAnyway()
        {
            _logger.Log(LogLevel.Debug, $"reporter.MakeReport({DateTime.UtcNow:u});");
            Task task = ReportProcessor.MakeReportTask();
            task.ContinueWith(t =>
            {
                var ex = t.Exception.InnerException;
                _logger.Log(LogLevel.Error, $"Exception: {ex.Message}");
                _logger.Log(LogLevel.Error, $"Repeat");
                MakeReportAnyway().RunSynchronously();
            }, TaskContinuationOptions.NotOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously);
            return task;
        }


    }
}
