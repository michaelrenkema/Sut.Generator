using Sut;
using Sut.Generator.Example;
using Microsoft.Extensions.Logging;

namespace Test;

[Sut<Example2>]
public partial class Test2
{
  [Test]
  [Arguments(LogLevel.Debug, "Test")]
  [Arguments(LogLevel.Information, "Test")]
  [Arguments(LogLevel.Trace, "Test")]
  [Arguments(LogLevel.Warning, "Test")]
  [Arguments(LogLevel.Error, "Test")]
  [Arguments(LogLevel.Critical, "Test")]
  public void Log(LogLevel logLevel, string message)
  {
    var sut = this.Sut
      .With_Logger(logLevel, message)
      .Build();

    sut.Log(logLevel, message);

    this.Sut.Logger.Verify();
  }

  [Test]
  [Arguments(LogLevel.Warning, "Test")]
  [Arguments(LogLevel.Error, "Test")]
  [Arguments(LogLevel.Critical, "Test")]
  public void LogError(LogLevel logLevel, string message)
  {
    var exception = new Exception("");

    var sut = this.Sut
      .With_Logger(logLevel, message, exception)
      .Build();

    sut.LogError(logLevel, message, exception);

    this.Sut.Logger.Verify();
  }
}