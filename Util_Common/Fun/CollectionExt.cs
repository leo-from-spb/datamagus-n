using System.Text;

namespace Util.Common.Fun;


/// <summary>
/// Utility functions for working with collections.
/// </summary>
public static class CollectionExt
{
    
    public static void Into<T>(this IEnumerable<T> sequence, List<T> list)
    {
        list.AddRange(sequence);
    }
    

    public static string JoinToString(this IEnumerable<string> strings,
                                      string separator = ", ",
                                      string prefix    = "",
                                      string suffix    = "",
                                      string empty     = "")
    {
        var b     = new StringBuilder();
        var begin = true;
        foreach (var s in strings)
        {
            b.Append(begin ? prefix : separator);
            b.Append(s);
            begin = false;
        }

        b.Append(begin ? empty : suffix);
        return b.ToString();
    }

    public static string JoinToString<T>(this IEnumerable<T> items,
                                         Func<T, string?>    func,
                                         string              separator = ", ",
                                         string              prefix    = "",
                                         string              suffix    = "",
                                         string              empty     = "")
    {
        var b     = new StringBuilder();
        var begin = true;
        foreach (var item in items)
        {
            var s = func(item);
            if (s is null) continue;
            b.Append(begin ? prefix : separator);
            b.Append(s);
            begin = false;
        }

        b.Append(begin ? empty : suffix);
        return b.ToString();
    }
    
}
