using System;
using System.Collections.Generic;
using System.Linq;
using Generator.SourceTree.Abstract;
using Generator.SourceTree.Rules;
using Microsoft.CodeAnalysis;

namespace Generator.SourceTree.Model
{
    internal class TypeGeneratorNode : ISourceGeneratorNode
    {
        private readonly ITypeSymbol namedTypeSymbol;
        private readonly IReadOnlyList<ISourceGeneratorNode> typeArgNodes;

        public TypeGeneratorNode(
            ITypeSymbol namedTypeSymbol,
            IReadOnlyList<ISourceGeneratorNode> typeArgNodes)
        {
            this.namedTypeSymbol = namedTypeSymbol;
            this.typeArgNodes = typeArgNodes;
        }

        public string Name => this.namedTypeSymbol.Name;

        public IReadOnlyCollection<string> RequiredNamespaces =>
            new[] { this.namedTypeSymbol.GetFullNamespace() }
                .Concat(this.typeArgNodes.SelectMany(n => n.RequiredNamespaces))
                .ToArray();

        public IReadOnlyCollection<AttributeData> Attributes { get; } = Array.Empty<AttributeData>();

        public void Accept(ISourceGeneratorNodeVisitor sourceGeneratorNodeVisitor)
        {
            sourceGeneratorNodeVisitor.VisitType(this);
        }

        public void AddSourceText(IRuleSet ruleSet, ICodeGeneratorBuilder codeGeneratorBuilder)
        {
            if (ruleSet.IsAllowedNamespace(this.namedTypeSymbol.GetFullNamespace()))
            {
                if (this.typeArgNodes.Count > 0)
                {
                    codeGeneratorBuilder.AddSource(this.namedTypeSymbol.Name);
                    codeGeneratorBuilder.AddSource("<");

                    for (int i = 0; i < this.typeArgNodes.Count; i++)
                    {
                        ISourceGeneratorNode? typeArgNode = this.typeArgNodes[i];
                        typeArgNode.AddSourceText(ruleSet, codeGeneratorBuilder);

                        if (i < this.typeArgNodes.Count - 1)
                        {
                            codeGeneratorBuilder.AddSource(", ");
                        }
                    }

                    codeGeneratorBuilder.AddSource(">");
                }
                else
                {
                    codeGeneratorBuilder.AddSource(this.namedTypeSymbol.Name);
                }
            }
            else
            {
                codeGeneratorBuilder.AddSource("object");
            }
        }
    }
}
