using Sut;
using Sut.Generator.Example;

namespace Test;

[Sut<Example4>]
public partial class Test4
{
  [Test]
  public async Task Success()
  {
    var command = new Command(1);

    var sut = this.Sut
      .With_Dependency1_Get(i => i == command.Id, command)
      .Build();

    var result = await sut.Get(command.Id);

    await Assert.That(result).IsEqualTo(command);
  }

  [Test]
  public async Task Failure()
  {
    var command = new Command(1);
    var exception = new Exception("test");

    var sut = this.Sut
      .With_Dependency1_Get_Exception(i => i == command.Id, exception)
      .Build();

    await Assert.ThrowsAsync<Exception>(sut.Get(command.Id));
  }
}