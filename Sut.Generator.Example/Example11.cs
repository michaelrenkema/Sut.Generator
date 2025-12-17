namespace Sut.Generator.Example;

public class Example11(
  IDependency8<Command> dependency8
) {
  private IDependency8<Command> Dependency8 => dependency8;

  public async Task<Command?> Get(int id) => await Dependency8.Get(new Command(id));
}