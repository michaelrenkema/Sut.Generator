//HintName: Test2.g.cs
#nullable enable

using Moq;
using Sut;
using Sut.Generator.Example;

namespace Test;

public partial class Test2 {
  private Builder Sut { get; } = new();

  private class Builder {
    public Mock<Microsoft.Extensions.Logging.ILogger<Example2>> Logger { get; } = new();

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

    public Example2 Build() {
      return new Example2(
        Logger.Object
      );
    }
  }
}
