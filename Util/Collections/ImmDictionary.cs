using System.Collections;
using System.Collections.Generic;

namespace Util.Collections;


/// <summary>
/// Read-only immutable dictionary.
/// </summary>
/// <typeparam name="K">type of key.</typeparam>
/// <typeparam name="V">type of value.</typeparam>
public abstract class ImmDictionary<K,V> : IReadOnlyDictionary<K,V>
{
    /// <summary>
    /// The Key comparer.
    /// </summary>
    protected static readonly EqualityComparer<K> comparer = EqualityComparer<K>.Default;

    /// <summary>
    /// Finds the value associated with the given key.
    /// </summary>
    /// <param name="key">key to find.</param>
    /// <returns>found (or not found) value.</returns>
    public abstract Found<V> Find(K key);

    /// <summary>
    /// Finds the value associated with the given key.
    /// </summary>
    /// <param name="key">key to find.</param>
    /// <param name="noValue">what to return when the key not found.</param>
    /// <returns>found value, or the <paramref name="noValue"/> when not found.</returns>
    #nullable disable
    public abstract V Get(K key, V noValue = default(V));
    #nullable restore

    /// <summary>
    /// Finds the value associated with the given key.
    /// </summary>
    /// <param name="key">the found value, or the type-default value when not found.</param>
    public V this[K key] => Find(key).Item;

    /// <summary>
    /// Check whether this dictionary contains the given key.
    /// </summary>
    /// <param name="key">key to find.</param>
    /// <returns>whether the given key found.</returns>
    public abstract bool ContainsKey(K key);

    /// <summary>
    /// Number of pairs in the dictionary.
    /// </summary>
    public abstract int Count { get; }

    /// <summary>
    /// Whether this dictionary has at least one pair.
    /// </summary>
    public abstract bool IsNotEmpty { get; }

    /// <summary>
    /// Whether this dictionary is empty.
    /// </summary>
    public abstract bool IsEmpty { get; }

    /// <summary>
    /// Set of keys.
    /// </summary>
    public abstract IReadOnlyCollection<K> Keys { get; }

    /// <summary>
    /// Collection of values.
    /// </summary>
    public abstract IReadOnlyCollection<V> Values { get; }

    /// <summary>
    /// All key-value pairs as en IEnumerable.
    /// </summary>
    public abstract IReadOnlyCollection<KeyValuePair<K,V>> Entries { get; }

    /// <summary>
    /// Creates an enumerator of all pairs.
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator<KeyValuePair<K, V>> GetEnumerator();


    public bool TryGetValue(K key, out V value)
    {
        var f = Find(key);
        value = f.Item;
        return f.Ok;
    }


    IEnumerable<K> IReadOnlyDictionary<K, V>.Keys   => Keys;
    IEnumerable<V> IReadOnlyDictionary<K, V>.Values => Values;

    IEnumerator IEnumerable.GetEnumerator() => Entries.GetEnumerator();

    public override string ToString() => $"{this.GetType().Name}({Count} pairs)";

}
