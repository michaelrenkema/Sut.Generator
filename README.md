# Sut.Generator

This project is intended to reduce the amount of boilerplate code required to setup mocks for unit testing in C# .NET projects, and to reduce code duplication often present in mock setup across individual tests. It uses an incremental generator to add code for setup of mocks using a fluent builder. Special cases have been added for generics, IOptions from the Microsoft.Extensions.Options namespace, and ILogger from the Microsoft.Extensions.Logging namespace.

## Installation

    dotnet add package Sut.Generator --prerelease

## Usage
Adding the [Sut<Class>] attribute to a unit test class, where the class name is the type being tested, will result in a fluent builder being generated to assist with setup of mocks. The incremental generator will inspect all dependencies of the class and generate setup code for their methods.

    namespace App;

    public interface IDependency {
      string Method(string input);
    }

    public class Class(IDependency dependency) {
      IDependency Dependency => dependency;

      public string Execute(string input) {
        return Dependency.Method(input);
      }
    }

---

    using Sut;
    using App;

    namespace Test;

    [Sut<Class>]
    public partial class Tests {
      [Test]
      public async Task Success() {
        var input = "test input";
        var output = "test output";

        var sut = this.Sut
          .With_Dependency_Method(input, output)
          .Build();

        var result = sut.Execute(input);

        await Assert.That(result).IsEqualTo(output);
      }

      [Test]
      public async Task Failure() {
        var input = "test input";
        var exception = new Exception();

        var sut = this.Sut
          .With_Dependency_Method_Exception(input, exception)
          .Build();

        await Assert.ThrowsAsync<ArgumentException>(sut.Execute(input));
      }
    }

## Contributing

Contributions are welcome.

### Deployment

The incremental generator is deployed to nuget.org using GitHub Actions. Tagging a commit with a semantic version will trigger build and deployment. The tag may include a prerelease suffix.

### Local debugging

The Sut.Generator.Tests project can be used to debug the incremental generator. There are numerous unit tests setup that use the Verify library to output and compare generated code, and to provide an entry point for debugging. Simply set a breakpoint in the Sut.Generator project code then debug a unit test in the Sut.Generator.Tests project.

### Local testing

Once local debugging is complete, the following process can be executed to build and install a local NuGet package into the Sut.Generator.Example.Tests project. The unit tests in this project will test the incremental generator output in a scenario similar to that used by consumers of the NuGet package.

    cd ./Sut.Generator
    dotnet build --no-incremental
    dotnet pack --no-build --configuration Debug /p:Version=0.0.0-pre --output ./out
    rm -rf ~/.nuget/packages/sut.generator
    dotnet nuget push ./out/Sut.Generator.0.0.0-pre.nupkg --source ~/.nuget/packages
    cd ../Sut.Generator.Example.Tests
    dotnet add package Sut.Generator --prerelease --source ~/.nuget/packages
    dotnet build --no-incremental
    dotnet test