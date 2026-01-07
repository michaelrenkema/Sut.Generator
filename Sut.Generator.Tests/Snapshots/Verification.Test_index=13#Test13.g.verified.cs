//HintName: Test13.g.cs
#nullable enable

using Moq;
using Sut;
using Sut.Generator.Example;

namespace Test;

public partial class Test13 {
  private Builder Sut { get; } = new();

  private class Builder {
    public Mock<IDependency10> Dependency10 { get; } = new();

    public Builder With_Dependency10_Get(
      global::System.Linq.Expressions.Expression<global::System.Func<global::System.ValueTuple<global::System.String, global::System.String>[], global::System.Boolean>> parameters
    ) {
      Dependency10
        .Setup(x =>
          x.Get(
            It.Is<global::System.ValueTuple<global::System.String, global::System.String>[]>(parameters)
          )
        )
        .Verifiable();
      return this;
    }

    public Builder With_Dependency10_Get_Exception(
      global::System.Linq.Expressions.Expression<global::System.Func<global::System.ValueTuple<global::System.String, global::System.String>[], global::System.Boolean>> parameters,
      global::System.Exception exception
    ) {
      Dependency10
        .Setup(x =>
          x.Get(
            It.Is<global::System.ValueTuple<global::System.String, global::System.String>[]>(parameters)
          )
        )
        .ThrowsAsync(exception);
      return this;
    }

    public Example13 Build() {
      return new Example13(
        Dependency10.Object
      );
    }
  }
}
