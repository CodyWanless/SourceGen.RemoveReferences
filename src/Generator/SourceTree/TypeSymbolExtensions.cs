using Microsoft.CodeAnalysis;

namespace Generator.SourceTree
{
    internal static class TypeSymbolExtensions
    {
        public static string GetFullNamespace(this ISymbol typeSymbol)
        {
            return GetFullNamespace(typeSymbol.ContainingNamespace);
        }

        public static string GetFullNamespace(this INamespaceSymbol namespaceSymbol)
        {
            if (namespaceSymbol.ContainingNamespace.Name is not { Length: > 0 })
            {
                return namespaceSymbol.Name;
            }

            var prefix = GetFullNamespace(namespaceSymbol.ContainingNamespace);
            if (prefix is { Length: > 0 })
            {
                return $"{prefix}.{namespaceSymbol.Name}";
            }

            return namespaceSymbol.Name;
        }

        public static string GetAccessibilityString(this ISymbol typeSymbol)
        {
            return typeSymbol.DeclaredAccessibility switch
            {
                Accessibility.Private => "private",
                Accessibility.Protected => "protected",
                Accessibility.Internal => "internal",
                Accessibility.Public => "public",
                _ => string.Empty,
            };
        }
    }
}
