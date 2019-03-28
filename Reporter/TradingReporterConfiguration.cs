using System;
using System.IO;

namespace Reporter
{
    public class TradingReporterConfiguration
    {
        public static string DefaultReportingDirrectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PowerTradingReporter");
        public static SessionInfo DefaultSessionInfo => new SessionInfo { SessionStart = TimeSpan.FromHours(23) }; //TODO: Fix requirements

        public string ReportingDirrectory { get; set; } = DefaultReportingDirrectory;
        public SessionInfo SessionInfo { get; set; } = DefaultSessionInfo;

    }
}