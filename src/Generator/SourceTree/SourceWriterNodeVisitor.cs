using Generator.SourceTree.Abstract;
using Generator.SourceTree.Model;
using Microsoft.CodeAnalysis;

namespace Generator.SourceTree
{
    internal sealed class SourceWriterNodeVisitor : ISourceGeneratorNodeVisitor
    {
        private readonly CodeGeneratorWriter codeGeneratorWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceWriterNodeVisitor"/> class.
        /// </summary>
        /// <param name="context">Source generator context.</param>
        public SourceWriterNodeVisitor(GeneratorExecutionContext context)
        {
            this.codeGeneratorWriter = new CodeGeneratorWriter(context);
        }

        public void VisitClass(ClassGeneratorNode classGeneratorNode)
        {
            classGeneratorNode.NamespaceGeneratorNode.Accept(this);
            this.codeGeneratorWriter.BeginWriteScope(classGeneratorNode.NamespaceGeneratorNode);

            classGeneratorNode.AddSourceText(this.codeGeneratorWriter);
            this.codeGeneratorWriter.BeginWriteScope(classGeneratorNode);
            foreach (var child in classGeneratorNode.Children)
            {
                child.Accept(this);
            }

            this.codeGeneratorWriter.EndWriteScope();
            this.codeGeneratorWriter.EndWriteScope();
        }

        public void VisitConstructor(ConstructorGeneratorNode constructorGeneratorNode)
        {
            constructorGeneratorNode.AddSourceText(this.codeGeneratorWriter);
        }

        public void VisitEnum(EnumGeneratorNode enumGeneratorNode)
        {
            enumGeneratorNode.NamespaceGeneratorNode.Accept(this);
            this.codeGeneratorWriter.BeginWriteScope(enumGeneratorNode.NamespaceGeneratorNode);

            enumGeneratorNode.AddSourceText(this.codeGeneratorWriter);
            this.codeGeneratorWriter.BeginWriteScope(enumGeneratorNode);
            foreach (var child in enumGeneratorNode.Children)
            {
                child.Accept(this);
            }

            this.codeGeneratorWriter.EndWriteScope();
            this.codeGeneratorWriter.EndWriteScope();
        }

        public void VisitField(FieldGeneratorNodeBase fieldGeneratorNode)
        {
            fieldGeneratorNode.AddSourceText(this.codeGeneratorWriter);
        }

        public void VisitNamespace(NamespaceGeneratorNode namespaceGeneratorNode)
        {
            namespaceGeneratorNode.AddSourceText(this.codeGeneratorWriter);
        }

        public void VisitProperty(PropertyGeneratorNode propertyGeneratorNode)
        {
            propertyGeneratorNode.AddSourceText(this.codeGeneratorWriter);
        }

        public void WriteSource()
        {
            // Essentially a buffer flush
            this.codeGeneratorWriter.Write();
        }
    }
}
