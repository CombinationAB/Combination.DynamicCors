using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Combination.DynamicCors
{
    internal sealed class DynamicCorsFactory : IDynamicCorsFactory
    {
        private static readonly IReadOnlyCollection<string> DefaultMethods = new[] { "GET", "POST" }.ToImmutableArray();

        internal IReadOnlyCollection<string> Methods { get; private set; } = DefaultMethods;

        internal Regex? Pattern { get; private set; }

        public IDynamicCorsFactory WithMethods(params string[] methods)
        {
            Methods = methods.ToImmutableArray();
            return this;
        }

        public IDynamicCorsFactory WithPattern(Regex regex)
        {
            Pattern = regex;
            return this;
        }

        public IDynamicCorsFactory WithPattern(string regex)
            => WithPattern(new Regex(regex));
    }
}
