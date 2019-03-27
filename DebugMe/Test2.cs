using NLog;
using Reporter;
using System;
using Unity;

namespace DebugMe
{
    public class Test2 : ITest
    {
        [Dependency]
        public ILogger Logger { get; set; }

        [Dependency]
        public TradingReporter Reporter { get; set; }

        public void Run()
        {
            Reporter.Config.UpdateFromAppConfig();

            Logger.Log(LogLevel.Debug, $"Config.ReportingInterval = {Reporter.Config.ReportingInterval}");
            Logger.Log(LogLevel.Debug, $"Config.SessionInfo.SessionStart = {Reporter.Config.SessionInfo.SessionStart}");
            Logger.Log(LogLevel.Debug, $"Config.ReportingDirrectory = {Reporter.Config.ReportingDirrectory}");

            while (true)
            {
                var key = Console.ReadKey().KeyChar;
                switch (key)
                {
                    case 's':
                        Logger.Log(LogLevel.Debug, "Start");
                        Reporter.OnStart();
                        break;
                    case 'x':
                        Logger.Log(LogLevel.Debug, "Stop");
                        Reporter.OnStop();
                        break;
                    case 'p':
                        Logger.Log(LogLevel.Debug, "Pause");
                        Reporter.OnPause();
                        break;
                    case 'c':
                        Logger.Log(LogLevel.Debug, "Continue");
                        Reporter.OnContinue();
                        break;
                    case 'q':
                        return;
                    default:
                        Console.WriteLine();
                        continue;
                }
            }
        }

    }
}
