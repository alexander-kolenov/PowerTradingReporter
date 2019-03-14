using System.Diagnostics;
using Utils.Logger;

namespace WindowsService
{
    class ServiceLogger : ILogger
    {
        private readonly EventLog _logger;

        public ServiceLogger(Service service)
        {
            //use default service logger
            _logger = service.EventLog;
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
