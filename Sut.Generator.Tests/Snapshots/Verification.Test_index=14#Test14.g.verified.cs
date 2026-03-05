//HintName: Test14.g.cs
#nullable enable

using Moq;
using Sut;
using Sut.Generator.Example;

namespace Test;

public partial class Test14 {
  public Builder Sut { get; } = new();

  public class Builder {
    public Mock<IDependency11<Command2?>> Dependency11 { get; } = new();

    public Builder With_Dependency11_Get(
      global::System.Linq.Expressions.Expression<global::System.Func<Command2?, global::System.Boolean>> input,
      Command2? returns,
      global::System.Action<Command2?>? callback = null
    ) {
      var setup = Dependency11
        .Setup(x =>
          x.Get(
            It.Is<Command2?>(input)
          )
        )
        .ReturnsAsync(returns);

      if (callback is not null)
        setup.Callback(callback);
      else
        setup.Verifiable();

      return this;
    }

    public Builder With_Dependency11_Get_Exception(
      global::System.Linq.Expressions.Expression<global::System.Func<Command2?, global::System.Boolean>> input,
      global::System.Exception exception
    ) {
      Dependency11
        .Setup(x =>
          x.Get(
            It.Is<Command2?>(input)
          )
        )
        .ThrowsAsync(exception);
      return this;
    }

    public Example14 Build() {
      return new Example14(
        Dependency11.Object
      );
    }
  }
}
