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
    public static ImmList<E> ToImmList<E>(this E[] array) =>
        array.Length switch
        {
            0 => EmptySet<E>.Instance,
            1 => new ImmutableArrayList<E>(array.CloneArray()), // use a singleton set or list instead
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
                   1 => new ImmutableArrayList<E>(new[] { collection.First() }), // use a singleton set or list instead
                   _ => new ImmutableArrayList<E>(collection.ToArray())
               };
    }


    private static E[] CloneArray<E>(this E[] originalArray)
    {
        int n        = originalArray.Length;
        E[] newArray = new E[n];
        Array.Copy(originalArray, newArray, n);
        return newArray;
    }
}
