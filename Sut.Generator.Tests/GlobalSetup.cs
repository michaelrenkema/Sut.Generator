using System.Runtime.CompilerServices;

[assembly: Retry(3)]
[assembly: System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]

namespace Sut.Generator.Tests;

public class GlobalSetup
{
  [ModuleInitializer]
  public static void Init() =>
    VerifySourceGenerators.Initialize();
}