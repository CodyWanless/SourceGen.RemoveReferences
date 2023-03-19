using System;
using System.Collections.Generic;
using Generator.SourceTree.Abstract;
using Generator.SourceTree.Rules;
using Microsoft.CodeAnalysis;

namespace Generator.SourceTree.Model
{
    internal class ConstructorGeneratorNode : ISourceGeneratorNode
    {
        private readonly IMethodSymbol ctorSymbol;

        public ConstructorGeneratorNode(IMethodSymbol symbol)
        {
            this.ctorSymbol = symbol;
        }

        public string Name => this.ctorSymbol.Name;

        public IReadOnlyCollection<string> RequiredNamespaces { get; } = Array.Empty<string>();

        public IReadOnlyCollection<AttributeData> Attributes { get; } = Array.Empty<AttributeData>();

        public void Accept(ISourceGeneratorNodeVisitor sourceGeneratorNodeVisitor)
        {
            sourceGeneratorNodeVisitor.VisitConstructor(this);
        }

        public void AddSourceText(
            IRuleSet ruleSet,
            ICodeGeneratorBuilder codeGeneratorBuilder)
        {
            if (this.ctorSymbol.Parameters.Length == 0)
            {
                return;
            }

            codeGeneratorBuilder.AddLineOfSource($"public {this.ctorSymbol.ContainingType.Name}() {{}}");
        }
    }
}
