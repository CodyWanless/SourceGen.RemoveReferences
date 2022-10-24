using Generator.SourceTree.Model;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Generator.SourceTree.Builder
{
    internal class EnumGeneratorNodeBuilder : TypeGeneratorNodeBuilder<EnumGeneratorNode, EnumGeneratorNodeBuilder>
    {
        private readonly List<ISourceGeneratorNode> fieldGeneratorNodes = new();

        public EnumGeneratorNodeBuilder(
            string sourceAssemblyName,
            string destinationAssemblyName)
            : base(sourceAssemblyName, destinationAssemblyName)
        {
        }

        public override EnumGeneratorNodeBuilder AddTypeData(
            ITypeSymbol typeSymbol)
        {
            var enumFields = typeSymbol.GetMembers()
                .Where(ts => ts.Kind == SymbolKind.Field);

            return this;
        }

        protected override EnumGeneratorNode Build(
            string name,
            string updatedNamespace,
            IReadOnlyCollection<INamedTypeSymbol> interfaces,
            IReadOnlyCollection<AttributeData> attributes,
            SortedSet<string> usingDeclartions)
        {
            return new(
                name,
                updatedNamespace,
                interfaces,
                attributes,
                usingDeclartions,
                this.fieldGeneratorNodes);
        }

        protected override IEnumerable<ISourceGeneratorNode> GetChildrenNodes()
        {
            return this.fieldGeneratorNodes;
        }
    }
}
