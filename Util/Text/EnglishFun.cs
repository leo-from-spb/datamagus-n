using System.Diagnostics.CodeAnalysis;

namespace Util.Text;

public static class EnglishFun
{
    extension(string thing)
    {
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public string Plural
        {
            get
            {
                int n = thing.Length;
                if (n == 0) return thing;
                char z = thing[n - 1];
                char y = n >= 2 ? thing[n - 2] : '\0';
                return z switch
                       {
                           'y' => thing[..(n - 1)] + "ies",
                           's' => thing + "es",
                           'x' => thing + "es",
                           'h' => thing + (y == 'c' || y == 's' ? "es" : "s"),
                           _   => thing + 's'
                       };
            }
        }

    }
}
