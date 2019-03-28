using Reporter;
using System.ServiceProcess;
using Unity;

namespace WindowsService
{
    public partial class Service : ServiceBase
    {
        [Dependency]
        public TradingReporter TradingReporter { get; set; }

        public Service()
        {
            InitializeComponent();
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
