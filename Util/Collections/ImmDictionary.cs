using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Util.Collections;

/// <summary>
/// Immutable dictionary.
/// </summary>
/// <typeparam name="K">type of keys.</typeparam>
/// <typeparam name="V">type of values.</typeparam>
public abstract class ImmDictionary<K,V> : IReadOnlyDictionary<K,V>
{

    /// <summary>
    /// Whether the dictionary contains at least one entry.
    /// </summary>
    public abstract bool IsNotEmpty { get; }

    /// <summary>
    /// Whether the dictionary is empty.
    /// </summary>
    public abstract bool IsEmpty { get; }

    /// <summary>
    /// Count of entries.
    /// </summary>
    public abstract int Count { get; }

    /// <summary>
    /// Finds the value by the key.
    /// </summary>
    /// <param name="key">the key to look for.</param>
    /// <returns>result of the search.</returns>
    public abstract Found<V> Find(K key);

    /// <summary>
    /// Finds the value by the key.
    /// </summary>
    /// <param name="key">the key to look for.</param>
    /// <param name="notFound">what to return when not found.</param>
    /// <returns>found associated value, or the defaultValue if not found.</returns>
    public abstract V Get(K key, V notFound = default(V));

    /// <summary>
    /// Finds the entry with the specified key and return its index.
    /// </summary>
    /// <param name="key">key to find.</param>
    /// <returns>index of the entry (zero-based), or a negative number when not found.</returns>
    public abstract int IndexOfKey(K key);

    /// <summary>
    /// Checks whether an entry with the specified key exists.
    /// </summary>
    /// <param name="key">key to find.</param>
    /// <returns>true if exists.</returns>
    public abstract bool ContainsKey(K key);

    public bool TryGetValue(K key, out V value)
    {
        var f = Find(key);
        value = f.Item;
        return f.Ok;
    }

    /// <summary>
    /// Finds the value by the key.
    /// </summary>
    /// <param name="key">the key to find.</param>
    public V this[K key] => Find(key).Item;

    /// <summary>
    /// Set of keys.
    /// </summary>
    public abstract IReadOnlyCollection<K> Keys { get; }

    IEnumerable<K> IReadOnlyDictionary<K, V>.Keys => Keys;

    /// <summary>
    /// Collection of values.
    /// </summary>
    public abstract IReadOnlyCollection<V> Values { get; }

    IEnumerable<V> IReadOnlyDictionary<K, V>.Values => Values;

    /// <summary>
    /// List of all entries.
    /// </summary>
    public abstract ImmList<KeyValuePair<K, V>> Entries { get; }

    /// <summary>
    /// Enumerates all entries.
    /// </summary>
    public abstract IEnumerator<KeyValuePair<K, V>> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


    internal static readonly Found<V> notFound = new Found<V>(default(V), false);

}



/// <summary>
/// Empty dictionary.
/// </summary>
public sealed class ImmZeroDictionary<K,V> : ImmDictionary<K,V>
{
    /// <summary>
    /// The instance of an empty dictionary.
    /// </summary>
    public static readonly ImmZeroDictionary<K, V> Instance = new ImmZeroDictionary<K,V>();

    private ImmZeroDictionary() { }

    public override bool IsNotEmpty => false;
    public override bool IsEmpty    => true;
    public override int  Count      => 0;

    public override IReadOnlyCollection<K>     Keys    => ImmZeroSet<K>.Instance;
    public override IReadOnlyCollection<V>     Values  => ImmZeroSet<V>.Instance;
    public override ImmList<KeyValuePair<K,V>> Entries => ImmZeroSet<KeyValuePair<K,V>>.Instance;

    public override Found<V> Find(K key) => notFound;

    public override V Get(K key, V notFound = default(V)) => notFound;

    public override int  IndexOfKey(K  key) => -1;
    public override bool ContainsKey(K key) => false;

    public override IEnumerator<KeyValuePair<K,V>> GetEnumerator() => Enumerable.Empty<KeyValuePair<K,V>>().GetEnumerator();

}

