using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace Generator.SourceTree.Model
{
    internal record FieldGeneratorNode : ISourceGeneratorNode
    {
        public string Name { get; }

        public IReadOnlyCollection<AttributeData> Attributes { get; }

        // This capture field type only
        public IReadOnlyCollection<string> RequiredNamespaces => throw new NotImplementedException();

        public void AddSourceText(ICodeGeneratorBuilder codeGeneratorBuilder)
        {
            throw new NotImplementedException();
        }
    }
}
