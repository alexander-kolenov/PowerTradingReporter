using NLog.Config;
using NLog.Targets;
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
