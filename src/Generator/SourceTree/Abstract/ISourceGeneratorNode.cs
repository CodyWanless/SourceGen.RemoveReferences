using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Generator.SourceTree.Abstract
{
    internal interface ISourceGeneratorNode
    {
        string Name { get; }

        IReadOnlyCollection<string> RequiredNamespaces { get; }

        IReadOnlyCollection<AttributeData> Attributes { get; }

        void AddSourceText(ICodeGeneratorBuilder codeGeneratorBuilder);

        void Accept(ISourceGeneratorNodeVisitor sourceGeneratorNodeVisitor);
    }
}
