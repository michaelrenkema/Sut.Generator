//HintName: Test10.g.cs
#nullable enable

using Moq;
using Sut;
using Sut.Generator.Example;

namespace Test;

public partial class Test10 {
  private Builder Sut { get; } = new();

  private class Builder {
    public Mock<IDependency7> Dependency7 { get; } = new();

    public Builder With_Dependency7_Get(
      global::System.Linq.Expressions.Expression<global::System.Func<global::System.Int32[], global::System.Boolean>> ids,
      Command returns
    ) {
      Dependency7
        .Setup(x =>
          x.Get(
            It.Is<global::System.Int32[]>(ids)
          )
        )
        .ReturnsAsync(returns)
        .Verifiable();
      return this;
    }

    public Builder With_Dependency7_Get_Exception(
      global::System.Linq.Expressions.Expression<global::System.Func<global::System.Int32[], global::System.Boolean>> ids,
      global::System.Exception exception
    ) {
      Dependency7
        .Setup(x =>
          x.Get(
            It.Is<global::System.Int32[]>(ids)
          )
        )
        .ThrowsAsync(exception);
      return this;
    }

    public Example10 Build() {
      return new Example10(
        Dependency7.Object
      );
    }
  }
}
