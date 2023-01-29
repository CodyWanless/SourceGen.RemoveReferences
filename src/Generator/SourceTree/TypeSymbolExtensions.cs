using Microsoft.CodeAnalysis;

namespace Generator.SourceTree
{
    internal static class TypeSymbolExtensions
    {
        public static string GetFullNamespace(this ISymbol typeSymbol)
        {
            if (typeSymbol.ContainingNamespace?.Name is not { Length: > 0 })
            {
                return typeSymbol.Name;
            }

            var prefix = GetFullNamespace(typeSymbol.ContainingNamespace);
            if (prefix is { Length: > 0 })
            {
                return $"{prefix}.{typeSymbol.ContainingNamespace.Name}";
            }

            return typeSymbol.ContainingNamespace.Name;
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
