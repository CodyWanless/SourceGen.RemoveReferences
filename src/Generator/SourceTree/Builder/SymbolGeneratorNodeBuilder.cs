using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Generator.SourceTree.Builder
{
    internal abstract class SymbolGeneratorNodeBuilder<T, TBuilder>
        where T : ISourceGeneratorNode
        where TBuilder : SymbolGeneratorNodeBuilder<T, TBuilder>
    {
        private const string SystemPrefix = "System";

        private readonly IReadOnlyCollection<string> allowedNamespacePrefixes;
        protected readonly string sourceAssemblyName;
        protected readonly string destinationAssemblyName;

        protected IReadOnlyCollection<AttributeData>? attributes;
        protected string? name;

        protected SymbolGeneratorNodeBuilder(
            string sourceAssemblyName,
            string destinationAssemblyName)
        {
            this.sourceAssemblyName = sourceAssemblyName ?? throw new ArgumentNullException(nameof(sourceAssemblyName));
            this.destinationAssemblyName = destinationAssemblyName ?? throw new ArgumentNullException(nameof(sourceAssemblyName));
            this.allowedNamespacePrefixes = new[] { SystemPrefix, this.sourceAssemblyName };
        }

        public TBuilder AddName(ISymbol symbol)
        {
            this.name = symbol.Name;
            return (TBuilder)this;
        }

        public TBuilder AddAttributes(ISymbol symbol)
        {
            this.attributes = symbol.GetAttributes()
                .Where(a => this.CanIncludeSymbol(a.AttributeClass!.ContainingSymbol))
                .ToArray();

            return (TBuilder)this;
        }

        public T Build()
        {
            return this.Build(this.name!);
        }

        protected bool CanIncludeSymbol(ISymbol typeSymbol)
        {
            return this.allowedNamespacePrefixes.Any(typeSymbol.GetFullNamespace().StartsWith);
        }

        protected abstract T Build(
            string name);
    }
}
