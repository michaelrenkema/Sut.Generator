//HintName: Test12.g.cs
#nullable enable

using Moq;
using Sut;
using Sut.Generator.Example;

namespace Test;

public partial class Test12 {
  private Builder Sut { get; } = new();

  private class Builder {
    public Mock<IDependency9<Command2>> Dependency9 { get; } = new();

    public Builder With_Dependency9_Get(
      global::System.Linq.Expressions.Expression<global::System.Func<Command2, global::System.Boolean>> input,
      Command2 returns
    ) {
      Dependency9
        .Setup(x =>
          x.Get(
            It.Is<Command2>(input)
          )
        )
        .ReturnsAsync(returns)
        .Verifiable();
      return this;
    }

    public Builder With_Dependency9_Get_Exception(
      global::System.Linq.Expressions.Expression<global::System.Func<Command2, global::System.Boolean>> input,
      global::System.Exception exception
    ) {
      Dependency9
        .Setup(x =>
          x.Get(
            It.Is<Command2>(input)
          )
        )
        .ThrowsAsync(exception);
      return this;
    }

    public Example12 Build() {
      return new Example12(
        Dependency9.Object
      );
    }
  }
}
