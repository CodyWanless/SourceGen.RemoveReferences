using System;
using System.Collections.Generic;
using System.Linq;
using Generator.SourceTree.Abstract;
using Generator.SourceTree.Rules;
using Microsoft.CodeAnalysis;

namespace Generator.SourceTree.Model
{
    internal record ClassGeneratorNode : ISourceGeneratorNode
    {
        private readonly INamedTypeSymbol namedTypeSymbol;

        public ClassGeneratorNode(
            INamedTypeSymbol symbol,
            NamespaceGeneratorNode namespaceGeneratorNode,
            IReadOnlyCollection<ISourceGeneratorNode> children)
        {
            this.namedTypeSymbol = symbol;
            this.NamespaceGeneratorNode = namespaceGeneratorNode;
            this.Children = children;
        }

        public IReadOnlyCollection<INamedTypeSymbol> Interfaces { get; } = Array.Empty<INamedTypeSymbol>();

        public string Name => this.namedTypeSymbol.Name;

        public IReadOnlyCollection<string> RequiredNamespaces =>
            this.Children
                .SelectMany(c => c.RequiredNamespaces)
                .Distinct()
                .ToArray();

        public IReadOnlyCollection<AttributeData> Attributes => throw new NotImplementedException();

        public NamespaceGeneratorNode NamespaceGeneratorNode { get; }

        public IReadOnlyCollection<ISourceGeneratorNode> Children { get; }

        public void Accept(ISourceGeneratorNodeVisitor sourceGeneratorNodeVisitor)
        {
            sourceGeneratorNodeVisitor.VisitClass(this);
        }

        public void AddSourceText(
            IRuleSet ruleSet,
            ICodeGeneratorBuilder codeGeneratorBuilder)
        {
            // TODO: Default values
            //       Non auto-properties
            //       Interfaces
            foreach (var dependency in this.RequiredNamespaces
                .Where(ruleSet.IsAllowedNamespace)
                .Select(this.NamespaceGeneratorNode.GetNewNamespace)
                .OrderBy(s => s))
            {
                codeGeneratorBuilder.AddLineOfSource($"using {dependency};");
            }

            codeGeneratorBuilder.AddNewLine();
            codeGeneratorBuilder.AddLineOfSource($"public class {this.Name}");
        }
    }
}
