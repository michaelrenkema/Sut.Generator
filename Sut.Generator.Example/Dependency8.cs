namespace Sut.Generator.Example;

public interface IDependency8<T> where T : struct
{
  Task<T?> Get(T input);
}
