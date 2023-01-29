using System;
using System.Collections.Generic;
using Generator.SourceTree.Abstract;
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
            ICodeGeneratorBuilder codeGeneratorBuilder)
        {
            codeGeneratorBuilder.AddLineOfSource($"public {this.ctorSymbol.ContainingType.Name}() {{}}");
        }
    }
}
