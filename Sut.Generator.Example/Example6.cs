namespace Sut.Generator.Example;

public class Example6(
  IDependency3 dependency3
) {
  private IDependency3 Dependency3 => dependency3;

  public void Run(Command command)
  {
    Dependency3.Run(command);
  }
}