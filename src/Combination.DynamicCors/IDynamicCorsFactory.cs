using System.Text.RegularExpressions;

namespace Combination.DynamicCors
{
    public interface IDynamicCorsFactory
    {
        //Test
        IDynamicCorsFactory WithMethods(params string[] methods);

        IDynamicCorsFactory WithPattern(Regex regex);

        IDynamicCorsFactory WithPattern(string regex);

        IDynamicCorsFactory WithHeaders(string[] headers);
    }
}
