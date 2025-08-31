//HintName: Test4.g.cs
#nullable enable

using Moq;
using Sut;
using Sut.Generator.Example;

namespace Test;

public partial class Test4 {
  private Builder Sut { get; } = new();

  private class Builder {
    public Mock<IDependency1> Dependency1 { get; } = new();

    public Builder With_Dependency1_Get(
      System.Linq.Expressions.Expression<System.Func<System.Int32, System.Boolean>> id,
      Command returns
    ) {
      Dependency1
        .Setup(x =>
          x.Get(
            It.Is<System.Int32>(id)
          )
        )
        .ReturnsAsync(returns)
        .Verifiable();
      return this;
    }

    public Builder With_Dependency1_Get_Exception(
      System.Linq.Expressions.Expression<System.Func<System.Int32, System.Boolean>> id,
      System.Exception exception
    ) {
      Dependency1
        .Setup(x =>
          x.Get(
            It.Is<System.Int32>(id)
          )
        )
        .ThrowsAsync(exception);
      return this;
    }

    public Example4 Build() {
      return new Example4(
        Dependency1.Object
      );
    }
  }
}
