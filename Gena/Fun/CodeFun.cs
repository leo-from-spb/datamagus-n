namespace Gena.Fun;


/// <summary>
/// Utility functions.
/// </summary>
public static class CodeFun
{

    public static string? Wrap(this string? str, string? prefix = null, string? suffix = null)
    {
        if (str is null || str.Length == 0) return null;
        return prefix + str + suffix;
    }

}
