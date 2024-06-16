using System;
using System.Collections.Generic;
using System.Linq;

namespace Util.Collections;

public static class Imm
{

    public static ConstArraySortedSet<T> ToImmSortedSet<T>(IEnumerable<T> elements)
        where T : IComparable<T>
    {
        T[] array = elements.ToArray();
        ImmAlg.SortUnique(array, out int n);
        if (n < array.Length / 2)
            Array.Resize(ref array, n);
        return new ConstArraySortedSet<T>(array, 0, n, false);
    }


    /// <summary>
    /// Empty list and set.
    /// </summary>
    /// <typeparam name="T">element type</typeparam>
    /// <returns>the empty instance.</returns>
    public static ROrderSet<T> EmptySet<T>() => ConstEmptySet<T>.Instance;

    /// <summary>
    /// Empty list and sorted set.
    /// </summary>
    /// <typeparam name="T">element type</typeparam>
    /// <returns>the empty instance.</returns>
    public static RSortedSet<T> EmptySortedSet<T>() where T : IComparable<T> => ConstEmptySortedSet<T>.Instance;



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
