using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Combination.DynamicCors
{
    internal sealed class DynamicCorsFactory : IDynamicCorsFactory
    {
        private static readonly IReadOnlyCollection<string> DefaultMethods = new[] { "GET", "POST" }.ToImmutableArray();

        private static readonly IReadOnlyCollection<string> DefaultHeaders = new [] {"Content-Type", "Content-Length" }.ToImmutableArray();

        internal IReadOnlyCollection<string> Methods { get; private set; } = DefaultMethods;

        internal IReadOnlyCollection<string> Headers { get; private set; } = DefaultHeaders;

        internal IReadOnlyCollection<string> ExposedHeaders { get; private set; } = Array.Empty<string>();

        internal Regex? Pattern { get; private set; }

        public IDynamicCorsFactory WithMethods(params string[] methods)
        {
            Methods = methods.ToImmutableArray();
            return this;
        }

        public IDynamicCorsFactory WithMethods(IReadOnlyCollection<string> methods)
        {
            Methods = methods.ToImmutableArray();
            return this;
        }

        public IDynamicCorsFactory WithHeaders(params string[] headers)
        {
            Headers = headers.ToImmutableArray();
            return this;
        }

        public IDynamicCorsFactory WithExposedHeaders(params string[] headers)
        {
            ExposedHeaders = headers.ToImmutableArray();
            return this;
        }

        public IDynamicCorsFactory WithHeaders(IReadOnlyCollection<string> headers)
        {
            Headers = headers.ToImmutableArray();
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
