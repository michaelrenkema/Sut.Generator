namespace Sut.Generator.Example;

public class Dependency5
{
  public virtual async Task<T> Update<T>(T input)
  {
    return await Task.FromResult(input);
  }
}