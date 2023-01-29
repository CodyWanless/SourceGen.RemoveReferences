using System;
using System.Collections.Generic;
using Generator.SourceTree.Abstract;
using Microsoft.CodeAnalysis;

namespace Generator.SourceTree.Model
{
    internal record PropertyGeneratorNode : ISourceGeneratorNode
    {
        private readonly IPropertySymbol propertySymbol;
        private readonly IMethodSymbol? getMethodSymbol;
        private readonly IMethodSymbol? setMethodSymbol;

        public PropertyGeneratorNode(IPropertySymbol symbol)
        {
            this.propertySymbol = symbol;
            this.getMethodSymbol = symbol.GetMethod;
            this.setMethodSymbol = symbol.SetMethod;
        }

        public string Name => this.propertySymbol.Name;

        public IReadOnlyCollection<AttributeData> Attributes => this.propertySymbol.GetAttributes();

        // This capture property return type only
        public IReadOnlyCollection<string> RequiredNamespaces => throw new NotImplementedException();

        public void Accept(ISourceGeneratorNodeVisitor sourceGeneratorNodeVisitor)
        {
            sourceGeneratorNodeVisitor.VisitProperty(this);
        }

        public void AddSourceText(
            ICodeGeneratorBuilder codeGeneratorBuilder)
        {
            codeGeneratorBuilder.AddLineOfSource($"{this.propertySymbol.GetAccessibilityString()} {this.propertySymbol.Type.Name} {this.propertySymbol.Name} {this.CreateGetterAndSetter()}");
        }

        private string CreateGetterAndSetter()
        {
            return $"{{ {this.CreateGetMethod()} {this.CreateSetMethod()}}}";
        }

        private string? CreateGetMethod()
        {
            if (this.getMethodSymbol is null)
            {
                return null;
            }

            var accessibility = string.Empty;
            if (this.getMethodSymbol.DeclaredAccessibility != Accessibility.Public)
            {
                accessibility = this.getMethodSymbol.GetAccessibilityString();
            }

            return $"{accessibility} get;".Trim();
        }

        private string? CreateSetMethod()
        {
            if (this.setMethodSymbol is null
                || this.propertySymbol.IsReadOnly)
            {
                return null;
            }

            var accessibility = string.Empty;
            if (this.setMethodSymbol.DeclaredAccessibility != Accessibility.Public)
            {
                accessibility = this.setMethodSymbol.GetAccessibilityString();
            }

            return $"{accessibility} set;".Trim();
        }
    }
}
