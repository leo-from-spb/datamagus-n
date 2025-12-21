using System;
using System.Collections.Generic;
using Util.Collections.Implementation;

namespace Util.Collections;


/// <summary>
/// Methods to make immutable collections.
/// </summary>
public static class Imm
{

    /// <summary>
    /// Makes a list with the given elements.
    /// </summary>
    /// <param name="elements"></param>
    /// <typeparam name="E"></typeparam>
    /// <returns>new list.</returns>
    public static ImmList<E> ListOf<E>(params E[] elements) => elements.ToImmList();

    /// <summary>
    /// Deduplicates elements and makes a set of them.
    /// Preserves the original order.
    /// </summary>
    /// <param name="elements"></param>
    /// <typeparam name="E"></typeparam>
    /// <returns>new set.</returns>
    public static ImmListSet<E> SetOf<E>(params E[] elements) => elements.ToImmSet();

    /// <summary>
    /// Sortes and deduplicates elements and makes a set of them.
    /// </summary>
    /// <param name="elements"></param>
    /// <typeparam name="E"></typeparam>
    /// <returns>new sorted set.</returns>
    public static ImmSortedListSet<E> SortedSetOf<E>(params E[] elements)
        where E : IComparable<E>
        => elements.ToImmSortedSet();


    /// <summary>
    /// Makes an immutables dictionary of one pair.
    /// </summary>
    /// <param name="key">the key.</param>
    /// <param name="value">the value.</param>
    /// <typeparam name="K">type of the key.</typeparam>
    /// <typeparam name="V">type of the value.</typeparam>
    /// <returns>the just created immutable dictionary.</returns>
    public static ImmListDict<K,V> DictOf<K,V>(K key, V value) =>
        new ImmutableSingletonDictionary<K,V>(key, value);

    /// <summary>
    /// Makes an immutables dictionary of a couple of pairs.
    /// </summary>
    /// <param name="key1">the key of pair 1.</param>
    /// <param name="value1">the value of pair 1.</param>
    /// <param name="key2">the key of pair 2.</param>
    /// <param name="value2">the value of pair 2.</param>
    /// <typeparam name="K">type of the key.</typeparam>
    /// <typeparam name="V">type of the value.</typeparam>
    /// <returns>the just created immutable dictionary.</returns>
    public static ImmListDict<K,V> DictOf<K,V>(K key1, V value1, K key2, V value2)
        where K : IEquatable<K>
        => EqualityComparer<K>.Default.Equals(key1, key2)
            ? new ImmutableSingletonDictionary<K,V>(key1, value1)
            : new ImmutableMiniDictionary<K,V>([new(key1, value1), new(key2, value2)]);

    /// <summary>
    /// Makes an immutables dictionary of three pairs.
    /// </summary>
    /// <param name="key1">the key of pair 1.</param>
    /// <param name="value1">the value of pair 1.</param>
    /// <param name="key2">the key of pair 2.</param>
    /// <param name="value2">the value of pair 2.</param>
    /// <param name="key3">the key of pair 3.</param>
    /// <param name="value3">the value of pair 3.</param>
    /// <typeparam name="K">type of the key.</typeparam>
    /// <typeparam name="V">type of the value.</typeparam>
    /// <returns>the just created immutable dictionary.</returns>
    public static ImmListDict<K,V> DictOf<K,V>(K key1, V value1, K key2, V value2, K key3, V value3)
        where K : IEquatable<K>
    {
        var eq = EqualityComparer<K>.Default;
        if (eq.Equals(key1, key2)) return DictOf(key1, value1, key3, value3);
        if (eq.Equals(key1, key3)) return DictOf(key1, value1, key2, value2);
        if (eq.Equals(key2, key3)) return DictOf(key1, value1, key2, value2);

        KeyValuePair<K,V>[] pairs =
            new KeyValuePair<K,V>[]{ new(key1,value1), new(key2,value2), new(key3,value3) };
        return new ImmutableMiniDictionary<K,V>(pairs);
    }



    /**
     * Allows to use <see cref="ImmList"/> and <see cref="ImmCollection"/> with a collection expression.
     */
    public static ImmList<E> CreateImmList<E>(ReadOnlySpan<E> elements)
        => elements.ToImmList();

    /**
     * Allows to use <see cref="ImmSet"/> and <see cref="ImmListSet"/> with a collection expression.
     */
    public static ImmListSet<E> CreateImmListSet<E>(ReadOnlySpan<E> elements)
        => elements.ToImmSet();

    /**
     * Allows to use <see cref="ImmSortedSet"/> and <see cref="ImmSortedListSet"/> with a collection expression.
     */
    public static ImmSortedListSet<E> CreateImmSortedListSet<E>(ReadOnlySpan<E> elements)
        where E : IComparable<E>
        => elements.ToImmSortedSet();
}
