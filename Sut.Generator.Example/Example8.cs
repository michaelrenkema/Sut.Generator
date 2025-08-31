namespace Sut.Generator.Example;

public class Example8(
  Dependency4 dependency4
) {
  private Dependency4 Dependency4 => dependency4;

  public void Update(Status status) => Dependency4.Status = status;

  public bool Ok => Dependency4.Status == Status.Success;
}