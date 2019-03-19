using Reporter;
using System;
using System.Diagnostics;
using Utils.Logger;

namespace DebugMe
{
    class Program
    {
        static void Main(string[] args)
        {
            Test2();
        }

        private static void Test1()
        {
            TradingReporterConfiguration config = new TradingReporterConfiguration();
            config.UpdateFromAppConfig();
            ILogger logger = new DebugLogger();

            using (TradingReporter reporter = new TradingReporter(config, logger))
            {
                DateTime dt = DateTime.UtcNow;

                for (int i = 0; i < 3; i++)
                {
                    dt = dt.AddHours(1);
                    try
                    {
                        logger.Log(LogLevel.Debug, $"reporter.MakeReport({dt:u});");
                        reporter.MakeReport(dt);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"{dt}:  {ex.Message}  -- {ex.StackTrace}");
                    }
                }
            }
        }

        private static void Test2()
        {
            TradingReporterConfiguration config = new TradingReporterConfiguration();
            config.UpdateFromAppConfig();

            ILogger logger = new ConsoleLogger();

            using (TradingReporter reporter = new TradingReporter(config, logger))
            {
                while (true)
                {
                    var key = Console.ReadKey().KeyChar;
                    switch (key)
                    {
                        case 's':
                            logger.Log(LogLevel.Debug, "Start");
                            reporter.OnStart();
                            break;
                        case 'x':
                            logger.Log(LogLevel.Debug, "Stop");
                            reporter.OnStop();
                            break;
                        case 'p':
                            logger.Log(LogLevel.Debug, "Pause");
                            reporter.OnPause();
                            break;
                        case 'c':
                            logger.Log(LogLevel.Debug, "Continue");
                            reporter.OnContinue();
                            break;
                        case 'q':
                            return;
                        default:
                            Console.WriteLine();
                            continue;
                    }
                    logger.Log(LogLevel.Debug, $"Config.ReportingInterval = {config.ReportingInterval}");
                    logger.Log(LogLevel.Debug, $"Config.SessionInfo.SessionStart = {config.SessionInfo.SessionStart}");
                    logger.Log(LogLevel.Debug, $"Config.ReportingDirrectory = {config.ReportingDirrectory}");

                }
            }

        }

    }
}
