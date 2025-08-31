namespace Sut.Generator.Example;

public class Example9(
  Dependency6 dependency6
) {
  private Dependency6 Dependency6 => dependency6;

  public bool Evaluate(Command command, CancellationToken cancellationToken = default) => Dependency6.Evaluate((c, t) => !t.IsCancellationRequested && c.Id == command.Id, cancellationToken);
}