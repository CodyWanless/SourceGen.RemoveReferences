using System.Collections.Generic;
using Generator.SourceTree.Abstract;
using Microsoft.CodeAnalysis;

namespace Generator.SourceTree.Model
{
    internal abstract record FieldGeneratorNodeBase : ISourceGeneratorNode
    {
        protected readonly IFieldSymbol fieldSymbol;

        public FieldGeneratorNodeBase(IFieldSymbol fieldSymbol)
        {
            this.fieldSymbol = fieldSymbol;
        }

        public string Name => this.fieldSymbol.Name;

        public IReadOnlyCollection<AttributeData> Attributes
            => this.fieldSymbol.GetAttributes();

        public IReadOnlyCollection<string> RequiredNamespaces =>
            new[] { this.fieldSymbol.Type.GetFullNamespace() };

        public void Accept(ISourceGeneratorNodeVisitor sourceGeneratorNodeVisitor)
        {
            sourceGeneratorNodeVisitor.VisitField(this);
        }

        public abstract void AddSourceText(ICodeGeneratorBuilder codeGeneratorBuilder);
    }
}
