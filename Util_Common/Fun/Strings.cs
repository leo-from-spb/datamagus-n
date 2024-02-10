namespace Util.Common.Fun;

public static class Strings
{
    public static string? with(this string? str, char prefix = '\0', char suffix = '\0') =>
        str is null ? null :
        prefix != '\0' && suffix != '\0' ? $"{prefix}{str}{suffix}" :
        prefix != '\0' ? prefix + str :
        suffix != '\0' ? str + suffix :
        str;

    public static string? with(this string? str, string prefix = "", string suffix = "") =>
        str is not null ? $"{prefix}{str}{suffix}" : null;
}