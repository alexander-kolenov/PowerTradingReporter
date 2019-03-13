using Reporter;
using System;
using System.Diagnostics;
using Utils.Logger;

namespace DebugMe
{
    class Program
    {
        static void Main(string[] args)
        {
            Test2();
        }

        private static void Test2()
        {
            TradingReporterConfiguration c = new TradingReporterConfiguration();
            c.UpdateFromAppConfig();

            TradingReporter reporter = new TradingReporter(c, new DebugLogger());

            while (true)
            {
                var key = Console.ReadKey().KeyChar;
                switch (key)
                {
                    case 's':
                        Console.WriteLine("Start");
                        reporter.OnStart();
                        break;
                    case 'x':
                        Console.WriteLine("Stop");
                        reporter.OnStop();
                        break;
                    case 'p':
                        Console.WriteLine("Pause");
                        reporter.OnPause();
                        break;
                    case 'c':
                        Console.WriteLine("Continue");
                        reporter.OnContinue();
                        break;
                }

            }
            
        }

        private static void Test1()
        {
            TradingReporterConfiguration c = new TradingReporterConfiguration();
            c.UpdateFromAppConfig();

            TradingReporter reporter = new TradingReporter(c, new DebugLogger());

            DateTime dt = DateTime.Now;

            for (int i = 0; i < 10; i++)
            {
                dt = dt.AddHours(1);
                try
                {
                    reporter.MakeReport(dt);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{dt}:  {ex.Message}  -- {ex.StackTrace}");
                }
            }
        }
    }
}
