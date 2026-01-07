namespace Sut.Generator.Example;

public interface IDependency9<T> where T : class
{
  Task<T?> Get(T input);
}
