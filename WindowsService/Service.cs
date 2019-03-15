using Reporter;
using System.ServiceProcess;

namespace WindowsService
{
    public partial class Service : ServiceBase
    {
        TradingReporterConfiguration _config;
        ServiceLogger _logger;
        TradingReporter TradingReporter;

        public Service()
        {
            InitializeComponent();
            _logger = new ServiceLogger(this);

            _config = new TradingReporterConfiguration();
            _config.UpdateFromAppConfig();

            TradingReporter = new TradingReporter(_config, _logger);
        }

        protected override void OnStart(string[] args)
        {
            _config.UpdateFromAppConfig();
            TradingReporter.OnStart();
        }

        protected override void OnStop()
        {
            TradingReporter.OnStop();
        }

        protected override void OnPause()
        {
            TradingReporter.OnPause();
        }

        protected override void OnContinue()
        {
            _config.UpdateFromAppConfig();
            TradingReporter.OnContinue();
        }
    }
}
