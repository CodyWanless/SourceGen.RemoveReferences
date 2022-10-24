using Generator.SourceTree.Model;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Generator.SourceTree.Builder
{
    internal class ClassGeneratorNodeBuilder : TypeGeneratorNodeBuilder<ClassGeneratorNode, ClassGeneratorNodeBuilder>
    {
        private readonly List<ISourceGeneratorNode> childGeneratorNodes = new();

        public ClassGeneratorNodeBuilder(
            string sourceAssemblyName,
            string destinationAssemblyName)
            : base(sourceAssemblyName, destinationAssemblyName)
        {
        }

        public override ClassGeneratorNodeBuilder AddTypeData(ITypeSymbol typeSymbol)
        {
            var classMembers = typeSymbol.GetMembers()
                .Where(this.CanIncludeSymbol)
                .ToArray();
            return this;
        }

        protected override ClassGeneratorNode Build(
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
                this.childGeneratorNodes);
        }

        protected override IEnumerable<ISourceGeneratorNode> GetChildrenNodes()
        {
            return this.childGeneratorNodes;
        }
    }
}
