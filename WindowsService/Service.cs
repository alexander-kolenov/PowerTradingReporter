using NLog;
using NLog.Config;
using NLog.Targets;
using Reporter;
using System;
using System.ServiceProcess;
using Unity;
using Unity.Lifetime;

namespace WindowsService
{
    public partial class Service : ServiceBase
    {

        TradingReporter _tradingReporter;

        public Service()
        {
            InitializeComponent();

            IUnityContainer _container = new UnityContainer();
            _container = new UnityContainer();
            _container.RegisterInstance(new ServiceLogger(this));
            _container.RegisterType<TradingReporter>(new ContainerControlledLifetimeManager());

            ConfigurationItemFactory.Default.CreateInstance = (Type type) => _container.Resolve(type);
            Target.Register<ServiceLogger>("ServiceLogger");

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
