using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generator.SourceTree.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Generator
{
    internal sealed class CodeGeneratorWriter : ICodeGeneratorWriter, ICodeGeneratorBuilder, ICodeGeneratorScope
    {
        private readonly StringBuilder stringBuilder;
        private readonly Stack<WriteScopeDetails> writeScopeDetailStack;
        private readonly GeneratorExecutionContext context;
        private readonly string typeName;

        private WriteScopeDetails currentWriteDetails;

        public CodeGeneratorWriter(
            GeneratorExecutionContext context,
            string typeName)
        {
            this.typeName = typeName;
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
            this.stringBuilder.Append(this.currentWriteDetails.IndentWhitespace);
            this.stringBuilder.Append(sourceText);
        }

        public void BeginWriteScope(ISourceGeneratorNode node)
        {
            // Add brace to denote new scope
            this.stringBuilder.AppendLine("{");

            // Capture parent information and update indentation
            WriteScopeDetails writeScopeSettings = this.currentWriteDetails.Parent is null
                ? new(node, 0)
                : new(node, this.currentWriteDetails.IndentationLevel + 4);
            this.writeScopeDetailStack.Push(this.currentWriteDetails);
            this.currentWriteDetails = writeScopeSettings;
        }

        public void EndWriteScope()
        {
            // Reset to prior parent
            this.currentWriteDetails = this.writeScopeDetailStack.Pop();

            // Add closing brace
            this.stringBuilder.AppendLine("}");
        }

        public void Write()
        {
            if (this.typeName is not { Length: > 0 })
            {
                throw new InvalidOperationException($"{nameof(this.typeName)} not set. Be sure to invoke {nameof(this.BeginWriteScope)} at least once.");
            }

            this.context.AddSource(this.typeName, SourceText.From(this.stringBuilder.ToString(), Encoding.UTF8, SourceHashAlgorithm.Sha256));
        }

        private record WriteScopeDetails
        {
            public WriteScopeDetails(
                ISourceGeneratorNode? parent,
                int indentationLevel)
            {
                this.Parent = parent;
                this.IndentationLevel = indentationLevel;
                this.IndentWhitespace = new string(Enumerable.Repeat(' ', indentationLevel).ToArray());
            }

            public ISourceGeneratorNode? Parent { get; }

            public int IndentationLevel { get; }

            public string IndentWhitespace { get; }
        }
    }
}
