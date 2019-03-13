using System;

namespace Reporter
{
    public class TradingReporterConfiguration
    {
        public string ReportingDirrectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        public int ReportingIntervalInMinutes { get; set; } = 5;
    }
}