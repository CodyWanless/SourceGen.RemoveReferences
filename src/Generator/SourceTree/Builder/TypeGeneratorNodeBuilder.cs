using Generator.SourceTree.Model;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Generator.SourceTree.Builder
{
    internal abstract class TypeGeneratorNodeBuilder<T, TBuilder> : SymbolGeneratorNodeBuilder<T, TBuilder>
        where T : TypeGeneratorNode
        where TBuilder : TypeGeneratorNodeBuilder<T, TBuilder>
    {
        protected IReadOnlyCollection<INamedTypeSymbol>? classInterfaces;
        protected (string sourceNamespace, string updatedNamespace)? namespaces;

        protected TypeGeneratorNodeBuilder(
            string sourceAssemblyName,
            string destinationAssemblyName)
            : base(sourceAssemblyName, destinationAssemblyName)
        {
        }

        public TBuilder AddNamespace(ITypeSymbol typeSymbol)
        {
            var sourceNamespace = typeSymbol.GetFullNamespace();
            this.namespaces = (sourceNamespace, sourceNamespace.Replace(this.sourceAssemblyName, this.destinationAssemblyName));

            return (TBuilder)this;
        }

        public TBuilder AddInterfaces(ITypeSymbol typeSymbol)
        {
            this.classInterfaces = typeSymbol.Interfaces
                .Where(this.CanIncludeSymbol)
                .ToArray();

            return (TBuilder)this;
        }

        public abstract TBuilder AddTypeData(ITypeSymbol typeSymbol);

        protected override T Build(string name)
        {
            var usingDeclarations = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var symbolNamespace in this.classInterfaces.Concat(this.attributes.Select(a => a.AttributeClass))
                .Select(s => s!.GetFullNamespace()))
            {
                usingDeclarations.Add(symbolNamespace);
            }
            foreach (var childNode in this.GetChildrenNodes())
            {
                usingDeclarations.UnionWith(childNode.RequiredNamespaces);
            }
            usingDeclarations.Remove(this.namespaces!.Value.updatedNamespace);

            return this.Build(
                name,
                this.namespaces!.Value.updatedNamespace,
                this.classInterfaces!,
                this.attributes!,
                usingDeclarations);
        }

        protected abstract IEnumerable<ISourceGeneratorNode> GetChildrenNodes();

        protected abstract T Build(
            string name,
            string updatedNamespace,
            IReadOnlyCollection<INamedTypeSymbol> interfaces,
            IReadOnlyCollection<AttributeData> attributes,
            SortedSet<string> usingDeclartions);
    }
}
