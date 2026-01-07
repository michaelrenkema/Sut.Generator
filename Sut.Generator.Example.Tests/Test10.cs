using Sut;
using Sut.Generator.Example;

namespace Test;

[Sut<Example13>]
public partial class Test10
{
  [Test]
  public async Task Success()
  {
    var id = 1;

    var sut = this.Sut
      .With_Dependency10_Get(x => x.Length == 1)
      .Build();

    await sut.Get(id);
  }
}
