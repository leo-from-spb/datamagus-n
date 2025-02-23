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
    public static ImmOrderedSet<E> SetOf<E>(params E[] elements) => elements.ToImmSet();

    /// <summary>
    /// Sortes and deduplicates elements and makes a set of them.
    /// </summary>
    /// <param name="elements"></param>
    /// <typeparam name="E"></typeparam>
    /// <returns>new sorted set.</returns>
    public static ImmSortedSet<E> SortedSetOf<E>(params E[] elements)
        where E : IComparable<E>
        => elements.ToImmSortedSet();


    /// <summary>
    /// Creates a snapshot of this array.
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
    /// Collects all elements from this <paramref name="source"/> and make an immutable list of them.
    /// </summary>
    /// <param name="source">the original collection.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>the immutable list.</returns>
    public static ImmList<E> ToImmList<E>(this IEnumerable<E> source)
    {
        if (source is IReadOnlyCollection<E> collection) return collection.ToImmList();
        E[] array = source.ToArray();
        return array.ToImmList();
    }


    /// <summary>
    /// Creates a snapshot of this collection.
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
    /// Collects and deduplicates all elements from this <paramref name="source"/> and creates an immutable set.
    /// </summary>
    /// <param name="source">elements.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>the created immutable set.</returns>
    public static ImmSet<E> ToImmSet<E>(this IEnumerable<E> source)
    {
        return source switch
               {
                   ImmSet<E> alreadyImmSet     => alreadyImmSet,
                   IReadOnlySet<E> set         => set.ToImmSet(),
                   ImmutableArrayList<E> immAL => DeduplicateAndPrepareSet(immAL.ShareElementsArray(), immAL.Count),
                   E[] array                   => DeduplicateAndPrepareSet(array, array.Length),
                   _                           => DeduplicateAndPrepareSet<E>(source, 0)
               };
    }


    /// <summary>
    /// Collects and deduplicates all elements from this <paramref name="source"/> and creates an immutable set.
    /// Preserves the original order.
    /// </summary>
    /// <param name="source">elements.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>the created immutable set.</returns>
    public static ImmOrderedSet<E> ToImmSet<E>(this IOrderedEnumerable<E> source)
    {
        return DeduplicateAndPrepareSet<E>(source, 0);
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

    private static ImmOrderedSet<E> DeduplicateAndPrepareSet<E>(IEnumerable<E> source, int initialCapacity)
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


    /// <summary>
    /// Deduplicates elements and creates a snapshot set of this <paramref name="collection"/>.
    /// </summary>
    /// <param name="collection">elements.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>the created immutable set.</returns>
    public static ImmSet<E> ToImmSet<E>(this IReadOnlyCollection<E> collection)
    {
        if (collection is ImmSet<E> immSet) return immSet;
        if (collection is IReadOnlySet<E> set) return set.ToImmSet();
        if (collection is IReadOnlyList<E> immList) immList.ToImmSet();

        int n = collection.Count;
        return DeduplicateAndPrepareSet(collection, n);
    }


    /// <summary>
    /// Makes an immutable snapshot of this <paramref name="set"/>.
    /// </summary>
    /// <param name="set">set of elements.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>the created immutable set.</returns>
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


    /// <summary>
    /// Deduplicates elements and creates a snapshot set of this <paramref name="list"/>.
    /// </summary>
    /// <param name="list">elements.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>the created immutable set.</returns>
    public static ImmOrderedSet<E> ToImmSet<E>(this IReadOnlyList<E> list)
    {
        if (list is ImmOrderedSet<E> immSet) return immSet;
        if (list is ImmutableArrayList<E> immArrayList) immArrayList.ToSet();

        int n = list.Count;
        if (n == 0) return EmptySet<E>.Instance;

        List<E> distinct = list.Deduplicate();
        int m = distinct.Count;
        if (m == 1) return new ImmutableSingleton<E>(distinct[0]);

        E[] newArray = distinct.ToArray();
        return m <= 3
            ? new ImmutableMiniSet<E>(newArray)
            : new ImmutableHashSet<E>(newArray);
    }


    /// <summary>
    /// Sorts and deduplicates elements and creates a snapshot set of this <paramref name="array"/>.
    /// </summary>
    /// <param name="array">elements.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>the created immutable sorted set.</returns>
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


    /// <summary>
    /// Sorts and deduplicates elements and creates a snapshot set of this <paramref name="collection"/>.
    /// </summary>
    /// <param name="collection">elements.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>the created immutable sorted set.</returns>
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


    /// <summary>
    /// Sorts elements and creates a snapshot of this <paramref name="set"/>.
    /// </summary>
    /// <param name="set">elements.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>the created immutable sorted set.</returns>
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


    /// <summary>
    /// Creates a snapshot of this <paramref name="sortedSet"/>.
    /// </summary>
    /// <param name="sortedSet">elements.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>the created immutable sorted set.</returns>
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


    /// <summary>
    /// Makes an immutables dictionary of one pair.
    /// </summary>
    /// <param name="key">the key.</param>
    /// <param name="value">the value.</param>
    /// <typeparam name="K">type of the key.</typeparam>
    /// <typeparam name="V">type of the value.</typeparam>
    /// <returns>the just created immutable dictionary.</returns>
    public static ImmDict<K,V> Dict<K,V>(K key, V value) =>
        new ImmutableSingletonDictionary<K,V>(key, value);

    /// <summary>
    /// Makes an immutables dictionary of a couple of pairs.
    /// </summary>
    /// <param name="key1">the key of the pair 1.</param>
    /// <param name="value1">the value of the pair 1.</param>
    /// <param name="key2">the key of the pair 2.</param>
    /// <param name="value2">the value of the pair 2.</param>
    /// <typeparam name="K">type of the key.</typeparam>
    /// <typeparam name="V">type of the value.</typeparam>
    /// <returns>the just created immutable dictionary.</returns>
    public static ImmDict<K,V> Dict<K,V>(K key1, V value1, K key2, V value2)
        where K : IEquatable<K>
        => EqualityComparer<K>.Default.Equals(key1, key2)
            ? new ImmutableSingletonDictionary<K,V>(key1, value1)
            : new ImmutableMiniDictionary<K,V>([new(key1, value1), new(key2, value2)]);

    /// <summary>
    /// Makes an immutables dictionary of three pairs.
    /// </summary>
    /// <param name="key1">the key of the pair 1.</param>
    /// <param name="value1">the value of the pair 1.</param>
    /// <param name="key2">the key of the pair 2.</param>
    /// <param name="value2">the value of the pair 2.</param>
    /// <param name="key3">the key of the pair 3.</param>
    /// <param name="value3">the value of the pair 3.</param>
    /// <typeparam name="K">type of the key.</typeparam>
    /// <typeparam name="V">type of the value.</typeparam>
    /// <returns>the just created immutable dictionary.</returns>
    public static ImmDict<K, V> Dict<K, V>(K key1, V value1, K key2, V value2, K key3, V value3)
        where K : IEquatable<K>
    {
        var eq = EqualityComparer<K>.Default;
        if (eq.Equals(key1, key2)) return Dict(key1, value1, key3, value3);
        if (eq.Equals(key1, key3)) return Dict(key1, value1, key2, value2);
        if (eq.Equals(key2, key3)) return Dict(key1, value1, key2, value2);

        KeyValuePair<K,V>[] pairs =
            new KeyValuePair<K,V>[]{ new(key1,value1), new(key2,value2), new(key3,value3) };
        return new ImmutableMiniDictionary<K,V>(pairs);
    }

    /// <summary>
    /// Makes an immutable snapshot of this dictionary.
    /// </summary>
    /// <param name="dictionary">the original dictionary.</param>
    /// <typeparam name="K">type of the key.</typeparam>
    /// <typeparam name="V">type of the value.</typeparam>
    /// <returns>the just created immutable snapshot.</returns>
    public static ImmDict<K,V> ToImmDict<K,V>(this IReadOnlyDictionary<K,V> dictionary)
    {
        if (dictionary is ImmDict<K,V> immDict) return immDict;

        int n = dictionary.Count;
        if (n == 0) return EmptyDictionary<K,V>.Instance;

        KeyValuePair<K,V>[] pairs = dictionary.ToArray();
        return n switch
               {
                   1    => new ImmutableSingletonDictionary<K,V>(pairs[0].Key, pairs[0].Value),
                   <= 4 => new ImmutableMiniDictionary<K,V>(pairs),
                   _    => new ImmutableHashDictionary<K,V>(pairs)
               };
    }

    /// <summary>
    /// Makes an immutable snapshot of this dictionary.
    /// </summary>
    /// <param name="dictionary">the original dictionary.</param>
    /// <typeparam name="K">type of the key.</typeparam>
    /// <typeparam name="V">type of the value.</typeparam>
    /// <returns>the just created immutable snapshot.</returns>
    public static ImmDict<K,V> ToImmDict<K,V>(this IDictionary<K,V> dictionary)
    {
        int n = dictionary.Count;
        if (n == 0) return EmptyDictionary<K,V>.Instance;

        KeyValuePair<K,V>[] pairs = dictionary.ToArray();
        return n switch
               {
                   1    => new ImmutableSingletonDictionary<K,V>(pairs[0].Key, pairs[0].Value),
                   <= 4 => new ImmutableMiniDictionary<K,V>(pairs),
                   _    => new ImmutableHashDictionary<K,V>(pairs)
               };
    }

    /// <summary>
    /// Makes an immutable snapshot of this dictionary.
    /// </summary>
    /// <param name="dictionary">the original dictionary.</param>
    /// <typeparam name="K">type of the key.</typeparam>
    /// <typeparam name="V">type of the value.</typeparam>
    /// <returns>the just created immutable snapshot.</returns>
    public static ImmDict<K,V> ToImmDict<K,V>(this Dictionary<K,V> dictionary)
    {
        int n = dictionary.Count;
        if (n == 0) return EmptyDictionary<K,V>.Instance;

        KeyValuePair<K,V>[] pairs = dictionary.ToArray();
        return n switch
               {
                   1    => new ImmutableSingletonDictionary<K,V>(pairs[0].Key, pairs[0].Value),
                   <= 4 => new ImmutableMiniDictionary<K,V>(pairs),
                   _    => new ImmutableHashDictionary<K,V>(pairs)
               };
    }



}
