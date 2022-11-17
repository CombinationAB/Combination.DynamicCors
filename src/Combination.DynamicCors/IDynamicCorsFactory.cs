using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Combination.DynamicCors
{
    public interface IDynamicCorsFactory
    {
        IDynamicCorsFactory WithMethods(params string[] methods);

        IDynamicCorsFactory WithMethods(IReadOnlyCollection<string> methods);

        IDynamicCorsFactory WithPattern(Regex regex);

        IDynamicCorsFactory WithPattern(string regex);

        IDynamicCorsFactory WithHeaders(params string[] headers);

        IDynamicCorsFactory WithExposedHeaders(params string[] headers);

        IDynamicCorsFactory WithHeaders(IReadOnlyCollection<string> headers);
    }
}
