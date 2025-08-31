//HintName: Test5.g.cs
#nullable enable

using Moq;
using Sut;
using Sut.Generator.Example;

namespace Test;

public partial class Test5 {
  private Builder<Command> Sut { get; } = new();

  private class Builder<Command> {
    public Mock<IDependency2<Command>> Dependency2 { get; } = new();

    public Builder<Command> With_Dependency2_Get(
      System.Collections.Generic.List<Command> returns
    ) {
      Dependency2
        .Setup(x =>
          x.Get()
        )
        .Returns(returns)
        .Verifiable();
      return this;
    }

    public Builder<Command> With_Dependency2_Get_Exception(
      System.Exception exception
    ) {
      Dependency2
        .Setup(x =>
          x.Get()
        )
        .Throws(exception);
      return this;
    }

    public Example5<Command> Build() {
      return new Example5<Command>(
        Dependency2.Object
      );
    }
  }
}
