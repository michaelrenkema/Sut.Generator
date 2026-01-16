using Sut;
using Sut.Generator.Example;

namespace Test;

[Sut<Example14>]
public partial class Test11
{
  [Test]
  public async Task Success()
  {
    var command = new Command2 { Id = 1 };

    var sut = this.Sut
      .With_Dependency11_Get(
        x => x != null && x.Id == command.Id,
        command
      )
      .Build();

    await sut.Get(command.Id);
  }
}
