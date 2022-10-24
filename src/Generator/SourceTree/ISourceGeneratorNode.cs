using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Generator.SourceTree
{
    internal interface ISourceGeneratorNode
    {
        string Name { get; }

        IReadOnlyCollection<string> RequiredNamespaces { get; }

        IReadOnlyCollection<AttributeData> Attributes { get; }

        void AddSourceText(ICodeGeneratorBuilder codeGeneratorBuilder);
    }
}
