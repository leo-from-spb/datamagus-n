using System.Diagnostics.CodeAnalysis;

namespace Util.Common.Fun;

public static class EnglishFun
{

    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public static string plural(this string thing)
    {
        int n = thing.Length;
        if (n == 0) return thing;
        char c = thing[n - 1];
        return c switch
               {
                   'y' => thing[..(n-1)] + "ies",
                   's' => thing + "es",
                   _   => thing + "s"
               };
    }
    
}
