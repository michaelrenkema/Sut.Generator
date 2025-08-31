using Microsoft.Extensions.Options;

namespace Sut.Generator.Example;

public class Example1(
  IOptions<TestOptions> options
)
{
  private IOptions<TestOptions> Options => options;

  public TestOptions? GetOptions()
  {
    return Options.Value;
  }
}
