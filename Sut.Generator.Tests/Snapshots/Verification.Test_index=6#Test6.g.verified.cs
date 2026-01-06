//HintName: Test6.g.cs
#nullable enable

using Moq;
using Sut;
using Sut.Generator.Example;

namespace Test;

public partial class Test6 {
  private Builder Sut { get; } = new();

  private class Builder {
    public Mock<IDependency3> Dependency3 { get; } = new();

    public Builder With_Dependency3_Run(
      global::System.Linq.Expressions.Expression<global::System.Func<Command, global::System.Boolean>> command
    ) {
      Dependency3
        .Setup(x =>
          x.Run(
            It.Is<Command>(command)
          )
        )
        .Verifiable();
      return this;
    }

    public Builder With_Dependency3_Run_Exception(
      global::System.Linq.Expressions.Expression<global::System.Func<Command, global::System.Boolean>> command,
      global::System.Exception exception
    ) {
      Dependency3
        .Setup(x =>
          x.Run(
            It.Is<Command>(command)
          )
        )
        .Throws(exception);
      return this;
    }

    public Example6 Build() {
      return new Example6(
        Dependency3.Object
      );
    }
  }
}
