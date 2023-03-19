using Generator.SourceTree.Abstract;
using Generator.SourceTree.Rules;
using Microsoft.CodeAnalysis;

namespace Generator.SourceTree.Model
{
    internal record ClassFieldGeneratorNode : FieldGeneratorNodeBase
    {
        public ClassFieldGeneratorNode(IFieldSymbol fieldSymbol)
            : base(fieldSymbol)
        {
        }

        public override void AddSourceText(
            IRuleSet ruleSet,
            ICodeGeneratorBuilder codeGeneratorBuilder)
        {
            codeGeneratorBuilder.AddLineOfSource($"{this.FieldSymbol.GetAccessibilityString()} {this.GetReadonly()} {this.GetTypeName(ruleSet)} {this.GetMemberName()};");
        }

        private string GetReadonly()
        {
            return this.FieldSymbol.IsReadOnly ? "readonly" : string.Empty;
        }

        private string GetTypeName(IRuleSet ruleSet)
        {
            return ruleSet.IsAllowedType(this)
                ? this.FieldSymbol.Type.Name
                : "object";
        }

        private string GetMemberName()
        {
            return this.FieldSymbol.Name;
        }
    }
}
