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
      global::System.Linq.Expressions.Expression<global::System.Func<global::System.Func<Command, global::System.Boolean>, global::System.Boolean>> predicate,
      global::System.Linq.Expressions.Expression<global::System.Func<global::System.Threading.CancellationToken, global::System.Boolean>> cancellationToken,
      global::System.Boolean returns,
      global::System.Action<global::System.Func<Command, global::System.Boolean>, global::System.Threading.CancellationToken>? callback = null
    ) {
      var setup = Dependency6
        .Setup(x =>
          x.Evaluate(
            It.Is<global::System.Func<Command, global::System.Boolean>>(predicate),
            It.Is<global::System.Threading.CancellationToken>(cancellationToken)
          )
        )
        .Returns(returns);

      if (callback is not null)
        setup.Callback(callback);
      else
        setup.Verifiable();

      return this;
    }

    public Builder With_Dependency6_Evaluate_Exception(
      global::System.Linq.Expressions.Expression<global::System.Func<global::System.Func<Command, global::System.Boolean>, global::System.Boolean>> predicate,
      global::System.Linq.Expressions.Expression<global::System.Func<global::System.Threading.CancellationToken, global::System.Boolean>> cancellationToken,
      global::System.Exception exception
    ) {
      Dependency6
        .Setup(x =>
          x.Evaluate(
            It.Is<global::System.Func<Command, global::System.Boolean>>(predicate),
            It.Is<global::System.Threading.CancellationToken>(cancellationToken)
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
