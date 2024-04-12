namespace Util.Fun;

public static class EnglishFun
{

    public static string Plural(this string thing)
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
