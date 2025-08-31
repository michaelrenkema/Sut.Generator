using Sut;
using Sut.Generator.Example;

namespace Test;

[Sut<Example6>]
public partial class Test6
{
  [Test]
  public void Success()
  {
    var command = new Command(1);

    var sut = this.Sut
      .With_Dependency3_Run(c => c.Id == command.Id)
      .Build();

    sut.Run(command);

    this.Sut.Dependency3.Verify();
  }

  [Test]
  public async Task Failure()
  {
    var command = new Command(1);
    var exception = new Exception("test");

    var sut = this.Sut
      .With_Dependency3_Run_Exception(c => c.Id == command.Id, exception)
      .Build();

    try
    {
      sut.Run(command);
    }
    catch (Exception ex)
    {
      await Assert.That(ex).IsEqualTo(exception);
    }
  }
}