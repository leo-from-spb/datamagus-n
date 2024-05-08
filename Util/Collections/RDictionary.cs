using System;
using System.Collections.Generic;

namespace Util.Collections;

/// <summary>
/// Readable dictionary.
/// <br/>
/// It can be an immutable dictionary, or an interface to a mutable one.
/// </summary>
/// <typeparam name="K">type of key.</typeparam>
/// <typeparam name="V">type of value.</typeparam>
public interface RDictionary<K,V> : IReadOnlyDictionary<K,V>
{

    /// <summary>
    /// Finds the value associated with the given key.
    /// </summary>
    /// <param name="key">key to find.</param>
    /// <returns>found (or not found) value.</returns>
    public Found<V> Find(K key);

    /// <summary>
    /// Finds the value associated with the given key.
    /// </summary>
    /// <param name="key">key to find.</param>
    /// <param name="noValue">what to return when the key not found.</param>
    /// <returns>found value, or the <paramref name="noValue"/> when not found.</returns>
    #nullable disable
    public V Get(K key, V noValue = default(V));
    #nullable restore

    /// <summary>
    /// Finds the value associated with the given key.
    /// </summary>
    /// <param name="key">the found value, or the type-default value when not found.</param>
    public new V this[K key] { get; }

    /// <summary>
    /// Check whether this dictionary contains the given key.
    /// </summary>
    /// <param name="key">key to find.</param>
    /// <returns>whether the given key found.</returns>
    public new bool ContainsKey(K key);

    /// <summary>
    /// Number of pairs in the dictionary.
    /// </summary>
    public new int Count { get; }

    /// <summary>
    /// Whether this dictionary has at least one pair.
    /// </summary>
    public bool IsNotEmpty { get; }

    /// <summary>
    /// Whether this dictionary is empty.
    /// </summary>
    public bool IsEmpty { get; }

    /// <summary>
    /// Set of keys.
    /// </summary>
    public new IReadOnlyCollection<K> Keys { get; }

    /// <summary>
    /// Collection of values.
    /// </summary>
    public new IReadOnlyCollection<V> Values { get; }

    /// <summary>
    /// All key-value pairs as en IEnumerable.
    /// </summary>
    public IReadOnlyCollection<KeyValuePair<K,V>> Entries { get; }


}


/// <summary>
/// Readable dictionary, which key-value pairs are ordered in some way
/// (may be not sorted, but ordered in some other way).
/// </summary>
/// <typeparam name="K">type of key.</typeparam>
/// <typeparam name="V">type of value.</typeparam>
public interface ROrderDictionary<K,V> : RDictionary<K,V>, IReadOnlyList<KeyValuePair<K,V>>
{
    /// <summary>
    /// Returns the key-value pair by its index.
    /// </summary>
    /// <param name="index">index, started with 0.</param>
    /// <returns>the pair.</returns>
    /// <exception cref="IndexOutOfRangeException">when the index is invalid.</exception>
    public KeyValuePair<K,V> At(int index);

    /// <summary>
    /// Returns the key-value pair by its index.
    /// </summary>
    /// <param name="index">index, started with 0.</param>
    /// <returns>the pair.</returns>
    /// <exception cref="IndexOutOfRangeException">when the index is invalid.</exception>
    public new KeyValuePair<K,V> this [int index] => At(index);

    KeyValuePair<K, V> IReadOnlyList<KeyValuePair<K, V>>.this[int index] => At(index);
}
