using Microsoft.CodeAnalysis;

namespace Sut.Generator;

public readonly struct SutDependencyMember(string name, MethodKind kind, bool isAsync, EquatableList<SutTypeArgument> typeArguments, string returnType, string returnTypeNamespace, EquatableList<SutTypeArgument> returnTypeArguments, EquatableList<SutDependencyMemberParameter> parameters)
    : IEquatable<SutDependencyMember>
{
  public string Name { get; init; } = name;
  public MethodKind Kind { get; init; } = kind;
  public bool IsAsync { get; init; } = isAsync;
  public EquatableList<SutTypeArgument> TypeArguments { get; init; } = typeArguments;
  public string? ReturnType { get; init; } = returnType;
  public string? ReturnTypeNamespace { get; init; } = returnTypeNamespace;
  public EquatableList<SutTypeArgument> ReturnTypeArguments { get; init; } = returnTypeArguments;
  public EquatableList<SutDependencyMemberParameter> Parameters { get; init; } = parameters;

  public bool Equals(SutDependencyMember other)
  {
    return this.Name == other.Name &&
      this.Kind == other.Kind &&
      this.IsAsync == other.IsAsync &&
      this.TypeArguments.Equals(other.TypeArguments) &&
      !(this.ReturnType is null || other.ReturnType is null || this.ReturnType != other.ReturnType) &&
      !(this.ReturnTypeNamespace is null || other.ReturnTypeNamespace is null || this.ReturnTypeNamespace != other.ReturnTypeNamespace) &&
      this.ReturnTypeArguments.Equals(other.ReturnTypeArguments) &&
      this.Parameters.Equals(other.Parameters);
  }
}
