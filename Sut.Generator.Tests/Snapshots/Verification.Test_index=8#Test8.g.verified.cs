//HintName: Test8.g.cs
#nullable enable

using Moq;
using Sut;
using Sut.Generator.Example;

namespace Test;

public partial class Test8 {
  private Builder Sut { get; } = new();

  private class Builder {
    public Mock<Dependency4> Dependency4 { get; } = new();

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

    public Example8 Build() {
      return new Example8(
        Dependency4.Object
      );
    }
  }
}
