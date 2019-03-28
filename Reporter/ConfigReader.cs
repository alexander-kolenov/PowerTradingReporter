using NLog;
using System;
using System.Configuration;

namespace Reporter
{
    public static class ConfigReader
    {

        public static TimeSpan DefaultReportingInterval => TimeSpan.FromMinutes(5);

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static TradingReporterConfiguration ReadTradingReporterConfigurationFromAppConfig()
        {
            TradingReporterConfiguration trConfig = new TradingReporterConfiguration();

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);

            KeyValueConfigurationCollection settings = config.AppSettings.Settings;

            string str;

            str = settings["ReportingDirrectory"]?.Value;
            trConfig.ReportingDirrectory = !String.IsNullOrWhiteSpace(str) ? str : TradingReporterConfiguration.DefaultReportingDirrectory;

            str = settings["TradingSessionStart"]?.Value;
            trConfig.SessionInfo = (str != null && TimeSpan.TryParse(str, out TimeSpan start)) ? new SessionInfo { SessionStart = start } : TradingReporterConfiguration.DefaultSessionInfo;

            Logger.Log(LogLevel.Debug, $"Config.SessionInfo.SessionStart = {trConfig.SessionInfo.SessionStart}");
            Logger.Log(LogLevel.Debug, $"Config.ReportingDirrectory = {trConfig.ReportingDirrectory}");

            return trConfig;
        }

        public static TimeSpan ReadReportingIntervalFromAppConfig()
        {
            TradingReporterConfiguration trConfig = new TradingReporterConfiguration();

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);

            KeyValueConfigurationCollection settings = config.AppSettings.Settings;

            string str;

            str = settings["ReportingIntervalInMinutes"]?.Value;
            TimeSpan ReportingInterval = (str != null && int.TryParse(str, out int interval) && interval > 0) ? TimeSpan.FromMinutes(interval) : DefaultReportingInterval;

            Logger.Log(LogLevel.Debug, $"Config.ReportingInterval = {ReportingInterval}");

            return ReportingInterval;
        }
    }
}
