namespace Sut.Generator.Example;

public class Example5<T>(
  IDependency2<T> dependency2
) {
  private IDependency2<T> Dependency2 => dependency2;

  public List<T> Get()
  {
    return Dependency2.Get();
  }
}