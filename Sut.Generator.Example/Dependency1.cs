namespace Sut.Generator.Example;

public interface IDependency1
{
  Task<Command> Get(int id);
}
