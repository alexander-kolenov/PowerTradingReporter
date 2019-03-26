using NLog;
using NLog.Config;
using NLog.Targets;
using Reporter;
using System;
using Unity;
using Unity.Lifetime;

namespace DebugMe
{
    class Program : IDisposable
    {
        private IUnityContainer _container;

        Program()
        {
            _container = new UnityContainer();
            _container.RegisterType<TradingReporter>(new ContainerControlledLifetimeManager());
            _container.RegisterFactory<ILogger>(o => LogManager.GetCurrentClassLogger());

            ConfigurationItemFactory.Default.CreateInstance = (Type type) => _container.Resolve(type);
            Target.Register<CustomDebugLogger>("CustomDebugLogger");
        }

        static void Main(string[] args)
        {
            using (Program p = new Program())
            {
                p.Test2();
            }
        }

        public void Dispose()
        {
            _container?.Dispose();
            _container = null;
        }

        private void Test1()
        {
            ILogger logger = _container.Resolve<ILogger>();
            TradingReporter reporter = _container.Resolve<TradingReporter>();

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
                    logger.Log(LogLevel.Debug, $"{dt}:  {ex.Message}  -- {ex.StackTrace}");
                }
            }
        }

        private void Test2()
        {
            ILogger logger = _container.Resolve<ILogger>();
            TradingReporter reporter = _container.Resolve<TradingReporter>();

            reporter.Config.UpdateFromAppConfig();

            logger.Log(LogLevel.Debug, $"Config.ReportingInterval = {reporter.Config.ReportingInterval}");
            logger.Log(LogLevel.Debug, $"Config.SessionInfo.SessionStart = {reporter.Config.SessionInfo.SessionStart}");
            logger.Log(LogLevel.Debug, $"Config.ReportingDirrectory = {reporter.Config.ReportingDirrectory}");

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
            }
        }

    }

}

