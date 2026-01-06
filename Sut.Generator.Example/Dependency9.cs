namespace Sut.Generator.Example;

public interface IDependency9<T> where T : class
{
  Task<T?> Get(T input);
}

public class Dependency9
  : IDependency9<Command2>
{
  public async Task<Command2?> Get(Command2 input)
  {
    return await Task.FromResult(input);
  }
}