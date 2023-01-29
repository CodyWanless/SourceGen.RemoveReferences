using System;
using System.Collections.Generic;
using Generator.SourceTree.Abstract;
using Microsoft.CodeAnalysis;

namespace Generator.SourceTree.Model
{
    internal record ClassGeneratorNode : ISourceGeneratorNode
    {
        private readonly INamedTypeSymbol namedTypeSymbol;

        public ClassGeneratorNode(
            INamedTypeSymbol symbol,
            ISourceGeneratorNode namespaceGeneratorNode,
            IReadOnlyCollection<ISourceGeneratorNode> children)
        {
            this.namedTypeSymbol = symbol;
            this.NamespaceGeneratorNode = namespaceGeneratorNode;
            this.Children = children;
        }

        public IReadOnlyCollection<INamedTypeSymbol> Interfaces { get; }

        public string Name => this.namedTypeSymbol.Name;

        public IReadOnlyCollection<string> RequiredNamespaces => throw new NotImplementedException();

        public IReadOnlyCollection<AttributeData> Attributes => throw new NotImplementedException();

        public ISourceGeneratorNode NamespaceGeneratorNode { get; }

        public IReadOnlyCollection<ISourceGeneratorNode> Children { get; }

        public void Accept(ISourceGeneratorNodeVisitor sourceGeneratorNodeVisitor)
        {
            sourceGeneratorNodeVisitor.VisitClass(this);
        }

        public void AddSourceText(ICodeGeneratorBuilder codeGeneratorBuilder)
        {
            // TODO: Aggregate usings 
            //       Remove user configurated interfaces and attributes by root namespace
            //       Interfaces

            codeGeneratorBuilder.AddLineOfSource($"public class {this.Name}");
        }
    }
}
