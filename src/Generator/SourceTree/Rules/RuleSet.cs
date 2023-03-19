using System;
using System.Linq;
using Generator.SourceTree.Abstract;

namespace Generator.SourceTree.Rules
{
    internal class RuleSet : IRuleSet
    {
        private static readonly string[] ExcludeNamespaces =
        {
            "ServiceStack",
        };

        public bool IsAllowedNamespace(string @namespace)
        {
            foreach (var excludeNamespace in ExcludeNamespaces)
            {
                if (@namespace.IndexOf(excludeNamespace, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return false;
                }
            }

            return true;
        }

        // TODO: Should be configurable
        public bool IsAllowedType(ISourceGeneratorNode sourceGeneratorNode)
        {
            return sourceGeneratorNode.RequiredNamespaces
                .All(n => this.IsAllowedNamespace(n));
        }
    }
}
