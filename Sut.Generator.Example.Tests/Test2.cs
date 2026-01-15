using Sut;
using Sut.Generator.Example;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

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
  public async Task Log(LogLevel logLevel, string message)
  {
    var logger = new FakeLogger<Example2>();

    var sut = this.Sut
      .With_Logger(logger)
      .Build();

    sut.Log(logLevel, message);

    await Assert.That(logger.LatestRecord.Level).IsEquivalentTo(logLevel);
    await Assert.That(logger.LatestRecord.Message).IsEquivalentTo(message);
  }

  [Test]
  [Arguments(LogLevel.Warning, "Test")]
  [Arguments(LogLevel.Error, "Test")]
  [Arguments(LogLevel.Critical, "Test")]
  public async Task LogError(LogLevel logLevel, string message)
  {
    var logger = new FakeLogger<Example2>();

    var exception = new Exception("");

    var sut = this.Sut
      .With_Logger(logger)
      .Build();

    sut.LogError(logLevel, message, exception);

    await Assert.That(logger.LatestRecord.Level).IsEquivalentTo(logLevel);
    await Assert.That(logger.LatestRecord.Message).IsEquivalentTo(message);
    await Assert.That(logger.LatestRecord.Exception).IsEquivalentTo(exception);
  }
}