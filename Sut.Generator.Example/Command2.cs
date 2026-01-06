namespace Sut.Generator.Example;

public enum Status2
{
  Success,
  Failure,
  Pending
}

public class Command2 {
  public int Id { get; set; }
  public Status2 Status { get; set; } = Status2.Pending;
}