namespace Sut.Generator;

public readonly struct Sut(string name, string @namespace, string source, string sourceVersion, string testClassName, string testClassNamespace, EquatableList<SutTypeArgument> typeArguments, EquatableList<SutDependency> dependencies)
    : IEquatable<Sut>
{
    public string Name { get; init; } = name;
    public string Namespace { get; init; } = @namespace;
    public string Source { get; init; } = source;
    public string SourceVersion { get; init; } = sourceVersion;
    public string TestClassName { get; init; } = testClassName;
    public string TestClassNamespace { get; init; } = testClassNamespace;
    public EquatableList<SutTypeArgument> TypeArguments { get; init; } = typeArguments;
    public EquatableList<SutDependency> Dependencies { get; init; } = dependencies;

    public bool Equals(Sut other)
    {
        return this.Name == other.Name &&
               this.Namespace == other.Namespace &&
               this.Source == other.Source &&
               this.SourceVersion == other.SourceVersion &&
               this.TestClassName == other.TestClassName &&
               this.TestClassNamespace == other.TestClassNamespace &&
               this.TypeArguments.Equals(other.TypeArguments) &&
               this.Dependencies.Equals(other.Dependencies);
    }
}
