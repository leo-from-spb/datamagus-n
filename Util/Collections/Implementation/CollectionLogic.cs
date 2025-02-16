using System;
using System.Collections;
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


    internal static bool IsNotEmpty<E>(this IEnumerable<E> source)
    {
        if (source is IReadOnlyCollection<E> collection)
        {
            return collection.Count > 0;
        }
        else
        {
            using IEnumerator<E> enumerator = source.GetEnumerator();
            return enumerator.MoveNext();
        }
    }

    internal static bool IsEmpty<E>(this IEnumerable<E> source) => !IsNotEmpty(source);


    internal static int CountTrues(this BitArray bits)
    {
        int n = 0;
        foreach (bool b in bits)
            if (b) n++;
        return n;
    }


}
