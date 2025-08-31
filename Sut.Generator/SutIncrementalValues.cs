using Microsoft.CodeAnalysis;

namespace Sut.Generator;

public readonly struct SutIncrementalValues(SyntaxNode node, Sut? sut) : IEquatable<SutIncrementalValues>
{
  public SyntaxNode Node { get; init; } = node;
  public Sut? Sut { get; init; } = sut;

  public bool Equals(SutIncrementalValues other)
  {
    return this.Sut.Equals(other.Sut);
  }
}