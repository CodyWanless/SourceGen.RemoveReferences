using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace Generator.SourceTree.Model
{
    internal record PropertyGeneratorNode : ISourceGeneratorNode
    {
        public string Name { get; }

        public string GetAccessModifier { get; }

        public string SetAccessModifier { get; }

        public IReadOnlyCollection<AttributeData> Attributes { get; }

        // This capture property return type only
        public IReadOnlyCollection<string> RequiredNamespaces => throw new NotImplementedException();

        public void AddSourceText(ICodeGeneratorBuilder codeGeneratorBuilder)
        {
            throw new NotImplementedException();
        }
    }
}
