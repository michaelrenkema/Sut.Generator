//HintName: Test1.g.cs
#nullable enable

using Moq;
using Sut;
using Sut.Generator.Example;

namespace Test;

public partial class Test1 {
  private Builder Sut { get; } = new();

  private class Builder {
    public global::Microsoft.Extensions.Options.IOptions<TestOptions>? Options { get; private set; }

    public Builder With_Options(
      TestOptions options
    ) {
      Options = global::Microsoft.Extensions.Options.Options.Create(options);
      return this;
    }

    public Example1 Build() {
      return new Example1(
        Options!
      );
    }
  }
}
