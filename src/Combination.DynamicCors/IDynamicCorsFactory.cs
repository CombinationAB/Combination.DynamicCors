using System.Text.RegularExpressions;

namespace Combination.DynamicCors
{
    public interface IDynamicCorsFactory
    {
        IDynamicCorsFactory WithMethods(params string[] methods);

        IDynamicCorsFactory WithPattern(Regex regex);

        IDynamicCorsFactory WithPattern(string regex);

        IDynamicCorsFactory WithHeaders(string[] headers);
    }
}
