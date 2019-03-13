namespace Utils.Logger
{
    public interface ILogger
    {
        void Log(LogLevel level, string message);
    }

    public enum LogLevel
    {
        None,
        Debug,
        Info,
        Warning,
        Error,
    }
}
