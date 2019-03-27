using NLog;
using Reporter;
using System;
using Unity;

namespace DebugMe
{
    public class Test1 : ITest
    {
        [Dependency]
        public ILogger Logger { get; set; }

        [Dependency]
        public TradingReporter Reporter { get; set; }

        public void Run()
        {
            DateTime dt = DateTime.UtcNow;

            for (int i = 0; i < 3; i++)
            {
                dt = dt.AddHours(1);
                try
                {
                    Logger.Log(LogLevel.Debug, $"reporter.MakeReport({dt:u});");
                    Reporter.MakeReport(dt);
                }
                catch (Exception ex)
                {
                    Logger.Log(LogLevel.Debug, $"{dt}:  {ex.Message}  -- {ex.StackTrace}");
                }
            }
        }

    }
}
