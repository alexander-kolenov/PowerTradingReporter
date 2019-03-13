using Reporter;
using System;
using System.Diagnostics;

namespace DebugMe
{
    class Program
    {
        static void Main(string[] args)
        {
            TradingReporterConfiguration c = new TradingReporterConfiguration();
            c.UpdateFromAppConfig();

            TradingReporter reporter = new TradingReporter(c);

            DateTime dt = DateTime.Now;

            for (int i = 0; i < 10; i++)
            {
                dt = dt.AddMonths(1);
                try
                {
                    reporter.MakeReport(dt);
                }
                catch(Exception ex)
                {
                    Debug.WriteLine($"{dt}:  {ex.Message}  -- {ex.StackTrace}");
                }
            }
        }
    }
}
