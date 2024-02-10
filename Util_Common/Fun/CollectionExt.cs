using System.Text;

namespace Util.Common.Fun;

public static class CollectionExt
{
    
    public static V? Get<K, V>(this IDictionary<K, V> dictionary, K key, V? missing = default(V))
    {
        bool ok = dictionary.TryGetValue(key, out var result);
        return ok ? result : missing;
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
