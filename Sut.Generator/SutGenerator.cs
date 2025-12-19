using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.Xml.Serialization;

namespace Sut.Generator;

[Generator]
public class SutGenerator : IIncrementalGenerator
{
  public void Initialize(IncrementalGeneratorInitializationContext context)
  {
    context.RegisterPostInitializationOutput(static postInitializationContext =>
    {
      postInitializationContext.AddSource("SutAttribute.g.cs", SourceText.From(
"""
namespace Sut;

[System.AttributeUsage(System.AttributeTargets.Class)]
public class SutAttribute<T>() : System.Attribute {}
""", Encoding.UTF8));
    });

    IncrementalValuesProvider<(SutIncrementalValues? Sut, Compilation Compilation)> suts = context.SyntaxProvider
        .ForAttributeWithMetadataName(
            "Sut.SutAttribute`1",
            predicate: static (syntaxNode, _) => IsSyntaxTargetForGeneration(syntaxNode),
            transform: static (ctx, _) => GetSut(ctx.TargetNode, ctx.SemanticModel))
        .Where(static m => m is not null)
        .Combine(context.CompilationProvider);

    IncrementalValuesProvider<(SutIncrementalValues? Sut, Compilation Compilation)> dependencies = suts
      .Select((value, _) => GetDependencies(value.Sut, value.Compilation.GetSemanticModel(value.Sut?.Node.SyntaxTree!)))
      .Combine(context.CompilationProvider);

    IncrementalValuesProvider<SutIncrementalValues?> members = dependencies
      .Select((value, _) => GetDependencyMembers(value.Sut, value.Compilation.GetSemanticModel(value.Sut?.Node.SyntaxTree!)));

    context.RegisterSourceOutput(members, static (spc, source) => Execute(spc, source?.Sut));
  }

  public static bool IsSyntaxTargetForGeneration(SyntaxNode node)
  {
    return node is ClassDeclarationSyntax;
  }

  public static SutIncrementalValues? GetSut(SyntaxNode node, SemanticModel semanticModel)
  {
    if (node is not ClassDeclarationSyntax classDeclarationSyntax)
      return null;

    var testClassSymbol = semanticModel.GetDeclaredSymbol(node);
    var testClassNamespace = testClassSymbol?.ContainingNamespace.ToString();

    if (testClassSymbol is null || testClassNamespace is null)
      return null;

    foreach (var attributeListSyntax in classDeclarationSyntax.AttributeLists)
    {
      foreach (var attributeSyntax in attributeListSyntax.Attributes)
      {
        GenericNameSyntax? genericName = GetGenericName(attributeSyntax);
        if (genericName is null) continue;
        if (genericName.TypeArgumentList.Arguments.Count == 0) continue;

        if (genericName.Identifier.ToString() is not "Sut" && genericName.Identifier.ToString() is not "SutAttribute")
          continue;

        var sutTypeArgument = genericName.TypeArgumentList.Arguments[0];

        var @type = semanticModel.GetTypeInfo(sutTypeArgument).Type;
        var @namespace = @type?.ContainingNamespace.ToString();
        var @assembly = @type?.ContainingAssembly;

        if (@type is null || @namespace is null || @assembly is null)
          continue;

        if (@type.TypeKind != TypeKind.Class &&
           @type.TypeKind != TypeKind.Struct &&
           @type.TypeKind != TypeKind.Interface &&
           !@type.IsRecord)
          continue;

        var typeArguments = new EquatableList<SutTypeArgument>();
        if (@type is INamedTypeSymbol namedType && namedType.IsGenericType && namedType.TypeArguments.Length > 0)
        {
          for (var i = 0; i < namedType.TypeArguments.Length; i++)
          {
            var typeArgument = namedType.TypeArguments[i];
            var typeParameter = namedType.TypeParameters[i];
            if (typeArgument.Kind == SymbolKind.TypeParameter)
              return null; // this would be an open generic, which is not supported here
            else if (typeArgument.Kind == SymbolKind.NamedType)
              typeArguments.Add(new(typeParameter.Name, typeArgument.Name, typeArgument.ContainingNamespace.ToString()));
          }
        }

        var sut = new SutIncrementalValues(
          node,
          new Sut(
            @type.Name,
            @namespace,
            @assembly.Identity.Name,
            @assembly.Identity.Version.ToString(),
            testClassSymbol.Name,
            testClassNamespace,
            typeArguments,
            []
          )
        );

        return sut;
      }
    }

    return null;
  }

  private static SutIncrementalValues? GetDependencies(SutIncrementalValues? sut, SemanticModel semanticModel)
  {
    if (sut is null || sut?.Sut is null || semanticModel is null)
      return null;

    var assemblySymbol = GetAssembly(sut?.Sut?.Source!, sut?.Sut?.SourceVersion!, semanticModel);
    if (assemblySymbol is null)
      return null;

    var namespaceSymbol = GetNamespace(assemblySymbol.GlobalNamespace, sut?.Sut?.Namespace!);
    if (namespaceSymbol is null)
      return null;

    var typeSymbol = namespaceSymbol.GetTypeMembers(sut?.Sut?.Name!).FirstOrDefault();
    if (typeSymbol is null)
      return null;

    // select the first public constructor
    foreach (var constructor in typeSymbol.InstanceConstructors)
    {
      if (!constructor.DeclaredAccessibility.HasFlag(Accessibility.Public))
        continue;

      var dependencies = new EquatableList<SutDependency>();

      foreach (var parameter in constructor.Parameters)
      {
        var parameterName = parameter.Name;
        var parameterType = parameter.Type as INamedTypeSymbol;
        var parameterNamespace = parameterType?.ContainingNamespace.ToString();
        var parameterAssembly = parameterType?.ContainingAssembly;

        if (parameterType is null || parameterNamespace is null || parameterAssembly is null)
          continue;

        var typeArguments = new EquatableList<SutTypeArgument>();
        if (parameterType.IsGenericType && parameterType.TypeArguments.Length > 0)
        {
          for (var i = 0; i < parameterType.TypeArguments.Length; i++)
          {
            var typeArgument = parameterType.TypeArguments[i];
            var typeParameter = parameterType.TypeParameters[i];
            if (typeArgument.Kind == SymbolKind.TypeParameter)
              typeArguments.Add(new(typeParameter.Name, typeArgument.Name));
            else if (typeArgument.Kind == SymbolKind.NamedType)
              typeArguments.Add(new(typeParameter.Name, typeArgument.Name, typeArgument.ContainingNamespace.ToString()));
          }
        }

        dependencies.Add(new SutDependency
        {
          Name = parameterName,
          TypeName = parameterType.Name,
          TypeArguments = typeArguments,
          TypeNamespace = parameterNamespace,
          TypeKind = parameterType.TypeKind,
          Source = parameterAssembly.Identity.Name,
          SourceVersion = parameterAssembly.Identity.Version.ToString(),
          IsSealed = parameterType.IsSealed
        });
      }

      return new SutIncrementalValues(
        sut?.Node!,
        new Sut(
          sut?.Sut?.Name!,
          sut?.Sut?.Namespace!,
          sut?.Sut?.Source!,
          sut?.Sut?.SourceVersion!,
          sut?.Sut?.TestClassName!,
          sut?.Sut?.TestClassNamespace!,
          sut?.Sut?.TypeArguments!,
          dependencies
        )
      );
    }

    return null;
  }

  public static SutIncrementalValues? GetDependencyMembers(SutIncrementalValues? sut, SemanticModel semanticModel)
  {
    if (sut is null || sut?.Sut is null || sut?.Sut?.Dependencies is null || semanticModel is null)
      return null;

    var dependencies = new EquatableList<SutDependency>();

    foreach (var dependency in sut?.Sut?.Dependencies!)
    {
      var assemblySymbol = GetAssembly(dependency.Source!, dependency.SourceVersion!, semanticModel);
      if (assemblySymbol is null)
        return null;

      var namespaceSymbol = GetNamespace(assemblySymbol.GlobalNamespace, dependency.TypeNamespace);
      if (namespaceSymbol is null)
        return null;

      var typeSymbol = namespaceSymbol.GetTypeMembers(dependency.TypeName).FirstOrDefault();
      if (typeSymbol is null)
        return null;

      if (dependency.TypeKind != TypeKind.Class &&
          dependency.TypeKind != TypeKind.Interface)
        continue;

      var members = typeSymbol.GetMembers();
      var dependencyMembers = new EquatableList<SutDependencyMember>();

      foreach (var member in members)
      {
        if (member is IMethodSymbol methodSymbol
          && (methodSymbol.DeclaredAccessibility.HasFlag(Accessibility.Public)
            || methodSymbol.DeclaredAccessibility.HasFlag(Accessibility.Internal))
          && (methodSymbol.MethodKind == MethodKind.Ordinary
            || methodSymbol.MethodKind == MethodKind.PropertyGet
            || methodSymbol.MethodKind == MethodKind.PropertySet))
        {
          var parameters = new EquatableList<SutDependencyMemberParameter>();

          var methodTypeArguments = new EquatableList<SutTypeArgument>();
          if (methodSymbol.IsGenericMethod && methodSymbol.Arity > 0)
          {
            for (var i = 0; i < methodSymbol.TypeArguments.Length; i++)
            {
              var typeArgument = methodSymbol.TypeArguments[i];
              var typeParameter = methodSymbol.TypeParameters[i];
              if (typeArgument.Kind == SymbolKind.TypeParameter)
                methodTypeArguments.Add(new(typeParameter.Name, typeArgument.Name));
              else if (typeArgument.Kind == SymbolKind.NamedType)
                methodTypeArguments.Add(new(typeParameter.Name, typeArgument.Name, typeArgument.ContainingNamespace.ToString()));
            }
          }

          foreach (var parameter in methodSymbol.Parameters)
            {
              var parameterTypeArguments = new EquatableList<SutTypeArgument>();
              if (parameter.Type is IArrayTypeSymbol arrayTypeSymbol)
              {
                parameters.Add(new SutDependencyMemberParameter(parameter.Name, parameter.Type?.TypeKind, arrayTypeSymbol.ElementType?.Name, arrayTypeSymbol.ElementType?.ContainingNamespace.ToString(), parameterTypeArguments));
              }
              else if (parameter.Type is ITypeParameterSymbol typeParameterSymbol)
              {
                var dependencyTypeArgument = dependency.TypeArguments.FirstOrDefault(t => t.TypeParameter == parameter.Type?.Name);
                if (dependencyTypeArgument != default)
                  parameters.Add(new SutDependencyMemberParameter(parameter.Name, parameter.Type?.TypeKind, dependencyTypeArgument.TypeArgument, dependencyTypeArgument.TypeArgumentNamespace, parameterTypeArguments));
                else
                  parameters.Add(new SutDependencyMemberParameter(parameter.Name, parameter.Type?.TypeKind, parameter.Type?.Name, parameter.Type?.ContainingNamespace.ToString(), parameterTypeArguments));
              }
              else
              {
                if (parameter.Type is INamedTypeSymbol namedType && namedType.IsGenericType && namedType.Arity > 0)
                {
                  for (var i = 0; i < namedType.TypeArguments.Length; i++)
                  {
                    var typeArgument = namedType.TypeArguments[i];
                    var typeParameter = namedType.TypeParameters[i];
                    if (typeArgument.Kind == SymbolKind.TypeParameter)
                      parameterTypeArguments.Add(new(typeParameter.Name, typeArgument.Name));
                    else if (typeArgument.Kind == SymbolKind.NamedType)
                      parameterTypeArguments.Add(new(typeParameter.Name, typeArgument.Name, typeArgument.ContainingNamespace.ToString()));
                  }
                }
                parameters.Add(new SutDependencyMemberParameter(parameter.Name, parameter.Type?.TypeKind, parameter.Type?.Name, parameter.Type?.ContainingNamespace.ToString(), parameterTypeArguments));
              }
            }

          string? returnType = null;
          string? returnTypeNamespace = null;
          var returnTypeArguments = new EquatableList<SutTypeArgument>();

          var isAsync = !methodSymbol.ReturnsVoid && methodSymbol.ReturnType.Name == "Task";
          if (isAsync)
          {
            if (methodSymbol.ReturnType is INamedTypeSymbol namedType && namedType.IsGenericType && namedType.TypeArguments.Length > 0)
            {
              var taskTypeArgument = namedType.TypeArguments.First();
              returnType = taskTypeArgument.Name;
              returnTypeNamespace = taskTypeArgument.ContainingNamespace.ToString();

              if (taskTypeArgument is INamedTypeSymbol taskNamedType && taskNamedType.IsGenericType && taskNamedType.TypeArguments.Length > 0)
              {
                for (var i = 0; i < taskNamedType.TypeArguments.Length; i++)
                {
                  var typeArgument = taskNamedType.TypeArguments[i];
                  var typeParameter = taskNamedType.TypeParameters[i];
                  if (typeArgument.Kind == SymbolKind.TypeParameter)
                  {
                    var dependencyTypeArgument = dependency.TypeArguments?.FirstOrDefault(t => t.TypeParameter == typeArgument.Name);
                    if (dependencyTypeArgument is not null)
                      returnTypeArguments.Add(new("", dependencyTypeArgument!.Value.TypeArgument, dependencyTypeArgument!.Value.TypeArgumentNamespace));
                    else
                      returnTypeArguments.Add(new(typeParameter.Name, typeArgument.Name));
                  }
                  else if (typeArgument.Kind == SymbolKind.NamedType)
                    returnTypeArguments.Add(new(typeParameter.Name, typeArgument.Name, typeArgument.ContainingNamespace.ToString()));
                }
              }
            }
          }
          else if (!methodSymbol.ReturnsVoid)
          {
            returnType = methodSymbol.ReturnType?.Name;
            returnTypeNamespace = methodSymbol.ReturnType?.ContainingNamespace.ToString();

            if (methodSymbol.ReturnType is INamedTypeSymbol namedType && namedType.IsGenericType && namedType.TypeArguments.Length > 0)
            {
              for (var i = 0; i < namedType.TypeArguments.Length; i++)
              {
                var typeArgument = namedType.TypeArguments[i];
                var dependencyTypeArgument = dependency.TypeArguments.First(t => t.TypeParameter == typeArgument.Name);
                if (typeArgument.Kind == SymbolKind.TypeParameter)
                  returnTypeArguments.Add(new("", dependencyTypeArgument.TypeArgument));
                else if (typeArgument.Kind == SymbolKind.NamedType)
                  returnTypeArguments.Add(new("", dependencyTypeArgument.TypeArgument, dependencyTypeArgument.TypeArgumentNamespace));
              }
            }
          }

          var methodName = (methodSymbol.MethodKind == MethodKind.PropertyGet || methodSymbol.MethodKind == MethodKind.PropertySet)
            ? (methodSymbol.AssociatedSymbol as IPropertySymbol)?.Name
            : methodSymbol.Name;

          dependencyMembers.Add(new SutDependencyMember
          {
            Name = methodName!,
            Kind = methodSymbol.MethodKind,
            IsAsync = isAsync,
            TypeArguments = methodTypeArguments,
            ReturnType = returnType,
            ReturnTypeNamespace = returnTypeNamespace,
            ReturnTypeArguments = returnTypeArguments,
            Parameters = parameters
          });
        }
      }

      dependencies.Add(new SutDependency
      {
        Name = dependency.Name,
        TypeName = dependency.TypeName,
        TypeNamespace = dependency.TypeNamespace,
        TypeKind = dependency.TypeKind,
        TypeArguments = dependency.TypeArguments,
        Source = dependency.Source,
        SourceVersion = dependency.SourceVersion,
        IsSealed = dependency.IsSealed,
        Members = dependencyMembers
      });
    }

    return new SutIncrementalValues(
      sut?.Node!,
      new Sut(
        sut?.Sut?.Name!,
        sut?.Sut?.Namespace!,
        sut?.Sut?.Source!,
        sut?.Sut?.SourceVersion!,
        sut?.Sut?.TestClassName!,
        sut?.Sut?.TestClassNamespace!,
        sut?.Sut?.TypeArguments!,
        dependencies
      )
    );
  }

  public static void Execute(SourceProductionContext context, Sut? sut)
  {
    if (sut is null || sut?.Dependencies is null)
      return;

    var sb = new StringBuilder()
      .AppendLine("#nullable enable")
      .AppendLine("")
      .AppendLine("using Moq;")
      .AppendLine("using Sut;")
      .AppendLine($"using {sut?.Namespace};")
      .AppendLine("")
      .AppendLine($"namespace {sut?.TestClassNamespace};")
      .AppendLine("")
      .AppendLine($"public partial class {sut?.TestClassName} {{");

    var builderName = $"Builder{sut?.TypeArguments.Format(sut.Value)}";

    sb.AppendLine($"  private {builderName} Sut {{ get; }} = new();")
      .AppendLine("")
      .AppendLine($"  private class {builderName} {{");

    if (sut?.Dependencies.Count > 0)
    {
      foreach (var dependency in sut?.Dependencies!)
      {
        var dependencyName = CodeIdentifier.MakePascal(dependency.Name);

        string dependencyType;
        if (dependency.TypeNamespace != sut?.Namespace)
          dependencyType = $"{dependency.TypeNamespace}.{dependency.TypeName}{dependency.TypeArguments.Format(sut!.Value)}";
        else
          dependencyType = $"{dependency.TypeName}{dependency.TypeArguments.Format(sut!.Value)}";

        if (dependency.IsOptions())
        {
          sb.AppendLine($"    public Microsoft.Extensions.Options.IOptions<{dependency.TypeArguments[0].TypeArgument}>? {dependencyName} {{ get; private set; }}");
        }
        else if (dependency.IsLogger())
        {
          sb.AppendLine($"    public Mock<Microsoft.Extensions.Logging.ILogger<{dependency.TypeArguments[0].TypeArgument}>> {dependencyName} {{ get; }} = new();");
        }
        else if (!dependency.IsSealed &&
          (dependency.TypeKind == TypeKind.Interface || dependency.TypeKind == TypeKind.Class))
        {
          sb.AppendLine($"    public Mock<{dependencyType}> {dependencyName} {{ get; }} = new();");
        }
        else
        {
          sb.AppendLine($"    public {dependencyType} {dependencyName} {{ get; private set; }}");
        }
      }

      sb.AppendLine();

      foreach (var dependency in sut?.Dependencies!)
      {
        if (!dependency.IsSealed &&
          (dependency.TypeKind == TypeKind.Interface || dependency.TypeKind == TypeKind.Class))
        {
          if (dependency.IsOptions())
          {
            sb.AppendLine(GenerateOptionsSetupMethod(dependency, builderName));
          }
          else if (dependency.IsLogger())
          {
            sb.AppendLine(GenerateLoggerSetupMethod(dependency, builderName));
          }
          else
          {
            foreach (var dependencyMember in dependency.Members)
            {
              switch (dependencyMember.Kind)
              {
                case MethodKind.PropertyGet:
                case MethodKind.PropertySet:
                  sb.AppendLine(GeneratePropertySetupMethod(dependency, dependencyMember, builderName));
                  break;
                default:
                  sb.AppendLine(GenerateSetupMethod((Sut)sut, dependency, dependencyMember, builderName, false));
                  sb.AppendLine(GenerateSetupMethod((Sut)sut, dependency, dependencyMember, builderName, true));
                  break;
              }
            }
          }
        }
        else
        {
          sb.AppendLine(GenerateSetupMethod(dependency, builderName));
        }
      }
    }

    sb.AppendLine($"    public {sut?.Name}{sut?.TypeArguments.Format(sut!.Value)} Build() {{")
      .AppendLine($"      return new {sut?.Name}{sut?.TypeArguments.Format(sut!.Value)}(");

    for (var i = 0; i < sut?.Dependencies.Count; i++)
    {
      var dependency = sut?.Dependencies[i];
      if (dependency is null) continue;

      var dependencyName = CodeIdentifier.MakePascal(dependency?.Name);

      if (dependency.IsOptions())
      {
        sb.Append($"        {dependencyName}!");
      }
      else if (!dependency!.Value.IsSealed &&
          (dependency?.TypeKind == TypeKind.Interface || dependency?.TypeKind == TypeKind.Class))
      {
        sb.Append($"        {dependencyName}.Object");
      }
      else
      {
        sb.Append($"        {dependencyName}");
      }

      if (i < sut?.Dependencies.Count - 1) sb.Append(",");
      sb.AppendLine();
    }

    sb.AppendLine("      );")
      .AppendLine("    }")
      .AppendLine("  }")
      .AppendLine("}");

    context.AddSource($"{sut?.TestClassName}.g.cs",
      SourceText.From(sb.ToString(), Encoding.UTF8)
    );
  }

  private static string GenerateSetupMethod(Sut sut, SutDependency dependency, SutDependencyMember dependencyMember, string builderName, bool setupException = false)
  {
    var sb = new StringBuilder();

    var dependencyName = CodeIdentifier.MakePascal(dependency.Name);

    sb.Append($"    public {builderName} With_{dependencyName}_{dependencyMember.Name}");
    if (setupException) sb.Append("_Exception");
    sb.AppendLine($"{dependencyMember.TypeArguments.Format(sut)}(");

    var hasReturnValue = !setupException && dependencyMember.ReturnType is not null;

    for (var i = 0; i < dependencyMember.Parameters.Count; i++)
    {
      var parameter = dependencyMember.Parameters[i];
      var parameterName = CodeIdentifier.MakeCamel(parameter.Name);
      string parameterType;
      if (parameter.TypeNamespace != sut.Namespace)
        parameterType = $"{parameter.TypeNamespace}.{parameter.TypeName}{parameter.TypeArguments.Format(sut)}";
      else
        parameterType = $"{parameter.TypeName}{parameter.TypeArguments.Format(sut)}";
      if (parameter.TypeKind == TypeKind.Array)
        parameterType += "[]";
      sb.Append($"      System.Linq.Expressions.Expression<System.Func<{parameterType}, System.Boolean>> {parameterName}");
      if (i < dependencyMember.Parameters.Count - 1 || hasReturnValue || setupException) sb.Append(",");
      sb.AppendLine();
    }

    if (hasReturnValue)
    {
      string dependencyType;
      if (dependencyMember.ReturnTypeNamespace != sut.Namespace)
        dependencyType = $"{dependencyMember.ReturnTypeNamespace}.{dependencyMember.ReturnType}{dependencyMember.ReturnTypeArguments.Format(sut)}";
      else
        dependencyType = $"{dependencyMember.ReturnType}{dependencyMember.ReturnTypeArguments.Format(sut)}";
      sb.AppendLine($"      {dependencyType} returns");
    }
    else if (setupException)
    {
      sb.AppendLine("      System.Exception exception");
    }

    sb.AppendLine("    ) {");

    sb.AppendLine($"      {dependencyName}");
    sb.AppendLine($"        .Setup(x =>");
    sb.Append($"          x.{dependencyMember.Name}(");

    if (dependencyMember.Parameters.Count > 0)
    {
      sb.AppendLine();

      foreach (var parameter in dependencyMember.Parameters)
      {
        var parameterName = CodeIdentifier.MakeCamel(parameter.Name);
        string parameterType;
        if (parameter.TypeNamespace != sut.Namespace)
          parameterType = $"{parameter.TypeNamespace}.{parameter.TypeName}{parameter.TypeArguments.Format(sut)}";
        else
          parameterType = $"{parameter.TypeName}{parameter.TypeArguments.Format(sut)}";
        if (parameter.TypeKind == TypeKind.Array)
          parameterType += "[]";
        sb.Append($"            It.Is<{parameterType}>({parameterName})");
        if (!parameter.Equals(dependencyMember.Parameters.Last())) sb.Append(",");
        sb.AppendLine();
      }

      sb.Append("          ");
    }

    sb.AppendLine(")");

    if (hasReturnValue)
    {
      sb.AppendLine("        )");
      sb.Append("        ");
      if (dependencyMember.IsAsync)
        sb.Append(".ReturnsAsync(");
      else
        sb.Append(".Returns(");

      sb.AppendLine("returns)");
    }

    if (setupException)
    {
      sb.AppendLine("        )");
      sb.Append("        ");
      if (dependencyMember.IsAsync)
        sb.Append(".ThrowsAsync(");
      else
        sb.Append(".Throws(");
      sb.AppendLine("exception);");
    }
    else
    {
      if (!hasReturnValue)
        sb.AppendLine("        )");
      sb.AppendLine("        .Verifiable();");
    }

    sb.AppendLine("      return this;");
    sb.AppendLine("    }");

    return sb.ToString();
  }

  private static string GenerateOptionsSetupMethod(SutDependency dependency, string builderName)
  {
    var sb = new StringBuilder();

    var dependencyName = CodeIdentifier.MakePascal(dependency.Name);

    sb.AppendLine($"    public {builderName} With_{dependencyName}(");

    var @type = dependency.TypeArguments[0].TypeArgument;

    sb.AppendLine($"      {@type} {dependency.Name}");
    sb.AppendLine("    ) {");
    sb.AppendLine($"      {dependencyName} = Microsoft.Extensions.Options.Options.Create({dependency.Name});");
    sb.AppendLine("      return this;");
    sb.AppendLine("    }");

    return sb.ToString();
  }

  private static string GenerateLoggerSetupMethod(SutDependency dependency, string builderName)
  {
    var sb = new StringBuilder();

    var dependencyName = CodeIdentifier.MakePascal(dependency.Name);

    sb.AppendLine($"    public {builderName} With_Logger(");
    sb.AppendLine("      Microsoft.Extensions.Logging.LogLevel logLevel,");
    sb.AppendLine("      string message,");
    sb.AppendLine("      System.Exception? exception = null");
    sb.AppendLine("    ) {");
    sb.AppendLine($"      {dependencyName}");
    sb.AppendLine("        .Setup(x =>");
    sb.AppendLine("          x.Log(");
    sb.AppendLine("            It.Is<Microsoft.Extensions.Logging.LogLevel>(l => l == logLevel),");
    sb.AppendLine("            It.IsAny<Microsoft.Extensions.Logging.EventId>(),");
    sb.AppendLine("            It.Is<It.IsAnyType>((v, t) => v.ToString() == message),");
    sb.AppendLine("            It.Is<System.Exception?>(e => e == exception),");
    sb.AppendLine("            It.IsAny<System.Func<It.IsAnyType, System.Exception?, string>>()");
    sb.AppendLine("          )");
    sb.AppendLine("        )");
    sb.AppendLine("        .Verifiable();");
    sb.AppendLine("      return this;");
    sb.AppendLine("    }");

    return sb.ToString();
  }
  private static string GeneratePropertySetupMethod(SutDependency dependency, SutDependencyMember dependencyMember, string builderName)
  {
    var sb = new StringBuilder();

    var dependencyName = CodeIdentifier.MakePascal(dependency.Name);
    var dependencyMemberName = CodeIdentifier.MakeCamel(dependencyMember.Name);

    sb.Append($"    public {builderName} With_{dependencyName}_{dependencyMember.Name}");
    sb.AppendLine("(");

    if (dependencyMember.Kind == MethodKind.PropertyGet)
      sb.AppendLine($"      {dependencyMember.ReturnType} {dependencyMemberName}");
    else if (dependencyMember.Kind == MethodKind.PropertySet)
      sb.AppendLine($"      System.Action<{dependencyName}> {dependencyMemberName}");

    sb.AppendLine("    ) {");
    sb.AppendLine($"      {dependencyName}");

    if (dependencyMember.Kind == MethodKind.PropertyGet)
    {
      sb.AppendLine($"        .SetupGet(x => x.{dependencyMember.Name})");
      sb.AppendLine($"        .Returns({dependencyMemberName})");
    }
    else if (dependencyMember.Kind == MethodKind.PropertySet)
      sb.AppendLine($"        .SetupSet({dependencyMemberName})");

    sb.AppendLine("        .Verifiable();");

    sb.AppendLine("      return this;");
    sb.AppendLine("    }");

    return sb.ToString();
  }

  private static string GenerateSetupMethod(SutDependency dependency, string builderName)
  {
    var sb = new StringBuilder();
    var dependencyName = CodeIdentifier.MakePascal(dependency.Name);

    sb.AppendLine($"    public {builderName} With_{dependencyName}(");
    sb.AppendLine($"      {dependency.TypeName} value");
    sb.AppendLine($"    ) {{");
    sb.AppendLine($"      {dependency.TypeName} = value;");
    sb.AppendLine("      return this;");
    sb.AppendLine("    }");

    return sb.ToString();
  }

  private static GenericNameSyntax? GetGenericName(AttributeSyntax attributeSyntax)
  {
    if (attributeSyntax.Name is QualifiedNameSyntax qualifiedNameSyntax)
    {
      if (qualifiedNameSyntax.Right is GenericNameSyntax genericNameSyntax)
      {
        return genericNameSyntax;
      }
    }
    else if (attributeSyntax.Name is GenericNameSyntax genericNameSyntax)
    {
      return genericNameSyntax;
    }

    return null;
  }

  private static IAssemblySymbol? GetAssembly(string assemblyName, string assemblyVersion, SemanticModel semanticModel)
  {
    IAssemblySymbol? assemblySymbol = null;
    if (semanticModel.Compilation.Assembly.Name == assemblyName &&
        semanticModel.Compilation.Assembly.Identity.Version.ToString() == assemblyVersion)
    {
      assemblySymbol = semanticModel.Compilation.Assembly;
    }
    else
    {
      assemblySymbol = semanticModel.Compilation.SourceModule.ReferencedAssemblySymbols
        .FirstOrDefault(a => a.Name == assemblyName && a.Identity.Version.ToString() == assemblyVersion);
    }

    return assemblySymbol;
  }

  private static INamespaceSymbol? GetNamespace(INamespaceSymbol parent, string @namespace)
  {
    var namespaceMembers = parent.GetNamespaceMembers();

    foreach (var namespaceSymbol in namespaceMembers)
    {
      if (namespaceSymbol.ToString() == @namespace)
      {
        return namespaceSymbol;
      }

      var nestedNamespace = GetNamespace(namespaceSymbol, @namespace);
      if (nestedNamespace is not null)
      {
        return nestedNamespace;
      }
    }

    return null;
  }
}