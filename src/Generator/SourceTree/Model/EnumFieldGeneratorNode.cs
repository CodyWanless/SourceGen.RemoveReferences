using Generator.SourceTree.Abstract;
using Generator.SourceTree.Rules;
using Microsoft.CodeAnalysis;

namespace Generator.SourceTree.Model
{
    internal record EnumFieldGeneratorNode : FieldGeneratorNodeBase
    {
        public EnumFieldGeneratorNode(IFieldSymbol fieldSymbol)
            : base(fieldSymbol)
        {
        }

        public override void AddSourceText(
            IRuleSet ruleSet,
            ICodeGeneratorBuilder codeGeneratorBuilder)
        {
            codeGeneratorBuilder.AddLineOfSource($"{this.FieldSymbol.Name} = {this.FieldSymbol.ConstantValue!},");
        }
    }
}
