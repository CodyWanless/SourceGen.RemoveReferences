using System;
using Generator.SourceTree.Abstract;
using Generator.SourceTree.Model;
using Generator.SourceTree.Rules;
using Microsoft.CodeAnalysis;
using RuleSet = Generator.SourceTree.Rules.RuleSet;

namespace Generator.SourceTree
{
    internal sealed class SourceWriterNodeVisitor : ISourceGeneratorNodeVisitor
    {
        private readonly CodeGeneratorWriter codeGeneratorWriter;
        private readonly IRuleSet ruleSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceWriterNodeVisitor"/> class.
        /// </summary>
        /// <param name="context">Source generator context.</param>
        /// <param name="typeName">Name of containing type. Either the class or enum type name.</param>
        public SourceWriterNodeVisitor(
            GeneratorExecutionContext context,
            string typeName)
        {
            this.codeGeneratorWriter = new CodeGeneratorWriter(context, typeName);
            if (!context.AnalyzerConfigOptions.GlobalOptions
                    .TryGetValue(Constants.ExcludeNamespaceBuildPropertyName, out var excludeNamespaces)
                || string.IsNullOrWhiteSpace(excludeNamespaces))
            {
                throw new InvalidOperationException($"Expected {Constants.ExcludeNamespaceBuildPropertyName} to be a comma delimited string of namespaces.");
            }

            this.ruleSet = new RuleSet(excludeNamespaces.Split(',', ';'));
        }

        public void VisitClass(ClassGeneratorNode classGeneratorNode)
        {
            classGeneratorNode.NamespaceGeneratorNode.Accept(this);
            this.codeGeneratorWriter.BeginWriteScope(classGeneratorNode.NamespaceGeneratorNode);

            classGeneratorNode.AddSourceText(this.ruleSet, this.codeGeneratorWriter);
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
            constructorGeneratorNode.AddSourceText(this.ruleSet, this.codeGeneratorWriter);
        }

        public void VisitEnum(EnumGeneratorNode enumGeneratorNode)
        {
            enumGeneratorNode.NamespaceGeneratorNode.Accept(this);
            this.codeGeneratorWriter.BeginWriteScope(enumGeneratorNode.NamespaceGeneratorNode);

            enumGeneratorNode.AddSourceText(this.ruleSet, this.codeGeneratorWriter);
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
            fieldGeneratorNode.AddSourceText(this.ruleSet, this.codeGeneratorWriter);
        }

        public void VisitNamespace(NamespaceGeneratorNode namespaceGeneratorNode)
        {
            namespaceGeneratorNode.AddSourceText(this.ruleSet, this.codeGeneratorWriter);
        }

        public void VisitProperty(PropertyGeneratorNode propertyGeneratorNode)
        {
            propertyGeneratorNode.AddSourceText(this.ruleSet, this.codeGeneratorWriter);
        }

        public void VisitType(TypeGeneratorNode typeGeneratorNode)
        {
            typeGeneratorNode.AddSourceText(this.ruleSet, this.codeGeneratorWriter);
        }

        public void WriteSource()
        {
            // Essentially a buffer flush
            this.codeGeneratorWriter.Write();
        }
    }
}
