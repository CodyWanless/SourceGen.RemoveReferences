using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Generator.SourceTree.Model
{
    internal record ClassGeneratorNode : TypeGeneratorNode
    {
        private readonly IReadOnlyCollection<ISourceGeneratorNode> childrenNodes;

        public ClassGeneratorNode(
            string name,
            string @namespace,
            IReadOnlyCollection<INamedTypeSymbol> interfaces,
            IReadOnlyCollection<AttributeData> attributes,
            IReadOnlyCollection<string> usingDeclarations,
            IReadOnlyCollection<ISourceGeneratorNode> childGeneratorNodes)
            : base(name, @namespace, interfaces, attributes, usingDeclarations)
        {
            this.childrenNodes = childGeneratorNodes;
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

            foreach (var child in this.childrenNodes)
            {
                child.AddSourceText(codeGeneratorBuilder);
            }

            codeGeneratorBuilder.AddLineOfSource("}");

            codeGeneratorBuilder.AddLineOfSource("}");
        }
    }
}
