using Sut;
using Sut.Generator.Example;

namespace Test;

[Sut<Example1>]
public partial class Test1
{
  [Test]
  public async Task Success()
  {
    TestOptions options = new() { Option1 = "test" };

    var sut = this.Sut
      .With_Options(options)
      .Build();

    var result = sut.GetOptions();

    await Assert.That(result).IsNotNull();
    await Assert.That(result!.Option1).IsEqualTo(options.Option1);
  }
}