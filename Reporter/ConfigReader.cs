using System;
using System.Configuration;

namespace Reporter
{
    public static class ConfigReader
    {

        public static void UpdateFromAppConfig(this TradingReporterConfiguration trConfig)
        {

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);

            KeyValueConfigurationCollection settings = config.AppSettings.Settings;

            string str;

            str = settings["ReportingDirrectory"]?.Value;
            trConfig.ReportingDirrectory = !String.IsNullOrWhiteSpace(str) ? str : TradingReporterConfiguration.DefaultReportingDirrectory;

            str = settings["ReportingIntervalInMinutes"]?.Value;
            trConfig.ReportingInterval = (str != null && int.TryParse(str, out int interval) && interval > 0) ? TimeSpan.FromMinutes(interval) : TradingReporterConfiguration.DefaultReportingInterval;

            str = settings["TradingSessionStart"]?.Value;
            trConfig.SessionInfo = (str != null && TimeSpan.TryParse(str, out TimeSpan start)) ? new SessionInfo { SessionStart = start } : TradingReporterConfiguration.DefaultSessionInfo;

        }

    }
}
