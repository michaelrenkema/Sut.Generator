//HintName: Test11.g.cs
#nullable enable

using Moq;
using Sut;
using Sut.Generator.Example;

namespace Test;

public partial class Test11 {
  private Builder Sut { get; } = new();

  private class Builder {
    public Mock<IDependency8<Command>> Dependency8 { get; } = new();

    public Builder With_Dependency8_Get(
      global::System.Linq.Expressions.Expression<global::System.Func<Command, global::System.Boolean>> input,
      global::System.Nullable<Command> returns
    ) {
      Dependency8
        .Setup(x =>
          x.Get(
            It.Is<Command>(input)
          )
        )
        .ReturnsAsync(returns)
        .Verifiable();
      return this;
    }

    public Builder With_Dependency8_Get_Exception(
      global::System.Linq.Expressions.Expression<global::System.Func<Command, global::System.Boolean>> input,
      global::System.Exception exception
    ) {
      Dependency8
        .Setup(x =>
          x.Get(
            It.Is<Command>(input)
          )
        )
        .ThrowsAsync(exception);
      return this;
    }

    public Example11 Build() {
      return new Example11(
        Dependency8.Object
      );
    }
  }
}
