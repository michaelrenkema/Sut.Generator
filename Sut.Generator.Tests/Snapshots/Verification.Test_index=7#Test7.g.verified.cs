//HintName: Test7.g.cs
#nullable enable

using Moq;
using Sut;
using Sut.Generator.Example;

namespace Test;

public partial class Test7 {
  public Builder Sut { get; } = new();

  public class Builder {
    public Mock<Dependency5> Dependency5 { get; } = new();

    public Builder With_Dependency5_Update<T>(
      global::System.Linq.Expressions.Expression<global::System.Func<T, global::System.Boolean>> input,
      T returns,
      global::System.Action<T>? callback = null
    ) {
      var setup = Dependency5
        .Setup(x =>
          x.Update(
            It.Is<T>(input)
          )
        )
        .ReturnsAsync(returns);

      if (callback is not null)
        setup.Callback(callback);
      else
        setup.Verifiable();

      return this;
    }

    public Builder With_Dependency5_Update_Exception<T>(
      global::System.Linq.Expressions.Expression<global::System.Func<T, global::System.Boolean>> input,
      global::System.Exception exception
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
