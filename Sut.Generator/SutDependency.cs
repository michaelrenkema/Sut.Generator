using Microsoft.CodeAnalysis;

namespace Sut.Generator;

public readonly struct SutDependency(string name, string typeName, string typeNamespace, TypeKind typeKind, EquatableList<SutTypeArgument> typeArguments, string source, string sourceVersion, bool isSealed, EquatableList<SutDependencyMember> members)
    : IEquatable<SutDependency>
{
    public string Name { get; init; } = name;
    public string TypeName { get; init; } = typeName;
    public string TypeNamespace { get; init; } = typeNamespace;
    public TypeKind TypeKind { get; init; } = typeKind;
    public EquatableList<SutTypeArgument> TypeArguments { get; init; } = typeArguments;
    public string Source { get; init; } = source;
    public string SourceVersion { get; init; } = sourceVersion;
    public bool IsSealed { get; init; } = isSealed;
    public EquatableList<SutDependencyMember> Members { get; init; } = members;

    public bool Equals(SutDependency other)
    {
        return this.Name == other.Name &&
               this.TypeName == other.TypeName &&
               this.TypeNamespace == other.TypeNamespace &&
               this.TypeKind == other.TypeKind &&
               this.Source == other.Source &&
               this.SourceVersion == other.SourceVersion &&
               this.TypeArguments.Equals(other.TypeArguments) &&
               this.IsSealed == other.IsSealed &&
               this.Members.Equals(other.Members);
    }
}

