namespace Sut.Generator.Example;

public class Example14(
  IDependency11<Command2?> dependency11
) {
  private IDependency11<Command2?> Dependency11 => dependency11;

  public async Task<Command2?> Get(int id) => await Dependency11.Get(new Command2 { Id = id });
}