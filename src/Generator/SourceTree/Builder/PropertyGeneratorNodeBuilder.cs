using Generator.SourceTree.Model;

namespace Generator.SourceTree.Builder
{
    internal class PropertyGeneratorNodeBuilder : SymbolGeneratorNodeBuilder<PropertyGeneratorNode, PropertyGeneratorNodeBuilder>
    {
        public PropertyGeneratorNodeBuilder(
            string sourceAssemblyName,
            string destinationAssemblyName)
            : base(sourceAssemblyName, destinationAssemblyName)
        {
        }

        protected override PropertyGeneratorNode Build(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}
