using NLog;
using NLog.Config;
using NLog.Targets;
using Reporter;
using System;
using Unity;

namespace DebugMe
{
    class Program
    {
        static void Main(string[] args)
        {
            using (IUnityContainer c = GetContainer())
            {
                ITest test = c.Resolve<Test2>();
                test.Run().GetAwaiter().GetResult();
            }
        }

        private static IUnityContainer GetContainer()
        {
            UnityContainer c = new UnityContainer();

            ConfigurationItemFactory.Default.CreateInstance = (Type type) => c.Resolve(type);
            Target.Register<CustomDebugLogger>("CustomDebugLogger");
            return c;
        }


    }
}
