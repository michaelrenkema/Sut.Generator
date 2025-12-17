using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Sut.Generator.Example;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Sut.Generator.Tests;

public class Verification
{
  private static Dictionary<int, string> Tests { get; }

  static Verification() {
    Tests = new Dictionary<int, string>()
    {
      {
        1,
        """
using Sut;
using Sut.Generator.Example;

namespace Test;

[Sut<Example1>]
public partial class Test1 {}
"""
      },
      {
        2,
        """
using Sut;
using Sut.Generator.Example;

namespace Test;

[Sut<Example2>]
public partial class Test2 {}
"""
      },
      {
        3,
        """
using Sut;
using Sut.Generator.Example;
using Microsoft.Extensions.Logging;

namespace Test;

[Sut<Example3>]
public partial class Test3 {}
"""
      },
      {
        4,
        """
using Sut;
using Sut.Generator.Example;

namespace Test;

[Sut<Example4>]
public partial class Test4 {}
"""
      },
      {
        5,
        """
using Sut;
using Sut.Generator.Example;

namespace Test;

[Sut<Example5<Command>>]
public partial class Test5 {}
"""
      },
      {
        6,
        """
using Sut;
using Sut.Generator.Example;

namespace Test;

[Sut<Example6>]
public partial class Test6 {}
"""
      },
      {
        7,
        """
using Sut;
using Sut.Generator.Example;

namespace Test;

[Sut<Example7>]
public partial class Test7 {}
"""
      },
      {
        8,
        """
using Sut;
using Sut.Generator.Example;

namespace Test;

[Sut<Example8>]
public partial class Test8 {}
"""
      },
      {
        9,
        """
using Sut;
using Sut.Generator.Example;

namespace Test;

[Sut<Example9>]
public partial class Test9 {}
"""
      },
      {
        10,
        """
using Sut;
using Sut.Generator.Example;

namespace Test;

[Sut<Example10>]
public partial class Test10 {}
"""
      },
      {
        11,
        """
using Sut;
using Sut.Generator.Example;

namespace Test;

[Sut<Example11>]
public partial class Test11 {}
"""
      }
    };
  }

  [Test]
  [Arguments(1)]
  [Arguments(2)]
  [Arguments(3)]
  [Arguments(4)]
  [Arguments(5)]
  [Arguments(6)]
  [Arguments(7)]
  [Arguments(8)]
  [Arguments(9)]
  [Arguments(10)]
  [Arguments(11)]
  public Task Test(int index)
  {
    var source = Tests[index];

    var syntaxTree = CSharpSyntaxTree.ParseText(source);

    IEnumerable<PortableExecutableReference> references =
    [
      MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
      MetadataReference.CreateFromFile(typeof(CancellationToken).Assembly.Location),
      MetadataReference.CreateFromFile(typeof(IOptions<>).Assembly.Location),
      MetadataReference.CreateFromFile(typeof(ILogger).Assembly.Location),
      MetadataReference.CreateFromFile(typeof(Command).Assembly.Location)
    ];

    var compilation = CSharpCompilation.Create(
      assemblyName: "Test",
      syntaxTrees: [syntaxTree],
      references: references);

    var generator = new SutGenerator();

    GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

    driver = driver.RunGenerators(compilation);

    VerifierSettings.DisableRequireUniquePrefix();

    return Verify(driver)
      .UseDirectory("Snapshots");
  }
}
