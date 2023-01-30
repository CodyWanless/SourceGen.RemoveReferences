using Generator.SourceTree.Abstract;
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
            ICodeGeneratorBuilder codeGeneratorBuilder)
        {
            codeGeneratorBuilder.AddLineOfSource($"{this.fieldSymbol.GetAccessibilityString()} {this.GetReadonly()} {this.GetTypeName()} {this.GetMemberName()};");
        }

        private string GetReadonly()
        {
            return this.fieldSymbol.IsReadOnly ? "readonly" : string.Empty;
        }

        private string GetTypeName()
        {
            return this.fieldSymbol.Type.Name;
        }

        private string GetMemberName()
        {
            return this.fieldSymbol.Name;
        }
    }
}
