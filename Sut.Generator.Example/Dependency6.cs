using System.Diagnostics.Contracts;

namespace Sut.Generator.Example;

public class Dependency6
{
  private Command Command { get; }

  public Dependency6(){}

  public Dependency6(Command command)
  {
    this.Command = command;
  }

  public virtual bool Evaluate(Func<Command, CancellationToken, bool> predicate, CancellationToken cancellationToken = default)
  {
    return predicate(Command, cancellationToken);
  }
}