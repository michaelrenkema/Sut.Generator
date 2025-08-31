namespace Sut.Generator.Example;

public enum Status
{
  Success,
  Failure,
  Pending
}

public readonly record struct Command(int Id, Status Status = Status.Pending);
