using System;
using System.Configuration;

namespace Reporter
{
    public class TradingReporterConfiguration
    {
        public static TimeSpan DefaultReportingInterval => TimeSpan.FromMinutes(5);
        public static string DefaultReportingDirrectory => AppDomain.CurrentDomain.BaseDirectory;
        public static SessionInfo DefaultSessionInfo => new SessionInfo { SessionStart = TimeSpan.FromHours(23) }; //TODO: Fix requirements

        public string ReportingDirrectory { get; set; } = DefaultReportingDirrectory;
        public TimeSpan ReportingInterval { get; set; } = DefaultReportingInterval;


        public SessionInfo SessionInfo { get; set; } = DefaultSessionInfo;

        public void UpdateFromAppConfig()
        {
            string str;

            str = ConfigurationManager.AppSettings["ReportingDirrectory"];
            ReportingDirrectory = !String.IsNullOrWhiteSpace(str) ? str : DefaultReportingDirrectory;

            str = ConfigurationManager.AppSettings["ReportingIntervalInMinutes"];
            ReportingInterval = (str != null && int.TryParse(str, out int interval) && interval > 0) ? TimeSpan.FromMinutes(interval) : DefaultReportingInterval;

        }
    }
}