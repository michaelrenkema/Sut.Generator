using Microsoft.Extensions.Logging;

namespace Sut.Generator.Example;

public class Example2(
  ILogger<Example2> logger
)
{
  private ILogger<Example2> Logger => logger;

  public void Log(LogLevel level, string message, params object?[] args)
  {
    switch (level)
    {
      case LogLevel.Trace:
        Logger.LogTrace(message, args);
        break;
      case LogLevel.Debug:
        Logger.LogDebug(message, args);
        break;
      case LogLevel.Information:
        Logger.LogInformation(message, args);
        break;
      case LogLevel.Warning:
        Logger.LogWarning(message, args);
        break;
      case LogLevel.Error:
        Logger.LogError(message, args);
        break;
      case LogLevel.Critical:
        Logger.LogCritical(message, args);
        break;
    }
  }

  public void LogError(LogLevel level, string message, Exception exception, params object?[] args)
  {
    switch (level)
    {
      case LogLevel.Warning:
        Logger.LogWarning(exception, message, args);
        break;
      case LogLevel.Error:
        Logger.LogError(exception, message, args);
        break;
      case LogLevel.Critical:
        Logger.LogCritical(exception, message, args);
        break;
    }
  }
}