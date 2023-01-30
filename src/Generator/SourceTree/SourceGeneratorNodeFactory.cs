using System.Collections.Generic;
using Generator.SourceTree.Abstract;
using Microsoft.CodeAnalysis;

namespace Generator.SourceTree
{
    /// <summary>
    /// Factory used to generate source nodes to be used to create new types
    /// in new assembly from compiled assembly.
    /// </summary>
    internal sealed class SourceGeneratorNodeFactory
    {
        private readonly SourceGeneratorNodeSymbolVisitor sourceGeneratorNodeSymbolVisitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceGeneratorNodeFactory"/> class.
        /// Probably need to include: Origin root namespace, Destination root namespace, Source namespaces to ignore members from.
        /// </summary>
        /// <param name="sourceAssemblyRootNamespace">Root namespace of source assembly.</param>
        /// <param name="destinationAssemblyRootNamespace">Root namespace of destination assembly.</param>
        public SourceGeneratorNodeFactory(
            string sourceAssemblyRootNamespace,
            string destinationAssemblyRootNamespace)
        {
            this.sourceGeneratorNodeSymbolVisitor = new SourceGeneratorNodeSymbolVisitor(
                sourceAssemblyRootNamespace,
                destinationAssemblyRootNamespace);
        }

        /// <summary>
        /// Returns constructed tree of type nodes with children properties that can
        /// be used to write new source from.
        /// </summary>
        /// <param name="typeSymbols">Symbols from compiled assembly.</param>
        /// <returns>Collection of source trees used to generate new trees from.</returns>
        public IReadOnlyCollection<ISourceGeneratorNode> CreateGeneratorsFromTypes(
            IReadOnlyCollection<ITypeSymbol> typeSymbols)
        {
            var topLevelNodes = new List<ISourceGeneratorNode>(typeSymbols.Count);

            foreach (var typeSymbol in typeSymbols)
            {
                var node = typeSymbol.Accept(this.sourceGeneratorNodeSymbolVisitor);
                if (node is not null)
                {
                    topLevelNodes.Add(node);
                }
            }

            return topLevelNodes;
        }
    }
}
