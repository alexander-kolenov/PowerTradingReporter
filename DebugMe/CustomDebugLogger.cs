using NLog;
using NLog.Targets;
using System;

namespace DebugMe
{
    [Target("CustomDebugLogger")]
    public class CustomDebugLogger : TargetWithLayout
    {
        protected override void Write(LogEventInfo logEvent)
        {
            Console.WriteLine($"    {logEvent.Message}");
        }
    }
}
