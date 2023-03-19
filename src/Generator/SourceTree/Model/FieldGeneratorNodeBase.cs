using System.Collections.Generic;
using Generator.SourceTree.Abstract;
using Generator.SourceTree.Rules;
using Microsoft.CodeAnalysis;

namespace Generator.SourceTree.Model
{
    internal abstract record FieldGeneratorNodeBase : ISourceGeneratorNode
    {
        public FieldGeneratorNodeBase(IFieldSymbol fieldSymbol)
        {
            this.FieldSymbol = fieldSymbol;
        }

        protected IFieldSymbol FieldSymbol { get; }

        public string Name => this.FieldSymbol.Name;

        public IReadOnlyCollection<AttributeData> Attributes
            => this.FieldSymbol.GetAttributes();

        public IReadOnlyCollection<string> RequiredNamespaces =>
            new[] { this.FieldSymbol.Type.GetFullNamespace() };

        public void Accept(ISourceGeneratorNodeVisitor sourceGeneratorNodeVisitor)
        {
            sourceGeneratorNodeVisitor.VisitField(this);
        }

        public abstract void AddSourceText(IRuleSet ruleSet, ICodeGeneratorBuilder codeGeneratorBuilder);
    }
}
