//HintName: Test9.g.cs
#nullable enable

using Moq;
using Sut;
using Sut.Generator.Example;

namespace Test;

public partial class Test9 {
  private Builder Sut { get; } = new();

  private class Builder {
    public Mock<Dependency6> Dependency6 { get; } = new();

    public Builder With_Dependency6_Evaluate(
      System.Linq.Expressions.Expression<System.Func<System.Func<Command, System.Boolean>, System.Boolean>> predicate,
      System.Linq.Expressions.Expression<System.Func<System.Threading.CancellationToken, System.Boolean>> cancellationToken,
      System.Boolean returns
    ) {
      Dependency6
        .Setup(x =>
          x.Evaluate(
            It.Is<System.Func<Command, System.Boolean>>(predicate),
            It.Is<System.Threading.CancellationToken>(cancellationToken)
          )
        )
        .Returns(returns)
        .Verifiable();
      return this;
    }

    public Builder With_Dependency6_Evaluate_Exception(
      System.Linq.Expressions.Expression<System.Func<System.Func<Command, System.Boolean>, System.Boolean>> predicate,
      System.Linq.Expressions.Expression<System.Func<System.Threading.CancellationToken, System.Boolean>> cancellationToken,
      System.Exception exception
    ) {
      Dependency6
        .Setup(x =>
          x.Evaluate(
            It.Is<System.Func<Command, System.Boolean>>(predicate),
            It.Is<System.Threading.CancellationToken>(cancellationToken)
          )
        )
        .Throws(exception);
      return this;
    }

    public Example9 Build() {
      return new Example9(
        Dependency6.Object
      );
    }
  }
}
