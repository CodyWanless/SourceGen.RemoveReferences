using System;
using System.Collections.Generic;
using Generator.SourceTree.Abstract;
using Generator.SourceTree.Rules;
using Microsoft.CodeAnalysis;

namespace Generator.SourceTree.Model
{
    internal class TypeGeneratorNode : ISourceGeneratorNode
    {
        private readonly ITypeSymbol namedTypeSymbol;

        public TypeGeneratorNode(
            ITypeSymbol namedTypeSymbol)
        {
            this.namedTypeSymbol = namedTypeSymbol;
        }

        public string Name => this.namedTypeSymbol.Name;

        public IReadOnlyCollection<string> RequiredNamespaces =>
            new[] { this.namedTypeSymbol.GetFullNamespace() };

        public IReadOnlyCollection<AttributeData> Attributes { get; } = Array.Empty<AttributeData>();

        public void Accept(ISourceGeneratorNodeVisitor sourceGeneratorNodeVisitor)
        {
            sourceGeneratorNodeVisitor.VisitType(this);
        }

        public void AddSourceText(IRuleSet ruleSet, ICodeGeneratorBuilder codeGeneratorBuilder)
        {
            if (ruleSet.IsAllowedNamespace(this.namedTypeSymbol.GetFullNamespace()))
            {
                codeGeneratorBuilder.AddSource(this.namedTypeSymbol.Name);
            }
            else
            {
                codeGeneratorBuilder.AddSource("object");
            }
        }
    }
}
