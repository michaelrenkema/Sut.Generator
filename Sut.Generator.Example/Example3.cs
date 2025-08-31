using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Sut.Generator.Example;

public class Example3(
  IOptions<TestOptions> options,
  ILogger<Example3> logger,
  IDependency1 dependency1,
  IDependency3 dependency3,
  Dependency4 dependency4,
  Dependency5 dependency5,
  Dependency6 dependency6
) {
  private TestOptions Options => options.Value;
  private ILogger<Example3> Logger => logger;
  private IDependency1 Dependency1 => dependency1;
  private IDependency3 Dependency3 => dependency3;
  private Dependency4 Dependency4 => dependency4;
  private Dependency5 Dependency5 => dependency5;
  private Dependency6 Dependency6 => dependency6;

  public async Task Run(int id, CancellationToken cancellationToken = default)
  {
    Logger.LogDebug("Running with option: {option}", Options.Option1);

    var command = await Dependency1.Get(id);
    Logger.LogInformation("Retrieved command: {id}", command.Id);

    if (!Dependency6.Evaluate((c, t) => c.Id == command.Id, cancellationToken))
      return;

    if (cancellationToken.IsCancellationRequested)
    {
      Logger.LogDebug("Operation was cancelled.");
      return;
    }

    try
    {
      Dependency3.Run(command);
    }
    catch (Exception ex)
    {
      Logger.LogError(ex, "Error when attempting to run command");
    }

    if (Dependency4.Status != Status.Success)
      return;

    if (cancellationToken.IsCancellationRequested)
    {
      Logger.LogDebug("Operation was cancelled.");
      return;
    }

    try
    {
      var result = await Dependency5.Update(command);
    }
    catch (Exception ex)
    {
      Logger.LogError(ex, "Error when attempting to update");
      throw;
    }

    Logger.LogDebug("Completed running with option: {option}", Options.Option1);
  }
}