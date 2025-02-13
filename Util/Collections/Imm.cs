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
    public static ImmSet<E> ToImmSet<E>(this E[] array)
    {
        int n = array.Length;
        if (n == 0) return EmptySet<E>.Instance;
        if (n == 1) return new ImmutableSingleton<E>(array[0]);

        var list = new List<E>(n);
        var hset = new HashSet<E>(n);

        // deduplicate elements
        foreach (E element in array)
        {
            if (hset.Add(element))
                list.Add(element);
        }

        int m = list.Count;
        if (m == 1) return new ImmutableSingleton<E>(array[0]);

        E[] newArray = list.ToArray();
        return m <= 3
            ? new ImmutableMiniSet<E>(newArray)
            : new ImmutableHashSet<E>(newArray);
    }


    public static ImmOrderedSet<E> ToImmSet<E>(this IReadOnlyCollection<E> collection)
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

}
