using NLog;
using NLog.Targets;
using System.Diagnostics;

namespace WindowsService
{
    [Target("ServiceLogger")]
    class ServiceLogger : TargetWithLayout
    {
        private readonly EventLog _logger;

        public ServiceLogger(Service service)
        {
            //use default service logger
            _logger = service.EventLog;
        }


        protected override void Write(LogEventInfo logEvent)
        {

            string message = logEvent.Message;
            if (logEvent.Level >= LogLevel.Error)
                _logger.WriteEntry(message, EventLogEntryType.Error);
            else if (logEvent.Level >= LogLevel.Warn)
                _logger.WriteEntry(message, EventLogEntryType.Warning);
            else if (logEvent.Level >= LogLevel.Info)
                _logger.WriteEntry(message, EventLogEntryType.Information);
#if DEBUG
            else
                _logger.WriteEntry(message, EventLogEntryType.Information);
#endif
        }
    }
}
