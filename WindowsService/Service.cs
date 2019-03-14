using Reporter;
using System.ServiceProcess;

namespace WindowsService
{
    public partial class Service : ServiceBase
    {
        ServiceLogger _logger;
        TradingReporter TradingReporter;

        public Service()
        {
            InitializeComponent();
            _logger = new ServiceLogger(this);

            TradingReporterConfiguration config = new TradingReporterConfiguration();
            config.UpdateFromAppConfig();

            TradingReporter = new TradingReporter(config, _logger);
        }

        protected override void OnStart(string[] args)
        {
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
            TradingReporter.OnContinue();
        }
    }
}
