using System;
using System.Configuration;

namespace Reporter
{
    public class TradingReporterConfiguration
    {
        public string ReportingDirrectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        public int ReportingIntervalInMinutes { get; set; } = 5;

        public void UpdateFromAppConfig()
        {
            string str;

            str = ConfigurationManager.AppSettings["ReportingDirrectory"];
            ReportingDirrectory = !String.IsNullOrWhiteSpace(str) ? str : AppDomain.CurrentDomain.BaseDirectory;

            str = ConfigurationManager.AppSettings["ReportingIntervalInMinutes"];
            ReportingIntervalInMinutes = (str != null && int.TryParse(str, out int interval) && interval > 0) ? interval : 5;

        }
    }
}