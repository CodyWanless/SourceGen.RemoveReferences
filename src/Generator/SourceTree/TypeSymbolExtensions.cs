using Microsoft.CodeAnalysis;

namespace Generator.SourceTree
{
    internal static class TypeSymbolExtensions
    {
        public static string GetFullNamespace(this ISymbol typeSymbol)
        {
            if (typeSymbol.ContainingNamespace?.Name is not { Length: > 0 })
            {
                return string.Empty;
            }

            var prefix = GetFullNamespace(typeSymbol.ContainingNamespace);
            if (prefix is { Length: > 0 })
            {
                return $"{prefix}.{typeSymbol.ContainingNamespace.Name}";
            }

            return typeSymbol.ContainingNamespace.Name;
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
