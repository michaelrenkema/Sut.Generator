namespace Sut.Generator.Example;

public class Example10(
  IDependency7 dependency7
) {
  private IDependency7 Dependency7 => dependency7;

  public async Task<Command> Get(int[]? ids) => await Dependency7.Get(ids);
}