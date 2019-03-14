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

    }
}