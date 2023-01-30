using Generator.SourceTree.Abstract;
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
            ICodeGeneratorBuilder codeGeneratorBuilder)
        {
            codeGeneratorBuilder.AddLineOfSource($"{this.fieldSymbol.Name} = {this.fieldSymbol.ConstantValue!},");
        }
    }
}
