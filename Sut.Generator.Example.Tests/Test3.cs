using Sut;
using Sut.Generator.Example;
using Microsoft.Extensions.Logging.Testing;
using Microsoft.Extensions.Logging;

namespace Test;

[Sut<Example3>]
public partial class Test3
{
  [Test]
  public async Task Success()
  {
    var options = new TestOptions { Option1 = "test" };
    var command = new Command(1);
    var logger = new FakeLogger<Example3>();

    var sut = this.Sut
      .With_Options(options)
      .With_Logger(logger)
      .With_Dependency1_Get(id => id == command.Id, command)
      .With_Dependency6_Evaluate((f) => f(command, default), (t) => t == default, true)
      .With_Dependency3_Run(c => c.Id == command.Id)
      .With_Dependency4_Status(Status.Success)
      .With_Dependency5_Update<Command>(c => c.Id == command.Id, command)
      .Build();

    await sut.Run(command.Id);

    this.Sut.Dependency1.Verify();
    this.Sut.Dependency6.Verify();

    await Assert.That(logger.LatestRecord.Level).IsEquivalentTo(LogLevel.Debug);
    await Assert.That(logger.LatestRecord.Message).IsEquivalentTo($"Completed running with option: {options.Option1}");
  }

  [Test]
  public async Task Failure()
  {
    var options = new TestOptions { Option1 = "test" };
    var command = new Command(1);
    var exception = new ArgumentException("Test exception");
    var logger = new FakeLogger<Example3>();

    var sut = this.Sut
      .With_Options(options)
      .With_Logger(logger)
      .With_Dependency1_Get(id => id == command.Id, command)
      .With_Dependency6_Evaluate((f) => f(command, default), (t) => t == default, true)
      .With_Dependency3_Run(c => c.Id == command.Id)
      .With_Dependency4_Status(Status.Success)
      .With_Dependency5_Update_Exception<Command>(c => c.Id == command.Id, exception)
      .Build();

    await Assert.ThrowsAsync<ArgumentException>(sut.Run(command.Id));

    await Assert.That(logger.LatestRecord.Level).IsEquivalentTo(LogLevel.Error);
    await Assert.That(logger.LatestRecord.Message).IsEquivalentTo("Error when attempting to update");
    await Assert.That(logger.LatestRecord.Exception).IsEquivalentTo(exception);
  }
}
