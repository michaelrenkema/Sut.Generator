namespace Sut.Generator.Example;

public interface IDependency10
{
  Task Get(params (string key, string value)[] parameters);
}
