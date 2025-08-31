namespace Sut.Generator;

public readonly struct SutDependencyMemberParameter(string name, string? typeName, string? typeNamespace, EquatableList<SutTypeArgument> typeArguments)
{
  public string Name { get; init; } = name;
  public string? TypeName { get; init; } = typeName;
  public string? TypeNamespace { get; init; } = typeNamespace;
  public EquatableList<SutTypeArgument> TypeArguments { get; init; } = typeArguments;

  public bool Equals(SutDependencyMemberParameter other)
  {
    return this.Name == other.Name &&
      !(this.TypeName is null || other.TypeName is null || this.TypeName != other.TypeName) &&
      !(this.TypeNamespace is null || other.TypeNamespace is null || this.TypeNamespace != other.TypeNamespace) &&
      this.TypeArguments.Equals(other.TypeArguments);
  }
}

