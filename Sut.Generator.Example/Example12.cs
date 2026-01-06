namespace Sut.Generator.Example;

public class Example12(
  IDependency9<Command2> dependency9
) {
  private IDependency9<Command2> Dependency9 => dependency9;

  public async Task<Command2?> Get(int id) => await Dependency9.Get(new Command2 { Id = id });
}