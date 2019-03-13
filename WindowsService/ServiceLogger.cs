using System.Diagnostics;
using Utils.Logger;

namespace WindowsService
{
    class ServiceLogger : ILogger
    {
        private EventLog _logger;

        public ServiceLogger(EventLog logger)
        {
            _logger = logger;
        }

        public void Log(LogLevel level, string message)
        {
            switch (level)
            {
                case LogLevel.None:
                    break;
                case LogLevel.Debug:
#if DEBUG
                    _logger.WriteEntry(message, EventLogEntryType.Information);
#endif
                    break;
                case LogLevel.Info:
                    _logger.WriteEntry(message, EventLogEntryType.Information);
                    break;
                case LogLevel.Warning:
                    _logger.WriteEntry(message, EventLogEntryType.Warning);
                    break;
                case LogLevel.Error:
                    _logger.WriteEntry(message, EventLogEntryType.Error);
                    break;
                default:
                    break;
            }
        }
    }
}
