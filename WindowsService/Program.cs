using NLog.Config;
using NLog.Targets;
using Reporter;
using System.ServiceProcess;
using Unity;
using Unity.Lifetime;

namespace WindowsService
{
    static class Program
    {
        static void Main()
        {
            using (IUnityContainer c = new UnityContainer())
            {
                //Add IDisposable
                c.RegisterType<TradingReporter>(new HierarchicalLifetimeManager());
                c.RegisterType<Service>(new HierarchicalLifetimeManager());

                //Add Custom logger to NLog
                ConfigurationItemFactory.Default.CreateInstance = (t) => c.Resolve(t);
                Target.Register<ServiceLogger>("ServiceLogger");

                //Run service
                ServiceBase.Run(new[] { c.Resolve<Service>() });
            }
        }
    }
}
