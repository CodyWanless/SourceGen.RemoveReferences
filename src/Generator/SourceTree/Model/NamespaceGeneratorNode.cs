using System;
using System.Collections.Generic;
using Generator.SourceTree.Abstract;
using Microsoft.CodeAnalysis;

namespace Generator.SourceTree.Model
{
    internal class NamespaceGeneratorNode : ISourceGeneratorNode
    {
        private readonly INamespaceSymbol namespaceSymbol;
        private readonly string sourceAssemblyRootNamespace;
        private readonly string destinationAssemblyRootNamespace;

        private string? newAssemblyNamespace;

        public NamespaceGeneratorNode(
            INamespaceSymbol namespaceSymbol,
            string sourceAssemblyRootNamespace,
            string destinationAssemblyRootNamespace)
        {
            this.namespaceSymbol = namespaceSymbol;
            this.sourceAssemblyRootNamespace = sourceAssemblyRootNamespace;
            this.destinationAssemblyRootNamespace = destinationAssemblyRootNamespace;
        }

        /// <inheritdoc />
        public string Name => this.namespaceSymbol.Name;

        public string NewAssemblyNamespace
        {
            get
            {
                if (this.newAssemblyNamespace == null)
                {
                    this.newAssemblyNamespace = this.namespaceSymbol
                        .GetFullNamespace()
                        .Replace(this.sourceAssemblyRootNamespace, this.destinationAssemblyRootNamespace);
                }

                return this.newAssemblyNamespace;
            }
        }

        /// <inheritdoc />
        public IReadOnlyCollection<string> RequiredNamespaces { get; } = Array.Empty<string>();

        /// <inheritdoc />
        public IReadOnlyCollection<AttributeData> Attributes { get; } = Array.Empty<AttributeData>();

        public void Accept(ISourceGeneratorNodeVisitor sourceGeneratorNodeVisitor)
        {
            sourceGeneratorNodeVisitor.VisitNamespace(this);
        }

        public void AddSourceText(
            ICodeGeneratorBuilder codeGeneratorBuilder)
        {
            // TODO: This has to be updated to align with new assembly
            codeGeneratorBuilder.AddLineOfSource($"namespace {this.NewAssemblyNamespace}");
        }

        internal string GetNewNamespace(
            string existing)
        {
            if (existing.StartsWith(this.sourceAssemblyRootNamespace))
            {
                return existing.Replace(this.sourceAssemblyRootNamespace, this.destinationAssemblyRootNamespace);
            }

            return existing;
        }
    }
}
