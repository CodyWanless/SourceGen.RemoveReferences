using System;
using System.Collections.Generic;
using System.Linq;
using Generator.SourceTree.Abstract;

namespace Generator.SourceTree.Rules
{
    internal class RuleSet : IRuleSet
    {
        private readonly IReadOnlyCollection<string> excludeNamespaces;

        public RuleSet(IReadOnlyCollection<string> excludeNamespaces)
        {
            this.excludeNamespaces = excludeNamespaces;
        }

        public bool IsAllowedNamespace(string @namespace)
        {
            foreach (var excludeNamespace in this.excludeNamespaces)
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
