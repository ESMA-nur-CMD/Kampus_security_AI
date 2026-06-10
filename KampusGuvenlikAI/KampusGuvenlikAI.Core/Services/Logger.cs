namespace KampusGuvenlikAI.Core.Services;

public static class Logger
{
    private static readonly object SyncRoot = new();

    public static string LogFilePath { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "KampusGuvenlikAI",
        "application.log");

    public static void Log(Exception exception)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(LogFilePath)!);
            var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {exception.GetType().Name} | {exception.Message}{Environment.NewLine}{exception.StackTrace}{Environment.NewLine}";

            lock (SyncRoot)
            {
                File.AppendAllText(LogFilePath, line);
            }
        }
        catch
        {
            // Logging must never crash the user-facing workflow.
        }
    }

    public static void LogMessage(string message)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(LogFilePath)!);
            lock (SyncRoot)
            {
                File.AppendAllText(LogFilePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}{Environment.NewLine}");
            }
        }
        catch
        {
        }
    }
}
