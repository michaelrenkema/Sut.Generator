using System.Text;

namespace Sut.Generator;

public static class Extensions
{
  public static bool IsOptions(this SutDependency? dependency)
  {
    return dependency is not null && dependency?.TypeNamespace == "Microsoft.Extensions.Options" && dependency?.TypeName == "IOptions";
  }

  public static bool IsOptions(this SutDependency dependency)
  {
    return dependency.TypeNamespace == "Microsoft.Extensions.Options" && dependency.TypeName == "IOptions";
  }

  public static bool IsLogger(this SutDependency? dependency)
  {
    return dependency is not null && dependency?.TypeNamespace == "Microsoft.Extensions.Logging" && dependency?.TypeName == "ILogger";
  }

  public static bool IsLogger(this SutDependency dependency)
  {
    return dependency.TypeNamespace == "Microsoft.Extensions.Logging" && dependency.TypeName == "ILogger";
  }

  public static string Format(this List<SutTypeArgument> typeArguments, Sut sut)
  {
    var sb = new StringBuilder();
    if (typeArguments.Count > 0)
    {
      sb.Append("<");
      for (var i = 0; i < typeArguments.Count; i++)
      {
        var typeArgument = typeArguments[i];
        if (sut.TypeArguments.Any(t => t.TypeParameter == typeArgument.TypeArgument))
        {
          var sutTypeArgument = sut.TypeArguments.First(t => t.TypeParameter == typeArgument.TypeArgument);
          if (!string.IsNullOrEmpty(sutTypeArgument.TypeArgumentNamespace)
            && sutTypeArgument.TypeArgumentNamespace != sut.Namespace)
          {
            sb.Append($"global::{sutTypeArgument.TypeArgumentNamespace}.");
          }
          sb.Append(sutTypeArgument.TypeArgument);
        }
        else
        {
          if (!string.IsNullOrEmpty(typeArgument.TypeArgumentNamespace)
            && typeArgument.TypeArgumentNamespace != sut.Namespace)
          {
            sb.Append($"global::{typeArgument.TypeArgumentNamespace}.");
          }
          sb.Append($"{typeArguments[i].TypeArgument}");
        }
        if (i < typeArguments.Count - 1)
          sb.Append(", ");
      }
      sb.Append(">");
    }
    return sb.ToString();
  }
}