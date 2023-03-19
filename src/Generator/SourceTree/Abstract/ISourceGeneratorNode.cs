using System.Collections.Generic;
using Generator.SourceTree.Rules;
using Microsoft.CodeAnalysis;

namespace Generator.SourceTree.Abstract
{
    internal interface ISourceGeneratorNode
    {
        string Name { get; }

        IReadOnlyCollection<string> RequiredNamespaces { get; }

        IReadOnlyCollection<AttributeData> Attributes { get; }

        void AddSourceText(
            IRuleSet ruleSet,
            ICodeGeneratorBuilder codeGeneratorBuilder);

        void Accept(ISourceGeneratorNodeVisitor sourceGeneratorNodeVisitor);
    }
}
