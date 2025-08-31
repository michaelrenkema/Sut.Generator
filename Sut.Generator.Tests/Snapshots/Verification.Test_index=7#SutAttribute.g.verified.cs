//HintName: SutAttribute.g.cs
namespace Sut;

[System.AttributeUsage(System.AttributeTargets.Class)]
public class SutAttribute<T>() : System.Attribute {}