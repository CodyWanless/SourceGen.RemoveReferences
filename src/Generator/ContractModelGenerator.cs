using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Generator
{
    [Generator]
    public class ContractModelGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            Console.WriteLine($"Executing {nameof(ContractModelGenerator)}");

            var importedReferenceTypes = this.GetAllTypeSymbols(context, context.Compilation).ToArray();
            if (!importedReferenceTypes.Any())
            {
                // No types found for specified reference assembly. Nothing to do.
                return;
            }

            var properties = importedReferenceTypes
                .Where(t => t.TypeKind == TypeKind.Enum || t.TypeKind == TypeKind.Class)
                .Where(t => t.DeclaredAccessibility == Accessibility.Public)
                .Select(t => new
                {
                    TypeSymbol = t,
                    TypeName = t.Name,
                    Properties = t.GetMembers()
                })
                .ToArray();

            Console.WriteLine("Operating type dump");
            foreach (var x in properties)
            {
                Console.WriteLine($"{x.TypeName} - members {string.Join(", ", x.Properties.Select(p => p.ToDisplayString()))}");
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }

        private IEnumerable<ITypeSymbol> GetAllTypeSymbols(
            GeneratorExecutionContext context,
            Compilation compilation)
        {
            Console.WriteLine($"All options {string.Join(", ", context.AnalyzerConfigOptions.GlobalOptions.Keys)}");
            if (!context.AnalyzerConfigOptions.GlobalOptions
                .TryGetValue("build_property.GenerateDtoFromAssembly", out var usedAssemblyName))
            {
                Console.WriteLine("Build property not found. Bailing.");
                return Enumerable.Empty<ITypeSymbol>();
            }
            Console.WriteLine($"Loading referenced types for assembly {usedAssemblyName}");

            return compilation
                .SourceModule
                .ReferencedAssemblySymbols
                .Where(a => a.Name == usedAssemblyName)
                .SelectMany(a =>
                {
                    try
                    {
                        var main = a.Identity.Name.Split('.').Aggregate(a.GlobalNamespace, (s, c) => s.GetNamespaceMembers().Single(m => m.Name.Equals(c)));

                        return GetAllTypes(main);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed gettings type symbols for {a.Name}: {ex.Message}");
                        return Enumerable.Empty<ITypeSymbol>();
                    }
                });
        }

        private static IEnumerable<ITypeSymbol> GetAllTypes(INamespaceSymbol root)
        {
            foreach (var namespaceOrTypeSymbol in root.GetMembers())
            {
                if (namespaceOrTypeSymbol is INamespaceSymbol @namespace)
                {
                    foreach (var nested in GetAllTypes(@namespace))
                    {
                        yield return nested;
                    }
                }
                else if (namespaceOrTypeSymbol is ITypeSymbol type)
                {
                    yield return type;
                }
            }
        }
    }
}
