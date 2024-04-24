using System.Diagnostics.CodeAnalysis;

namespace Util.Extensions;

public static class StringExt
{
    public static string? With(this string? str, char prefix = '\0', char suffix = '\0') =>
        str is null ? null :
        prefix != '\0' && suffix != '\0' ? $"{prefix}{str}{suffix}" :
        prefix != '\0' ? prefix + str :
        suffix != '\0' ? str + suffix :
        str;

    public static string? With(this string? str, string prefix = "", string suffix = "") =>
        str is not null ? $"{prefix}{str}{suffix}" : null;


    public static string PartBefore(this string str, char marker, string whenNoMarker = "")
    {
        int p = str.IndexOf(marker);
        return p switch
               {
                   > 0 => str[..p],
                   0   => "",
                   < 0 => whenNoMarker
               };
    }
    

    public static string Decap(this string str)
    {
        int n = str.Length;
        if (n == 0) return str;
        char c1 = str[0];
        char cL = char.ToLower(c1); 
        if (cL == c1) return str;
        return cL + str[1..];
    }


    public static char FirstChar(this string? str, char empty = '\0') => str is not null && str.Length >= 1 ? str[0] : empty;

    public static char LastChar(this string? str, char empty = '\0')
    {
        int n = str is not null ? str.Length : 0;
        return n > 0 ? str![n - 1] : empty;
    }


    public static string LastWord(this string str)
    {
        int n = str.Length;
        if (n == 0) return str;
        int k = n - 1;
        for (int i = n - 1; k > 0; k--)
            if (char.IsUpper(str[k])) return str[k..];
        return str;
    }


    /// <summary>
    /// Checks whether this substring is contained in the given string.
    /// </summary>
    /// <param name="substr">the substring to check.</param>
    /// <param name="str">the string where to find the substring.</param>
    public static bool IsIn(this string substr, [NotNullWhen(true)] string? str) => str is not null && str.Contains(substr);

    /// <summary>
    /// Check whether this string is not empty (and not null).
    /// </summary>
    /// <param name="str">the string to check.</param>
    /// <returns>true, when not null and not empty.</returns>
    public static bool IsNotEmpty([NotNullWhen(true)] this string? str) => str is not null && str.Length > 0;

    /// <summary>
    /// Check whether this string is empty or null.
    /// </summary>
    /// <param name="str">the string to check.</param>
    /// <returns>true, when empty or null.</returns>
    public static bool IsEmpty([NotNullWhen(false)] this string? str) => string.IsNullOrEmpty(str);

    /// <summary>
    /// If this string is not empty returns this string, otherwise null.
    /// </summary>
    public static string? Nullize(this string? str) => str is not null && str.Length > 0 ? str : null;

    /// <summary>
    /// Trims and nullizes.
    /// If this string is not blank returns this string, otherwise null.
    /// </summary>
    public static string? TrimNullize(this string? str)
    {
        if (str is null) return null;
        string s = str.Trim();
        return s.Length > 0 ? s : null;
    }


}