using System.Diagnostics;

namespace Utils.Logger
{
    public class DebugLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {
            Debug.WriteLine($"{level}: {message}");
        }
    }
}
