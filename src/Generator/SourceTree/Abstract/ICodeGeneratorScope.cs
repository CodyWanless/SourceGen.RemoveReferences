using Generator.SourceTree;

namespace Generator.SourceTree.Abstract
{
    internal interface ICodeGeneratorScope : ICodeGeneratorBuilder
    {
        void BeginWriteScope(ISourceGeneratorNode node);

        void EndWriteScope();
    }
}
