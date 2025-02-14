using System;
using System.Collections.Generic;
using System.Linq;
using Util.Collections.Implementation;

namespace Util.Collections;


/// <summary>
/// Methods to make immutable collections.
/// </summary>
public static class Imm
{

    /// <summary>
    /// Create a snapshot of this array.
    /// </summary>
    /// <param name="array">the original array.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>the snapshot.</returns>
    public static ImmList<E> ToImmList<E>(this E[] array) =>
        array.Length switch
        {
            0 => EmptySet<E>.Instance,
            1 => new ImmutableSingleton<E>(array[0]),
            _ => new ImmutableArrayList<E>(array.CloneArray())
        };

    /// <summary>
    /// Create a snapshot of this collection.
    /// </summary>
    /// <param name="collection">the original collection.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>the snapshot.</returns>
    public static ImmList<E> ToImmList<E>(this IReadOnlyCollection<E> collection)
    {
        if (collection is ImmutableArrayList<E> ia) return ia;
        return collection.Count switch
               {
                   0 => EmptySet<E>.Instance,
                   1 => new ImmutableSingleton<E>(collection.First()),
                   _ => new ImmutableArrayList<E>(collection.ToArray())
               };
    }


    /// <summary>
    /// Deduplicates elements and creates a snapshot set of this array.
    /// </summary>
    /// <param name="array">elements.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>the created immutable set.</returns>
    public static ImmOrderedSet<E> ToImmSet<E>(this E[] array)
    {
        int n = array.Length;
        if (n == 0) return EmptySet<E>.Instance;
        if (n == 1) return new ImmutableSingleton<E>(array[0]);

        return DeduplicateAndPrepareSet<E>(array, n);
    }

    private static ImmOrderedSet<E> DeduplicateAndPrepareSet<E>(IEnumerable<E> source, int n)
    {
        var list = new List<E>(n);
        var hset = new HashSet<E>(n);

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


    public static ImmSet<E> ToImmSet<E>(this IReadOnlyCollection<E> collection)
    {
        if (collection is ImmSet<E> immSet) return immSet;
        if (collection is IReadOnlySet<E> set) return set.ToImmSet();
        if (collection is IReadOnlyList<E> immList) immList.ToImmSet();

        int n = collection.Count;
        return DeduplicateAndPrepareSet(collection, n);
    }


    public static ImmSet<E> ToImmSet<E>(this IReadOnlySet<E> set)
    {
        int n = set.Count;
        return n switch
               {
                   0    => EmptySet<E>.Instance,
                   1    => new ImmutableSingleton<E>(set.First()),
                   <= 3 => new ImmutableMiniSet<E>(set.ToArray()),
                   _    => new ImmutableHashSet<E>(set.ToArray())
               };
    }



    public static ImmOrderedSet<E> ToImmSet<E>(this IReadOnlyList<E> collection)
    {
        if (collection is ImmOrderedSet<E> immSet) return immSet;
        if (collection is ImmutableArrayList<E> immArrayList) immArrayList.ToSet();

        int n = collection.Count;
        if (n == 0) return EmptySet<E>.Instance;

        List<E> distinct = collection.Deduplicate();
        int m = distinct.Count;
        if (m == 1) return new ImmutableSingleton<E>(distinct[0]);

        E[] newArray = distinct.ToArray();
        return m <= 3
            ? new ImmutableMiniSet<E>(newArray)
            : new ImmutableHashSet<E>(newArray);
    }


    public static ImmSortedSet<E> ToImmSortedSet<E>(this E[] array)
        where E : IComparable<E>
    {
        int n = array.Length;
        if (n == 0) return EmptySortedSet<E>.Instance;
        if (n == 1) return new ImmutableSortedSet<E>(array);

        E[] newArray = new E[n];
        Array.Copy(array, newArray, n);

        return SortDeduplicateAndPrepareSet(newArray);
    }


    public static ImmSortedSet<E> ToImmSortedSet<E>(this IReadOnlyCollection<E> collection)
        where E : IComparable<E>
    {
        if (collection is ImmSortedSet<E> immSortedSet) return immSortedSet;
        if (collection is SortedSet<E> alreadySortedSet) return alreadySortedSet.ToImmSortedSet();
        if (collection is IReadOnlySet<E> alreadySet) return alreadySet.ToImmSortedSet();

        int n = collection.Count;
        if (n == 0) return EmptySortedSet<E>.Instance;
        if (n == 1) return new ImmutableSortedSingleton<E>(collection.First());

        E[] array = collection.ToArray();
        return SortDeduplicateAndPrepareSet(array);
    }


    public static ImmSortedSet<E> ToImmSortedSet<E>(this IReadOnlySet<E> set)
        where E : IComparable<E>
    {
        int n = set.Count;
        if (n == 0) return EmptySortedSet<E>.Instance;
        if (n == 1) return new ImmutableSortedSingleton<E>(set.First());

        E[] newArray = set.ToArray();
        Array.Sort(newArray);
        return new ImmutableSortedSet<E>(newArray);
    }


    public static ImmSortedSet<E> ToImmSortedSet<E>(this SortedSet<E> sortedSet)
        where E : IComparable<E>
    {
        return sortedSet.Count switch
               {
                   0 => EmptySortedSet<E>.Instance,
                   1 => new ImmutableSortedSingleton<E>(sortedSet.Min!),
                   _ => new ImmutableSortedSet<E>(sortedSet.ToArray())
               };
    }


    private static ImmSortedSet<E> SortDeduplicateAndPrepareSet<E>(E[] newArray)
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

}
