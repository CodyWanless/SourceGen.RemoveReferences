using Generator.SourceTree;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generator
{
    internal class CodeGeneratorWriter : ICodeGeneratorWriter, ICodeGeneratorBuilder, ICodeGeneratorScope
    {
        private readonly StringBuilder stringBuilder;
        private readonly Stack<WriteScopeDetails> writeScopeDetailStack;
        private readonly GeneratorExecutionContext context;

        private WriteScopeDetails currentWriteDetails;
        private string? typeName;

        public CodeGeneratorWriter(GeneratorExecutionContext context)
        {
            this.stringBuilder = new();
            this.writeScopeDetailStack = new();
            this.currentWriteDetails = new(null, 0);
            this.context = context;
        }

        public int CurrentIndentation => this.currentWriteDetails.IndentationLevel;

        public void AddNewLine()
        {
            this.stringBuilder.AppendLine();
        }

        public void AddLineOfSource(string sourceText)
        {
            this.stringBuilder.Append(this.currentWriteDetails.GetIndentWhitespace());
            this.stringBuilder.Append(sourceText);
        }

        public void BeginWriteScope(ISourceGeneratorNode node)
        {
            WriteScopeDetails writeScopeSettings = this.currentWriteDetails.Parent is null
                ? new(node, 0)
                : new(node, this.currentWriteDetails.IndentationLevel + 4);
            this.typeName ??= node.Name;
            this.writeScopeDetailStack.Push(this.currentWriteDetails);
            this.currentWriteDetails = writeScopeSettings;
        }

        public void EndWriteScope()
        {
            this.currentWriteDetails = this.writeScopeDetailStack.Pop();
        }

        public void Write()
        {
            if (this.typeName is not { Length: > 0 })
            {
                throw new InvalidOperationException($"{nameof(this.typeName)} not set. Be sure to invoke {nameof(BeginWriteScope)} at least once.");
            }

            this.context.AddSource(this.typeName, SourceText.From(this.stringBuilder.ToString(), Encoding.UTF8, SourceHashAlgorithm.Sha256));
        }

        private record WriteScopeDetails
        {
            private readonly string indentWhitespace;

            public WriteScopeDetails(
                ISourceGeneratorNode? parent,
                int indentationLevel)
            {
                this.Parent = parent;
                this.IndentationLevel = indentationLevel;
                this.indentWhitespace = new string(Enumerable.Repeat(' ', indentationLevel).ToArray());
            }

            public ISourceGeneratorNode? Parent { get; }

            public int IndentationLevel { get; }

            public string GetIndentWhitespace() => this.indentWhitespace;
        }
    }
}
