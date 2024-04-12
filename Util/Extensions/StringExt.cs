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


    public static string PartBefore(this string str, char marker, string whenNoMArker = "")
    {
        int p = str.IndexOf(marker);
        return p switch
               {
                   > 0 => str[..p],
                   0   => "",
                   < 0 => whenNoMArker
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


    public static string LastWord(this string str)
    {
        int n = str.Length;
        if (n == 0) return str;
        int k = n - 1;
        for (int i = n - 1; k > 0; k--)
            if (char.IsUpper(str[k])) return str[k..];
        return str;
    }
}