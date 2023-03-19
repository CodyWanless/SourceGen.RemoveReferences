using Generator.SourceTree.Abstract;

namespace Generator.SourceTree.Rules
{
    internal interface IRuleSet
    {
        bool IsAllowedNamespace(string @namespace);

        bool IsAllowedType(ISourceGeneratorNode sourceGeneratorNode);
    }
}
