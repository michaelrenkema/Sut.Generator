using Sut;
using Sut.Generator.Example;

namespace Test;

[Sut<Example7>]
public partial class Test7
{
  [Test]
  public async Task Success()
  {
    var command = new Command(1);

    var sut = this.Sut
      .With_Dependency5_Update<Command>(c => c.Id == command.Id, command)
      .Build();

    await sut.Update(command);
  }
}