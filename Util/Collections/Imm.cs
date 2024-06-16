using System;
using System.Collections.Generic;
using System.Linq;

namespace Util.Collections;

public static class Imm
{

    /// <summary>
    /// Empty list and set.
    /// </summary>
    /// <typeparam name="T">element type</typeparam>
    /// <returns>the empty instance.</returns>
    public static ImmOrderSet<T> EmptySet<T>() => ConstEmptySet<T>.Instance;

    /// <summary>
    /// Empty list and sorted set.
    /// </summary>
    /// <typeparam name="T">element type</typeparam>
    /// <returns>the empty instance.</returns>
    public static ImmSortedSet<T> EmptySortedSet<T>() where T : IComparable<T> => ConstEmptySortedSet<T>.Instance;


    /// <summary>
    /// Returns a list of given elements, preserving the order.
    /// </summary>
    /// <param name="elements">elements.</param>
    /// <typeparam name="T">element type.</typeparam>
    /// <returns>list of given elements.</returns>
    public static ImmList<T> ListOf<T>(params T[] elements)
        => ToImmList(elements.AsSpan());


    /// <summary>
    /// Returns a list of given elements, preserving the order.
    /// </summary>
    /// <param name="elements">elements.</param>
    /// <typeparam name="T">element type.</typeparam>
    /// <returns>list of given elements.</returns>
    public static ImmList<T> ToImmList<T>(this Span<T> elements) =>
        elements.Length switch
        {
            0 => ConstEmptySet<T>.Instance,
            1 => new ConstSingletonSet<T>(elements[0]),
            _ => new ConstArrayList<T>(elements.ToArray(), false)
        };

    /// <summary>
    /// Returns a list of given elements, preserving the order.
    /// </summary>
    /// <param name="elements">elements.</param>
    /// <typeparam name="T">element type.</typeparam>
    /// <returns>list of given elements.</returns>
    public static ImmList<T> ToImmList<T>(this ReadOnlySpan<T> elements) =>
        elements.Length switch
        {
            0 => ConstEmptySet<T>.Instance,
            1 => new ConstSingletonSet<T>(elements[0]),
            _ => new ConstArrayList<T>(elements.ToArray(), false)
        };


    /// <summary>
    /// Sorts the given elements, remove duplicates, and return a sorted set.
    /// </summary>
    /// <param name="elements">elements.</param>
    /// <typeparam name="T">element type.</typeparam>
    /// <returns>sorted set of given elements.</returns>
    public static ImmSortedSet<T> SortedSetOf<T>(params T[] elements)
        where T : IComparable<T>
        => ToImmSortedSet(elements.AsSpan());

    /// <summary>
    /// Sorts the given elements, remove duplicates, and return a sorted set.
    /// </summary>
    /// <param name="elements">elements.</param>
    /// <typeparam name="T">element type.</typeparam>
    /// <returns>sorted set of given elements.</returns>
    public static ImmSortedSet<T> ToImmSortedSet<T>(this Span<T> elements)
        where T: IComparable<T>
    {
        int n = elements.Length;
        if (n == 0) return ConstEmptySortedSet<T>.Instance;
        if (n == 1) return new ConstSingletonSortedSet<T>(elements[0]);

        T[] array = elements.ToArray(); // copies elements into a new array
        return MakeSortedSet(array);
    }

    /// <summary>
    /// Sorts the given elements, remove duplicates, and return a sorted set.
    /// </summary>
    /// <param name="elements">elements.</param>
    /// <typeparam name="T">element type.</typeparam>
    /// <returns>sorted set of given elements.</returns>
    public static ImmSortedSet<T> ToImmSortedSet<T>(this ReadOnlySpan<T> elements)
        where T: IComparable<T>
    {
        int n = elements.Length;
        if (n == 0) return ConstEmptySortedSet<T>.Instance;
        if (n == 1) return new ConstSingletonSortedSet<T>(elements[0]);

        T[] array = elements.ToArray(); // copies elements into a new array
        return MakeSortedSet(array);
    }

    /// <summary>
    /// Collects the given elements into an array, sort them and remove duplicates,
    /// and return a sorted set.
    /// </summary>
    /// <param name="elements">elements.</param>
    /// <typeparam name="T">element type.</typeparam>
    /// <returns>sorted set of given elements.</returns>
    public static ImmSortedSet<T> ToImmSortedSet<T>(this IEnumerable<T> elements)
        where T : IComparable<T>
    {
        if (elements is ImmSortedSet<T> iss) return iss;
        T[] array = elements.ToArray();
        return MakeSortedSet(array);
    }

    private static ImmSortedSet<T> MakeSortedSet<T>(T[] array)
        where T : IComparable<T>
    {
        int n = array.Length;
        if (n == 0) return ConstEmptySortedSet<T>.Instance;
        if (n == 1) return new ConstSingletonSortedSet<T>(array[0]);
        ImmAlg.SortUnique(array, out n);
        if (n == 1) return new ConstSingletonSortedSet<T>(array[0]);
        return new ConstArraySortedSet<T>(array, 0, n, false);
    }










    /// <summary>
    /// Creates an immutables dictionary with entries from the given <paramref name="originalDictionary"/>, with a copy of all its entries.
    /// <br/>
    /// If the given <paramref name="originalDictionary"/> is already an instance of <see cref="ImmDictionary{K,V}"/>>, returns it as is.
    /// </summary>
    /// <param name="originalDictionary">the original dictionary.</param>
    /// <typeparam name="K">type of key.</typeparam>
    /// <typeparam name="V">type of value.</typeparam>
    /// <returns>the immutable dictionary.</returns>
    public static ImmDictionary<K, V> Dictionary<K, V>(IReadOnlyDictionary<K,V> originalDictionary)
    {
        if (originalDictionary is ImmDictionary<K, V> originalImmDictionary) return originalImmDictionary;

        int n = originalDictionary.Count;
        return n switch
               {
                   0    => ImmEmptyDictionary<K, V>.Empty,
                   <= 4 => new ImmMicroDictionary<K, V>(originalDictionary),
                   _    => new ImmCompactHashDictionary<K, V>(originalDictionary, n)
               };
    }

    /// <summary>
    /// Creates a dictionary of one key-value pair.
    /// </summary>
    public static ImmOrderArrayDictionary<K, V> Dictionary<K, V>(K key, V value)
    {
        var a = new KeyValuePair<K, V>[] { new(key, value) };
        return new ImmMicroDictionary<K, V>(a);
    }

    /// <summary>
    /// Creates a dictionary of two key-value pairs.
    /// </summary>
    public static ImmOrderArrayDictionary<K, V> Dictionary<K, V>(K key1, V value1, K key2, V value2)
    {
        var a = new KeyValuePair<K, V>[] { new(key1, value1), new(key2, value2) };
        return new ImmMicroDictionary<K, V>(a);
    }



    /// <summary>
    /// Creates an immutable snapshot of this dictionary.
    /// The original one remains unchanged.
    /// <br/>
    /// If this is already an instance of <see cref="ImmDictionary{K,V}"/>>, returns this as is.
    /// <br/>
    /// Otherwise, a new instance is created and all entries from the original dictionary are copied.
    /// In this case, the created instance doesn't reference the original one.
    /// </summary>
    /// <param name="originalDictionary">the original dictionary.</param>
    /// <typeparam name="K">type of key.</typeparam>
    /// <typeparam name="V">type of value.</typeparam>
    /// <returns>the immutable dictionary.</returns>
    public static ImmDictionary<K, V> ToImm<K, V>(this IReadOnlyDictionary<K, V> originalDictionary) => Dictionary(originalDictionary);


}
