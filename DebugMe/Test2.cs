using NLog;
using Reporter;
using System;
using System.Threading.Tasks;
using Unity;

namespace DebugMe
{
    public class Test2 : ITest
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Dependency]
        public TradingReporter Reporter { get; set; }

        public Task Run()
        {
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
                        return Task.CompletedTask;
                    default:
                        Console.WriteLine();
                        continue;
                }
            }
        }

    }
}
