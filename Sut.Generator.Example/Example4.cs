namespace Sut.Generator.Example;

public class Example4(
  IDependency1 dependency1
) {
  private IDependency1 Dependency1 => dependency1;

  public async Task<Command> Get(int id) => await Dependency1.Get(id);
}