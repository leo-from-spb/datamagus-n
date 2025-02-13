using System;
using System.Collections.Generic;

namespace Util.Collections.Implementation;


internal static class CollectionLogic
{

    internal static E[] CloneArray<E>(this E[] originalArray)
    {
        int n        = originalArray.Length;
        E[] newArray = new E[n];
        Array.Copy(originalArray, newArray, n);
        return newArray;
    }

    internal static List<E> Deduplicate<E>(this IEnumerable<E> source)
    {
        int n    = source is IReadOnlyCollection<E> sourceCollection ? sourceCollection.Count : 16;
        var list = new List<E>(n);
        var hset = new HashSet<E>(n);

        foreach (E element in source)
        {
            if (hset.Add(element))
                list.Add(element);
        }

        return list;
    }



}
