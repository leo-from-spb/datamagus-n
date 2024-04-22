using System;
using System.Collections.Generic;
using System.Text;

namespace Util.Extensions;


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
                                      string                   separator = ", ",
                                      string                   prefix    = "",
                                      string                   suffix    = "",
                                      string                   empty     = "") =>
        strings.JoinToString(func: s => s,
                             separator: separator,
                             prefix: prefix,
                             suffix: suffix,
                             empty: empty );


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

    /// <summary>
    /// Checks whether this collection is empty.
    /// </summary>
    /// <param name="collection">the collection to check.</param>
    /// <returns>true when empty.</returns>
    public static bool IsEmpty<E>(this IReadOnlyCollection<E> collection) => collection.Count == 0;

    /// <summary>
    /// Checks whether this collection contains something (is not empty).
    /// </summary>
    /// <param name="collection">the collection to check.</param>
    /// <returns>true when it is not empty.</returns>
    public static bool IsNotEmpty<E>(this IReadOnlyCollection<E> collection) => collection.Count > 0;

}
