using Sut;
using Sut.Generator.Example;

namespace Test;

[Sut<Example5<Command>>]
public partial class Test5
{
  [Test]
  public async Task Success()
  {
    var commands = new List<Command> {
      new(1),
      new(2)
    };

    var sut = this.Sut
      .With_Dependency2_Get(commands)
      .Build();

    var result = sut.Get();

    await Assert.That(result).IsEqualTo(commands);
  }

  [Test]
  public async Task Failure()
  {
    var exception = new Exception("test");

    var sut = this.Sut
      .With_Dependency2_Get_Exception(exception)
      .Build();

    try
    {
      var result = sut.Get();
    }
    catch (Exception ex)
    {
      await Assert.That(ex).IsEqualTo(exception);
    }
  }
}