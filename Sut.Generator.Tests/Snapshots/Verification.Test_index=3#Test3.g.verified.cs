//HintName: Test3.g.cs
#nullable enable

using Moq;
using Sut;
using Sut.Generator.Example;

namespace Test;

public partial class Test3 {
  private Builder Sut { get; } = new();

  private class Builder {
    public global::Microsoft.Extensions.Options.IOptions<TestOptions>? Options { get; private set; }
    public global::Microsoft.Extensions.Logging.ILogger<Example3>? Logger { get; private set; }
    public Mock<IDependency1> Dependency1 { get; } = new();
    public Mock<IDependency3> Dependency3 { get; } = new();
    public Mock<Dependency4> Dependency4 { get; } = new();
    public Mock<Dependency5> Dependency5 { get; } = new();
    public Mock<Dependency6> Dependency6 { get; } = new();

    public Builder With_Options(
      TestOptions options
    ) {
      Options = global::Microsoft.Extensions.Options.Options.Create(options);
      return this;
    }

    public Builder With_Logger(
      global::Microsoft.Extensions.Logging.ILogger<Example3> logger
    ) {
      Logger = logger;
      return this;
    }

    public Builder With_Dependency1_Get(
      global::System.Linq.Expressions.Expression<global::System.Func<global::System.Int32, global::System.Boolean>> id,
      Command returns,
      global::System.Action<global::System.Int32>? callback = null
    ) {
      var setup = Dependency1
        .Setup(x =>
          x.Get(
            It.Is<global::System.Int32>(id)
          )
        )
        .ReturnsAsync(returns);

      if (callback is not null)
        setup.Callback(callback);
      else
        setup.Verifiable();

      return this;
    }

    public Builder With_Dependency1_Get_Exception(
      global::System.Linq.Expressions.Expression<global::System.Func<global::System.Int32, global::System.Boolean>> id,
      global::System.Exception exception
    ) {
      Dependency1
        .Setup(x =>
          x.Get(
            It.Is<global::System.Int32>(id)
          )
        )
        .ThrowsAsync(exception);
      return this;
    }

    public Builder With_Dependency3_Run(
      global::System.Linq.Expressions.Expression<global::System.Func<Command, global::System.Boolean>> command,
      global::System.Action<Command>? callback = null
    ) {
      var setup = Dependency3
        .Setup(x =>
          x.Run(
            It.Is<Command>(command)
          )
        );

      if (callback is not null)
        setup.Callback(callback);
      else
        setup.Verifiable();

      return this;
    }

    public Builder With_Dependency3_Run_Exception(
      global::System.Linq.Expressions.Expression<global::System.Func<Command, global::System.Boolean>> command,
      global::System.Exception exception
    ) {
      Dependency3
        .Setup(x =>
          x.Run(
            It.Is<Command>(command)
          )
        )
        .Throws(exception);
      return this;
    }

    public Builder With_Dependency4_Status(
      Status status
    ) {
      Dependency4
        .SetupGet(x => x.Status)
        .Returns(status)
        .Verifiable();
      return this;
    }

    public Builder With_Dependency4_Status(
      global::System.Action<Dependency4> status
    ) {
      Dependency4
        .SetupSet(status)
        .Verifiable();
      return this;
    }

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

    public Example3 Build() {
      return new Example3(
        Options!,
        Logger!,
        Dependency1.Object,
        Dependency3.Object,
        Dependency4.Object,
        Dependency5.Object,
        Dependency6.Object
      );
    }
  }
}
