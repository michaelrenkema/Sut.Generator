using Sut;
using Sut.Generator.Example;
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

    var sut = this.Sut
      .With_Options(options)
      .With_Logger(LogLevel.Debug, $"Running with option: {options.Option1}")
      .With_Logger(LogLevel.Information, $"Retrieved command: {command.Id}")
      .With_Logger(LogLevel.Debug, $"Completed running with option: {options.Option1}")
      .With_Dependency1_Get(id => id == command.Id, command)
      .With_Dependency6_Evaluate((f) => f(command, default), (t) => t == default, true)
      .With_Dependency3_Run(c => c.Id == command.Id)
      .With_Dependency4_Status(Status.Success)
      .With_Dependency5_Update<Command>(c => c.Id == command.Id, command)
      .Build();

    await sut.Run(command.Id);

    this.Sut.Dependency1.Verify();
    this.Sut.Dependency6.Verify();
    this.Sut.Logger.Verify();
  }

  [Test]
  public async Task Failure()
  {
    var options = new TestOptions { Option1 = "test" };
    var command = new Command(1);
    var exception = new ArgumentException("Test exception");

    var sut = this.Sut
      .With_Options(options)
      .With_Logger(LogLevel.Debug, $"Running with option: {options.Option1}")
      .With_Logger(LogLevel.Information, $"Retrieved command: {command.Id}")
      .With_Logger(LogLevel.Error, "Error when attempting to update", exception)
      .With_Dependency1_Get(id => id == command.Id, command)
      .With_Dependency6_Evaluate((f) => f(command, default), (t) => t == default, true)
      .With_Dependency3_Run(c => c.Id == command.Id)
      .With_Dependency4_Status(Status.Success)
      .With_Dependency5_Update_Exception<Command>(c => c.Id == command.Id, exception)
      .Build();

    await Assert.ThrowsAsync<ArgumentException>(sut.Run(command.Id));

    this.Sut.Logger.Verify();
  }
}
