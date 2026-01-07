namespace Sut.Generator.Example;

public class Example13(
  IDependency10 dependency10
) {
  private IDependency10 Dependency10 => dependency10;

  public async Task Get(int id) => await Dependency10.Get(("id", id.ToString()));
}