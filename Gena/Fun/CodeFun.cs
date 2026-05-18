using Util.Extensions;

namespace Gena.Fun;


/// <summary>
/// Utility functions.
/// </summary>
public static class CodeFun
{

    extension(string? str)
    {
        public string? Wrap(string? prefix = null, string? suffix = null)
        {
            if (str is null || str.Length == 0) return null;
            return prefix + str + suffix;
        }


        public bool IsCapitalized => char.IsUpper(str.FirstChar());
    }


}
