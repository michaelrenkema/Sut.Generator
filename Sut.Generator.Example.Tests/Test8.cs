using Sut;
using Sut.Generator.Example;
using Moq;

namespace Test;

[Sut<Example8>]
public partial class Test8
{
  [Test]
  [Arguments(Status.Success)]
  [Arguments(Status.Failure)]
  [Arguments(Status.Pending)]
  public async Task Ok(Status status)
  {
    var sut = this.Sut
      .With_Dependency4_Status(status)
      .With_Dependency4_Status(x => x.Status = status)
      .Build();

    await Assert.That(sut.Ok).IsEqualTo(status == Status.Success);

    this.Sut.Dependency4.VerifyGet(x => x.Status, Times.Once);
    this.Sut.Dependency4.VerifySet(x => x.Status = status, Times.Never);
  }

  [Test]
  [Arguments(Status.Success)]
  [Arguments(Status.Failure)]
  [Arguments(Status.Pending)]
  public void Update(Status status)
  {
    var sut = this.Sut
      .With_Dependency4_Status(status)
      .With_Dependency4_Status(x => x.Status = status)
      .Build();

    sut.Update(status);

    this.Sut.Dependency4.VerifyGet(x => x.Status, Times.Never);
    this.Sut.Dependency4.VerifySet(x => x.Status = status, Times.Once);
  }
}
