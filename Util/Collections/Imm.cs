using System;
using System.Collections.Generic;
using System.Linq;

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
    public static ImmList<E> ToImmList<E>(this E[] array) => new ImmList<E>(array, true);

    /// <summary>
    /// Create a snapshot of this collection.
    /// </summary>
    /// <param name="collection">the original collection.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>the snapshot.</returns>
    public static ImmList<E> ToImmList<E>(this IReadOnlyCollection<E> collection) => new ImmList<E>(collection.ToArray(), false);

    /// <summary>
    /// Creates an immutable set of the given set, preserving the original order.
    /// <br/>
    /// The original set must use the default equality comparer, otherwise the behavior of the set is not predictable.
    /// </summary>
    /// <param name="set">the original set.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>new set.</returns>
    public static ImmSet<E> ToImmSet<E>(this IReadOnlySet<E> set)
    {
        int n = set.Count;
        E[] array = set.ToArray();
        return n <= 3 ? new ImmMiniSet<E>(array, false) : new ImmHashSet<E>(array, false);
    }

    /// <summary>
    /// Creates an immutable set of elements from the given collection,
    /// after deduplicated the elements.
    /// The order is preserved. When several equal elements are given in the original collection,
    /// the first one is used.
    /// </summary>
    /// <param name="collection">source elements.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>the created immutable set.</returns>
    public static ImmSet<E> ToImmSet<E>(this IReadOnlyCollection<E> collection)
    {
        int n = collection.Count;
        if (n == 0) return ImmZeroSet<E>.Instance;
        if (n == 1) return new ImmMiniSet<E>([collection.First()], false);

        var list = new List<E>(n);
        var hset  = new HashSet<E>(n);

        // deduplicate elements
        foreach (E element in collection)
        {
            if (hset.Add(element))
                list.Add(element);
        }

        E[] array = list.ToArray();
        return array.Length <= 3
            ? new ImmMiniSet<E>(array, false)
            : new ImmHashSet<E>(array, false);
    }


    /// <summary>
    /// Creates an immutable copy of this sorted set..
    /// </summary>
    /// <param name="sourceSet">source set of elements.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>the created immutable sorted set.</returns>
    public static ImmSortSet<E> ToImmSortSet<E>(this SortedSet<E> sourceSet)
        where E : IComparable<E>
    {
        if (sourceSet.Count == 0) return ImmSortSet<E>.Empty;

        E[] array = sourceSet.ToArray();
        return new ImmSortSet<E>(array, false);
    }

    /// <summary>
    /// Creates an immutable sorted set of elements from the given collection.
    /// Elements are sorted and deduplicated, so the order is NOT preserved.
    /// </summary>
    /// <param name="collection">source elements.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>the created immutable sorted set.</returns>
    public static ImmSortSet<E> ToImmSortSet<E>(this IReadOnlyCollection<E> collection)
        where E : IComparable<E>
    {
        if (collection.Count == 0) return ImmSortSet<E>.Empty;
        if (collection is ImmSortSet<E> iss) return iss;
        if (collection is SortedSet<E> ss) return ss.ToImmSortSet();

        ArraySegment<E> a = collection.SortAndDeduplicate();
        if (a.Count == 0) return ImmSortSet<E>.Empty;
        return new ImmSortSet<E>(a);
    }

    /// <summary>
    /// Creates an immutable sorted set of elements from the given collection.
    /// Elements are sorted and deduplicated, so the order is NOT preserved.
    /// </summary>
    /// <param name="elements">source elements.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>the created immutable sorted set.</returns>
    public static ImmSortSet<E> ToImmSortSet<E>(this IEnumerable<E> elements)
        where E : IComparable<E>
    {
        if (elements is IReadOnlyCollection<E> collection) return ToImmSortSet<E>(collection);

        ArraySegment<E> a = elements.SortAndDeduplicate();
        if (a.Count == 0) return ImmSortSet<E>.Empty;
        return new ImmSortSet<E>(a);
    }

}
