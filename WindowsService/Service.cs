using Reporter;
using System;
using System.ServiceProcess;
using Utils.Logger;

namespace WindowsService
{
    public partial class Service : ServiceBase
    {
        TradingReporterConfiguration _config;
        ServiceLogger _logger;
        TradingReporter _tradingReporter;

        public Service()
        {
            InitializeComponent();
            _logger = new ServiceLogger(this);

            _config = new TradingReporterConfiguration();
            _config.UpdateFromAppConfig();

            _tradingReporter = new TradingReporter(_config, _logger);
            this.Disposed += (s, e) =>_tradingReporter.Dispose();
        }

        private void Service_Disposed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnStart(string[] args)
        {
            _config.UpdateFromAppConfig();
            _tradingReporter.OnStart();
        }

        protected override void OnStop()
        {
            _tradingReporter.OnStop();
        }

        protected override void OnPause()
        {
            _tradingReporter.OnPause();
        }

        protected override void OnContinue()
        {
            _config.UpdateFromAppConfig();
            _tradingReporter.OnContinue();
        }

    }
}
