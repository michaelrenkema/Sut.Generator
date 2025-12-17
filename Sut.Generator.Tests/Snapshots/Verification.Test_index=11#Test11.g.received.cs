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
      System.Linq.Expressions.Expression<System.Func<T, System.Boolean>> input,
      System.Nullable<T> returns
    ) {
      Dependency8
        .Setup(x =>
          x.Get(
            It.Is<T>(input)
          )
        )
        .ReturnsAsync(returns)
        .Verifiable();
      return this;
    }

    public Builder With_Dependency8_Get_Exception(
      System.Linq.Expressions.Expression<System.Func<T, System.Boolean>> input,
      System.Exception exception
    ) {
      Dependency8
        .Setup(x =>
          x.Get(
            It.Is<T>(input)
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
