using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Generator.SourceTree.Model
{
    internal record EnumGeneratorNode : TypeGeneratorNode
    {
        private readonly IReadOnlyCollection<ISourceGeneratorNode> childSourceGeneratorNodes;

        public EnumGeneratorNode(
            string name,
            string @namespace,
            IReadOnlyCollection<INamedTypeSymbol> interfaces,
            IReadOnlyCollection<AttributeData> attributes,
            IReadOnlyCollection<string> usingDeclarations,
            IReadOnlyCollection<ISourceGeneratorNode> childSourceGeneratorNodes)
            : base(name, @namespace, interfaces, attributes, usingDeclarations)
        {
            this.childSourceGeneratorNodes = childSourceGeneratorNodes;
        }

        public override void AddSourceText(ICodeGeneratorBuilder codeGeneratorBuilder)
        {
            foreach (var usingDeclaration in this.RequiredNamespaces)
            {
                codeGeneratorBuilder.AddLineOfSource($"using {usingDeclaration}");
            }
            codeGeneratorBuilder.AddNewLine();

            codeGeneratorBuilder.AddLineOfSource($"namespace {this.Namespace}");
            codeGeneratorBuilder.AddLineOfSource("{");

            codeGeneratorBuilder.AddLineOfSource($"public class {this.Name}");
            codeGeneratorBuilder.AddLineOfSource("{");

            foreach (var child in this.childSourceGeneratorNodes)
            {
                child.AddSourceText(codeGeneratorBuilder);
            }

            codeGeneratorBuilder.AddLineOfSource("}");

            codeGeneratorBuilder.AddLineOfSource("}");
        }
    }
}
