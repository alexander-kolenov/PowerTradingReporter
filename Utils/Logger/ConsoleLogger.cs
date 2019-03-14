using System;

namespace Utils.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {
            Console.WriteLine($"{DateTime.UtcNow:u} {level}: {message}");
        }
    }
}
