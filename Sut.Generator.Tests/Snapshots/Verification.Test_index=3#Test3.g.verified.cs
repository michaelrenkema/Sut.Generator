//HintName: Test3.g.cs
#nullable enable

using Moq;
using Sut;
using Sut.Generator.Example;

namespace Test;

public partial class Test3 {
  private Builder Sut { get; } = new();

  private class Builder {
    public Microsoft.Extensions.Options.IOptions<TestOptions>? Options { get; private set; }
    public Mock<Microsoft.Extensions.Logging.ILogger<Example3>> Logger { get; } = new();
    public Mock<IDependency1> Dependency1 { get; } = new();
    public Mock<IDependency3> Dependency3 { get; } = new();
    public Mock<Dependency4> Dependency4 { get; } = new();
    public Mock<Dependency5> Dependency5 { get; } = new();
    public Mock<Dependency6> Dependency6 { get; } = new();

    public Builder With_Options(
      TestOptions options
    ) {
      Options = Microsoft.Extensions.Options.Options.Create(options);
      return this;
    }

    public Builder With_Logger(
      Microsoft.Extensions.Logging.LogLevel logLevel,
      string message,
      System.Exception? exception = null
    ) {
      Logger
        .Setup(x =>
          x.Log(
            It.Is<Microsoft.Extensions.Logging.LogLevel>(l => l == logLevel),
            It.IsAny<Microsoft.Extensions.Logging.EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString() == message),
            It.Is<System.Exception?>(e => e == exception),
            It.IsAny<System.Func<It.IsAnyType, System.Exception?, string>>()
          )
        )
        .Verifiable();
      return this;
    }

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

    public Builder With_Dependency3_Run(
      System.Linq.Expressions.Expression<System.Func<Command, System.Boolean>> command
    ) {
      Dependency3
        .Setup(x =>
          x.Run(
            It.Is<Command>(command)
          )
        )
        .Verifiable();
      return this;
    }

    public Builder With_Dependency3_Run_Exception(
      System.Linq.Expressions.Expression<System.Func<Command, System.Boolean>> command,
      System.Exception exception
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
      System.Action<Dependency4> status
    ) {
      Dependency4
        .SetupSet(status)
        .Verifiable();
      return this;
    }

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

    public Example3 Build() {
      return new Example3(
        Options!,
        Logger.Object,
        Dependency1.Object,
        Dependency3.Object,
        Dependency4.Object,
        Dependency5.Object,
        Dependency6.Object
      );
    }
  }
}
