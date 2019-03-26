using Reporter;
using System;
using System.ServiceProcess;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Utils.Logger;

namespace WindowsService
{
    public partial class Service : ServiceBase
    {

        TradingReporter _tradingReporter;

        public Service()
        {
            InitializeComponent();

            IUnityContainer _container = new UnityContainer();
            _container.RegisterType<ILogger, ServiceLogger>(new InjectionConstructor(new object[] { this }));
            _container.RegisterType<TradingReporter>(new ContainerControlledLifetimeManager());
            Disposed += (o, e) => _container.Dispose();

            _tradingReporter = _container.Resolve<TradingReporter>();
        }

        protected override void OnStart(string[] args)
        {
            _tradingReporter.Config.UpdateFromAppConfig();
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
            _tradingReporter.Config.UpdateFromAppConfig();
            _tradingReporter.OnContinue();
        }

    }
}
