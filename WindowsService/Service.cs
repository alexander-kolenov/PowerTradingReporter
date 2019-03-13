using Reporter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Utils.Logger;

namespace WindowsService
{
    public partial class Service : ServiceBase
    {
        ServiceLogger _logger;
        TradingReporter TradingReporter;

        public Service()
        {
            InitializeComponent();
            _logger = InitLogger();

            TradingReporterConfiguration config = new TradingReporterConfiguration();
            config.UpdateFromAppConfig();

            TradingReporter = new TradingReporter(config, _logger);
        }

        private ServiceLogger InitLogger()
        {
            var _log = new EventLog();
            _log.Source = ServiceName;
            _log.Log = "Application";
            _log.BeginInit();
            if (!EventLog.SourceExists(_log.Source))
            {
                EventLog.CreateEventSource(_log.Source, _log.Log);
            }
            _log.EndInit();

            return new ServiceLogger(_log);
        }

        protected override void OnStart(string[] args)
        {
            _logger.Log(LogLevel.Info,"Started");
            TradingReporter.OnStart();
        }

        protected override void OnStop()
        {
            _logger.Log(LogLevel.Info, "Stopped");
            TradingReporter.OnStop();
        }

        protected override void OnPause()
        {
            _logger.Log(LogLevel.Info, "Paused");
            TradingReporter.OnPause();
        }

        protected override void OnContinue()
        {
            _logger.Log(LogLevel.Info, "Continue");
            TradingReporter.OnContinue();
        }
    }
}
