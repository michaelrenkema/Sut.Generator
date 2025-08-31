//HintName: Test7.g.cs
#nullable enable

using Moq;
using Sut;
using Sut.Generator.Example;

namespace Test;

public partial class Test7 {
  private Builder Sut { get; } = new();

  private class Builder {
    public Mock<Dependency5> Dependency5 { get; } = new();

    public Builder With_Dependency5_Update<T>(
      System.Linq.Expressions.Expression<System.Func<T, System.Boolean>> input,
      T returns
    ) {
      Dependency5
        .Setup(x =>
          x.Update(
            It.Is<T>(input)
          )
        )
        .ReturnsAsync(returns)
        .Verifiable();
      return this;
    }

    public Builder With_Dependency5_Update_Exception<T>(
      System.Linq.Expressions.Expression<System.Func<T, System.Boolean>> input,
      System.Exception exception
    ) {
      Dependency5
        .Setup(x =>
          x.Update(
            It.Is<T>(input)
          )
        )
        .ThrowsAsync(exception);
      return this;
    }

    public Example7 Build() {
      return new Example7(
        Dependency5.Object
      );
    }
  }
}
