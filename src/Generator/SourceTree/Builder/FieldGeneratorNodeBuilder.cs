using Generator.SourceTree.Model;
using System;

namespace Generator.SourceTree.Builder
{
    internal class FieldGeneratorNodeBuilder : SymbolGeneratorNodeBuilder<FieldGeneratorNode, FieldGeneratorNodeBuilder>
    {
        public FieldGeneratorNodeBuilder(
            string sourceAssemblyName,
            string destinationAssemblyName)
            : base(sourceAssemblyName, destinationAssemblyName)
        {
        }

        protected override FieldGeneratorNode Build(string name)
        {
            throw new NotImplementedException();
        }
    }
}
