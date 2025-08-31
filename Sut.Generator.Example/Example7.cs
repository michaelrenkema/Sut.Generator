namespace Sut.Generator.Example;

public class Example7(
  Dependency5 dependency5
) {
  private Dependency5 Dependency5 => dependency5;

  public async Task<Command> Update(Command command)
  {
    return await Dependency5.Update(command);
  }
}