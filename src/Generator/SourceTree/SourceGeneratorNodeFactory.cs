using Generator.SourceTree.Builder;
using Generator.SourceTree.Model;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Generator.SourceTree
{
    internal class SourceGeneratorNodeFactory
    {
        private readonly string sourceAssemblyRootNamespace;
        private readonly string sourceAssemblyName;

        public SourceGeneratorNodeFactory(
            string sourceAssemblyRootNamespace,
            string sourceAssemblyName)
        {
            this.sourceAssemblyRootNamespace = sourceAssemblyRootNamespace;
            this.sourceAssemblyName = sourceAssemblyName;
        }

        public IReadOnlyCollection<ISourceGeneratorNode> CreateGeneratorsFromTypes(
            IReadOnlyCollection<ITypeSymbol> typeSymbols)
        {
            var topLevelNodes = new List<ISourceGeneratorNode>(typeSymbols.Count);

            foreach (var typeSymbol in typeSymbols)
            {
                TypeGeneratorNode? node = typeSymbol!.TypeKind switch
                {
                    TypeKind.Class => CreateClassGeneratorNode(typeSymbol),
                    TypeKind.Enum => CreateEnumGeneratorNode(typeSymbol),
                    _ => null,
                };

                if (node is null)
                {
                    continue;
                }
                topLevelNodes.Add(node);
            }

            return topLevelNodes;
        }

        private ClassGeneratorNode CreateClassGeneratorNode(ITypeSymbol classSymbol)
        {
            return new ClassGeneratorNodeBuilder(this.sourceAssemblyRootNamespace, this.sourceAssemblyName)
                .AddNamespace(classSymbol)
                .AddAttributes(classSymbol)
                .AddName(classSymbol)
                .AddInterfaces(classSymbol)
                .AddTypeData(classSymbol)
                .Build();
        }

        private EnumGeneratorNode CreateEnumGeneratorNode(ITypeSymbol enumTypeSymbol)
        {
            return new EnumGeneratorNodeBuilder(this.sourceAssemblyRootNamespace, this.sourceAssemblyName)
                .AddNamespace(enumTypeSymbol)
                .AddAttributes(enumTypeSymbol)
                .AddName(enumTypeSymbol)
                .AddInterfaces(enumTypeSymbol)
                .AddTypeData(enumTypeSymbol)
                .Build();
        }
    }
}
