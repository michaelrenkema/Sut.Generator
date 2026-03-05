//HintName: Test2.g.cs
#nullable enable

using Moq;
using Sut;
using Sut.Generator.Example;

namespace Test;

public partial class Test2 {
  public Builder Sut { get; } = new();

  public class Builder {
    public global::Microsoft.Extensions.Logging.ILogger<Example2>? Logger { get; private set; }

    public Builder With_Logger(
      global::Microsoft.Extensions.Logging.ILogger<Example2> logger
    ) {
      Logger = logger;
      return this;
    }

    public Example2 Build() {
      return new Example2(
        Logger!
      );
    }
  }
}
