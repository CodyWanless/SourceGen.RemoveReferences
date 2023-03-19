using System;
using System.Linq;
using Generator.SourceTree.Abstract;
using Generator.SourceTree.Model;
using Microsoft.CodeAnalysis;

namespace Generator.SourceTree
{
    /// <summary>
    /// Constructs the source tree for top level ISymbols.
    /// </summary>
    internal sealed class SourceGeneratorNodeSymbolVisitor : SymbolVisitor<ISourceGeneratorNode>
    {
        private readonly string sourceAssemblyRootNamespace;
        private readonly string destinationAssemblyRootNamespace;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceGeneratorNodeSymbolVisitor"/> class.
        /// </summary>
        /// <param name="sourceAssemblyRootNamespace">Root namespace of source assembly.</param>
        /// <param name="destinationAssemblyRootNamespace">Root namespace of destination (this) assembly.</param>
        public SourceGeneratorNodeSymbolVisitor(
            string sourceAssemblyRootNamespace,
            string destinationAssemblyRootNamespace)
        {
            this.sourceAssemblyRootNamespace = sourceAssemblyRootNamespace;
            this.destinationAssemblyRootNamespace = destinationAssemblyRootNamespace;
        }

        /// <inheritdoc/>
        public override ISourceGeneratorNode? DefaultVisit(ISymbol symbol)
        {
            Console.WriteLine($"Unhandled type node: {symbol.Name}");
            return base.DefaultVisit(symbol);
        }

        /// <summary>
        /// Generates a single <see cref="EnumFieldGeneratorNode"/> for a <see cref="IFieldSymbol"/>.
        /// </summary>
        /// <param name="symbol">The <see cref="IFieldSymbol"/> to create the <see cref="EnumFieldGeneratorNode"/> from.</param>
        /// <returns>A collection containing a single <see cref="EnumFieldGeneratorNode"/>.</returns>
        public override ISourceGeneratorNode? VisitField(IFieldSymbol symbol)
        {
            return symbol.ContainingType.TypeKind switch
            {
                TypeKind.Enum => new EnumFieldGeneratorNode(symbol),
                TypeKind.Class => new ClassFieldGeneratorNode(symbol),
                _ => null,
            };
        }

        /// <summary>
        /// Generates a <see cref="ConstructorGeneratorNode"/> for a <see cref="IMethodSymbol"/>.
        /// Other method types are not being supported.
        /// </summary>
        /// <param name="symbol">The <see cref="IMethodSymbol"/> to create the <see cref="ConstructorGeneratorNode"/> from.</param>
        /// <returns>A <see cref="ConstructorGeneratorNode"/>. Null for other method types.</returns>
        public override ISourceGeneratorNode? VisitMethod(IMethodSymbol symbol)
        {
            // Only intend on handling constructors
            return (symbol.MethodKind, symbol.ContainingType.TypeKind) switch
            {
                (MethodKind.Constructor, TypeKind.Class) => new ConstructorGeneratorNode(symbol),
                _ => null,
            };
        }

        /// <summary>
        /// Generates a <see cref="PropertyGeneratorNode"/> for a <see cref="IPropertySymbol"/>.
        /// </summary>
        /// <param name="symbol">The <see cref="IPropertySymbol"/> to create the <see cref="PropertyGeneratorNode"/> from.</param>
        /// <returns>A <see cref="PropertyGeneratorNode"/>.</returns>
        public override ISourceGeneratorNode? VisitProperty(IPropertySymbol symbol)
        {
            var typeNode = new TypeGeneratorNode(symbol.Type);
            return new PropertyGeneratorNode(symbol, typeNode);
        }

        /// <summary>
        /// Generates a single top level node of either <see cref="EnumGeneratorNode"/> or <see cref="ClassGeneratorNode"/>
        /// based on the kind of <see cref="INamedTypeSymbol"/> given.
        /// </summary>
        /// <param name="symbol">The <see cref="INamedTypeSymbol"/> to generate the type from.</param>
        /// <returns>Either a <see cref="EnumGeneratorNode"/> or <see cref="ClassGeneratorNode"/>. Null for other type kinds.</returns>
        public override ISourceGeneratorNode? VisitNamedType(
            INamedTypeSymbol symbol)
        {
            if (symbol.DeclaredAccessibility != Accessibility.Public)
            {
                return null;
            }

            var members = symbol.GetMembers();
            var children = members
               .Select(m => m.Accept(this))
               .Where(n => n is not null)
               .Select(n => n ?? throw new NullReferenceException("Unexpected null child member"))
               .ToArray();

            var namespaceGeneratorNode = new NamespaceGeneratorNode(
                symbol.ContainingNamespace,
                this.sourceAssemblyRootNamespace,
                this.destinationAssemblyRootNamespace);

            return symbol switch
            {
                { TypeKind: TypeKind.Enum, IsDefinition: true } => new EnumGeneratorNode(symbol, namespaceGeneratorNode, children),
                { TypeKind: TypeKind.Class, IsDefinition: true } => new ClassGeneratorNode(symbol, namespaceGeneratorNode, children),
                _ => null,
            };
        }
    }
}
