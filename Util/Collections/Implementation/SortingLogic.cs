using System;
using System.Collections.Generic;
using System.Linq;
using Util.Structures;

namespace Util.Collections.Implementation;


internal static class SortingLogic
{

    internal static ImmListSet<E> DeduplicateAndPrepareSet<E>(IEnumerable<E> source, int initialCapacity)
    {
        var list = new List<E>(initialCapacity);
        var hset = new HashSet<E>(initialCapacity);

        // deduplicate elements
        foreach (E element in source)
        {
            if (hset.Add(element))
                list.Add(element);
        }

        int m = list.Count;
        if (m == 1) return new ImmutableSingleton<E>(list[0]);

        E[] newArray = list.ToArray();
        return m <= 3
            ? new ImmutableMiniSet<E>(newArray)
            : new ImmutableHashSet<E>(newArray);
    }


    internal static ImmListSet<E> DeduplicateAndPrepareSet<E>(ReadOnlySpan<E> source, int initialCapacity)
    {
        var list = new List<E>(initialCapacity);
        var hset = new HashSet<E>(initialCapacity);

        // deduplicate elements
        foreach (E element in source)
        {
            if (hset.Add(element))
                list.Add(element);
        }

        int m = list.Count;
        if (m == 1) return new ImmutableSingleton<E>(list[0]);

        E[] newArray = list.ToArray();
        return m <= 3
            ? new ImmutableMiniSet<E>(newArray)
            : new ImmutableHashSet<E>(newArray);
    }


    internal static ImmSortedListSet<E> SortDeduplicateAndPrepareSet<E>(E[] newArray)
        where E : IComparable<E>
    {
        int n = newArray.Length;
        newArray.SortAndDeduplicate(out int newCount);

        if (newCount == 1) return new ImmutableSortedSingleton<E>(newArray[0]);

        if (newCount < n)
        {
            Array.Resize(ref newArray, newCount);
        }

        return new ImmutableSortedSet<E>(newArray);
    }


    internal static E[] SortAndDeduplicate<E>(this IEnumerable<E> collection)
        where E : IComparable<E>
    {
        E[] array = collection.ToArray();
        int n = array.Length;
        array.SortAndDeduplicate(out int m);
        if (m == n) return array;
        Array.Resize(ref array, m);
        return array;
    }


    /// <summary>
    /// Sorts and deduplicates elements inside this array.
    /// </summary>
    /// <param name="array">array with elements to sort and deduplicates.</param>
    /// <param name="newCount">new (possible smaller) count of elements.</param>
    /// <typeparam name="E"></typeparam>
    internal static void SortAndDeduplicate<E>(this E[] array, out int newCount)
        where E : IComparable<E>
    {
        int n = array.Length;
        newCount = n;
        if (n <= 1) return;

        Array.Sort(array);

        // check for duplicates
        var comparer = Comparer<E>.Default;
        int k = 1;
        while (k < n && comparer.Compare(array[k-1], array[k]) < 0) k++;
        if (k == n) return; // no duplicates

        // k points to the first duplicate
        // remove duplicates
        int m = k; // m will be the number of unique elements
        k++;
        while (k < n)
        {
            while (comparer.Compare(array[m - 1], array[k]) >= 0) k++;
            array[m] = array[k];
            m++;
            k++;
        }

        newCount = m;
    }


    internal static ImmSortedDict<K,V> MakeImmSortedDict<K,V>(KeyValuePair<K,V>[] pairs)
        where K : IComparable<K>
    {
        if (pairs.Length == 1) return new ImmutableSingletonSortedDictionary<K,V>(pairs[0]);

        var comparer = Comparer<K>.Default;
        Array.Sort(pairs, (x,y) => comparer.Compare(x.Key, y.Key));
        return new ImmutableSortedDictionary<K,V>(pairs);
    }


    internal static ImmSortedDict<uint,V> MakeImmSortedDict<V>(IEnumerable<KeyValuePair<uint,V>> pairs, int n)
    {
        if (n == 0) return EmptySortedDictionary<uint,V>.Instance;
        if (n == 1) return new ImmutableSingletonSortedDictionary<uint,V>(pairs.First());

        KeyValuePair<uint,V>[] array = pairs.ToArray();

        if (n > 2)
        {
            var interval = ImmutableFlatDictionary<V>.CollectInterval(array);
            if (interval.Length() <= n * 5) return new ImmutableFlatDictionary<V>(interval, array);
        }

        Array.Sort(array, (x,y) => Comparer<uint>.Default.Compare(x.Key, y.Key));
        return new ImmutableSortedDictionary<uint,V>(array);
    }
}
