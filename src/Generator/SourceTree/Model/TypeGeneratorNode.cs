using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Generator.SourceTree.Model
{
    internal abstract record TypeGeneratorNode : ISourceGeneratorNode
    {
        protected TypeGeneratorNode(
            string name,
            string @namespace,
            IReadOnlyCollection<INamedTypeSymbol> interfaces,
            IReadOnlyCollection<AttributeData> attributes,
            IReadOnlyCollection<string> usingDeclarations)
        {
            this.Name = name;
            this.Namespace = @namespace;
            this.Interfaces = interfaces;
            this.Attributes = attributes;
            this.RequiredNamespaces = usingDeclarations;
        }

        public string Name { get; }

        public string Namespace { get; }

        public IReadOnlyCollection<INamedTypeSymbol> Interfaces { get; }

        public IReadOnlyCollection<AttributeData> Attributes { get; }

        public IReadOnlyCollection<string> RequiredNamespaces { get; }

        public void AddSourceTextWithScope(ICodeGeneratorScope codeGeneratorScope)
        {
            try
            {
                codeGeneratorScope.BeginWriteScope(this);
                this.AddSourceText(codeGeneratorScope);
            }
            finally
            {
                codeGeneratorScope.EndWriteScope();
            }
        }

        public abstract void AddSourceText(ICodeGeneratorBuilder codeGeneratorBuilder);
    }
}
