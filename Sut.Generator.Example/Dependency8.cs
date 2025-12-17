namespace Sut.Generator.Example;

public interface IDependency8<T> where T : struct
{
  Task<T?> Get(T input);
}

public class Dependency8
  : IDependency8<Command>
{
  public async Task<Command?> Get(Command input)
  {
    return await Task.FromResult(input);
  }
}