using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Util.Collections;


/// <summary>
/// Immutable dictionary.
/// </summary>
/// <typeparam name="K">type of keys.</typeparam>
/// <typeparam name="V">type of associated values.</typeparam>
public interface ImmDict<K,V> : IReadOnlyDictionary<K,V>
{
    /// <summary>
    /// Inner implementation.
    /// </summary>
    internal ImmutableDictionary<K,V> Imp { get; }

    /// <summary>
    /// This dictionary has at least one entry.
    /// </summary>
    public bool Any { get; }

    /// <summary>
    /// This dictionary is empty.
    /// </summary>
    public bool IsEmpty { get; }

    /// <summary>
    /// Looks for an entry with the given key.
    /// </summary>
    /// <param name="key">the key to look for.</param>
    /// <returns>the result.</returns>
    public Found<V> Find(K key);

    /// <summary>
    /// Looks for an entry with the given key.
    /// </summary>
    /// <param name="key">the key to look for.</param>
    /// <param name="notFound">the value to return when the key is not found.</param>
    /// <returns>the result value if found or the specified value if not found.</returns>
    public V Find(K key, V notFound) =>
        #pragma warning disable CS8604
        Find(key) | notFound;
        #pragma warning restore CS8604

    bool IReadOnlyDictionary<K,V>.TryGetValue(K key, [MaybeNullWhen(false)] out V value)
    {
        var f = Find(key);
        value = f.Item;
        return f.Ok;
    }

    V IReadOnlyDictionary<K,V>.this[K key] => Find(key).Item;


    /// <summary>
    /// The set of dictionary keys.
    /// </summary>
    public new ImmSet<K> Keys { get; }

    /// <summary>
    /// The set of dictionary values.
    /// </summary>
    public new ImmCollection<V> Values { get; }

    /// <summary>
    /// The set of dictionary entries.
    /// </summary>
    public ImmCollection<KeyValuePair<K,V>> Entries { get; }


    IEnumerable<K> IReadOnlyDictionary<K,V>.Keys   => Keys;
    IEnumerable<V> IReadOnlyDictionary<K,V>.Values => Values;

    IEnumerator<KeyValuePair<K,V>> IEnumerable<KeyValuePair<K,V>>.GetEnumerator() => Entries.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Entries.GetEnumerator();
}



/// <summary>
/// Immutable dictionary, where entries have a meaningful order (but not always sorted).
/// </summary>
/// <typeparam name="K">type of keys.</typeparam>
/// <typeparam name="V">type of associated values.</typeparam>
public interface ImmOrdDict<K,V> : ImmDict<K,V>
{
    /// <summary>
    /// The first entry, or null if the dictionary is empty.
    /// </summary>
    public KeyValuePair<K,V> FirstEntry { get; }

    /// <summary>
    /// The last entry, or null if the dictionary is empty.
    /// </summary>
    public KeyValuePair<K,V> LastEntry { get; }

    /// <summary>
    /// The set of dictionary keys.
    /// </summary>
    public new ImmOrdSet<K> Keys { get; }

    ImmSet<K> ImmDict<K,V>.Keys => Keys;

    /// <summary>
    /// The set of dictionary values.
    /// </summary>
    public new ImmSeq<V> Values { get; }

    ImmCollection<V> ImmDict<K,V>.Values => Values;

    /// <summary>
    /// The set of dictionary entries.
    /// </summary>
    public new ImmSeq<KeyValuePair<K,V>> Entries { get; }

    ImmCollection<KeyValuePair<K,V>> ImmDict<K,V>.Entries => Entries;
}



/// <summary>
/// Immutable dictionary, where entries have a meaningful order (but not always sorted).
/// </summary>
/// <typeparam name="K">type of keys.</typeparam>
/// <typeparam name="V">type of associated values.</typeparam>
public interface ImmListDict<K,V> : ImmOrdDict<K,V>
{
    /// <summary>
    /// The set of dictionary keys.
    /// </summary>
    public new ImmListSet<K> Keys { get; }

    ImmOrdSet<K> ImmOrdDict<K,V>.Keys => Keys;
    ImmSet<K> ImmDict<K,V>.Keys => Keys;

    /// <summary>
    /// The set of dictionary values.
    /// </summary>
    public new ImmList<V> Values { get; }

    ImmSeq<V> ImmOrdDict<K,V>.Values => Values;
    ImmCollection<V> ImmDict<K,V>.Values => Values;

    /// <summary>
    /// The set of dictionary entries.
    /// </summary>
    public new ImmList<KeyValuePair<K,V>> Entries { get; }

    ImmSeq<KeyValuePair<K,V>> ImmOrdDict<K,V>.Entries => Entries;
    ImmCollection<KeyValuePair<K,V>> ImmDict<K,V>.Entries => Entries;
}



/// <summary>
/// Immutable dictionary where the entries are sorted by keys.
/// Doesn't preserve order when the original source is not sorted by the keys.
/// </summary>
/// <typeparam name="K">type of keys.</typeparam>
/// <typeparam name="V">type of associated values.</typeparam>
public interface ImmSortedDict<K,V> : ImmOrdDict<K,V>
    where K : IComparable<K>
{
    /// <summary>
    /// The minimal (first) key.
    /// </summary>
    public K MinKey { get; }

    /// <summary>
    /// The minimal (last) key.
    /// </summary>
    public K MaxKey { get; }
}
