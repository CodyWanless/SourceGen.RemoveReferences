using System;
using System.Collections.Generic;
using System.Linq;
using Generator.SourceTree;
using Microsoft.CodeAnalysis;

namespace Generator
{
    [Generator]
    public class ContractModelGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            Console.WriteLine($"Executing {nameof(ContractModelGenerator)}");

            if (!context.AnalyzerConfigOptions.GlobalOptions
                .TryGetValue(Constants.GenerateFromDtoBuildPropertyName, out var generateFromAssemblyName))
            {
                Console.WriteLine($"{Constants.GenerateFromDtoBuildPropertyName} build property not found. No sources to generate.");
                return;
            }

            var importedReferenceTypes = this.GetAllTypeSymbols(context.Compilation, generateFromAssemblyName).ToArray();
            if (!importedReferenceTypes.Any())
            {
                // No types found for specified reference assembly. Nothing to do.
                return;
            }

            var sourceNodes = new SourceGeneratorNodeFactory(
                generateFromAssemblyName,
                context.Compilation.Assembly.Name)
                .CreateGeneratorsFromTypes(importedReferenceTypes);
            foreach (var parentNode in sourceNodes)
            {
                var sourceNodeWriterVisitor = new SourceWriterNodeVisitor(context);
                parentNode.Accept(sourceNodeWriterVisitor);

                sourceNodeWriterVisitor.WriteSource();
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }

        private IEnumerable<ITypeSymbol> GetAllTypeSymbols(
            Compilation compilation,
            string generateFromAssemblyName)
        {
            Console.WriteLine($"Loading referenced types for assembly {generateFromAssemblyName}");

            return compilation
                .SourceModule
                .ReferencedAssemblySymbols
                .Where(a => a.Name == generateFromAssemblyName)
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
