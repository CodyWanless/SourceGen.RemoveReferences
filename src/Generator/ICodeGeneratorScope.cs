using Generator.SourceTree;

namespace Generator
{
    internal interface ICodeGeneratorScope : ICodeGeneratorBuilder
    {
        void BeginWriteScope(ISourceGeneratorNode node);

        void EndWriteScope();
    }
}
