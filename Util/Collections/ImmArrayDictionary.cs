using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Util.Collections;

/// <summary>
/// Immutable dictionary based on an array.
/// </summary>
/// <typeparam name="K">type of keys.</typeparam>
/// <typeparam name="V">type of values.</typeparam>
public abstract class ImmArrayDictionary<K,V> : ImmDictionary<K,V>
{
    protected static readonly EqualityComparer<K> KeyComparer = EqualityComparer<K>.Default;

    /// <summary>
    /// Internal array of entries.
    /// </summary>
    protected readonly ArraySegment<KeyValuePair<K,V>> segment;


    internal ImmArrayDictionary(ArraySegment<KeyValuePair<K, V>> segment)
    {
        this.segment = segment;
    }

    /// <summary>
    /// Whether the dictionary contains at least one entry.
    /// </summary>
    public override bool IsNotEmpty => segment.Count != 0;

    /// <summary>
    /// Whether the dictionary is empty.
    /// </summary>
    public override bool IsEmpty => segment.Count == 0;

    /// <summary>
    /// Count of entries.
    /// </summary>
    public override int Count => segment.Count;

    /// <summary>
    /// Finds the value by the key.
    /// </summary>
    /// <param name="key">the key to look for.</param>
    /// <returns>result of the search.</returns>
    public abstract override Found<V> Find(K key);

    /// <summary>
    /// Finds the value by the key.
    /// </summary>
    /// <param name="key">the key to look for.</param>
    /// <param name="notFound">what to return when not found.</param>
    /// <returns>found associated value, or the defaultValue if not found.</returns>
    public abstract override V Get(K key, V notFound = default(V));

    public abstract override int IndexOfKey(K key);

    public abstract override bool ContainsKey(K key);

    /// <summary>
    /// Set of keys.
    /// </summary>
    public override IReadOnlyCollection<K> Keys => new KeySet(this);

    /// <summary>
    /// Collection of values.
    /// </summary>
    public override IReadOnlyCollection<V> Values => new ValueCollection(this);

    /// <summary>
    /// List of all entries.
    /// </summary>
    public override ImmList<KeyValuePair<K, V>> Entries => new ImmList<KeyValuePair<K, V>>(segment);

    /// <summary>
    /// Enumerates all entries.
    /// </summary>
    public override IEnumerator<KeyValuePair<K, V>> GetEnumerator() => segment.GetEnumerator();


    /// <summary>
    /// The key set.
    /// </summary>
    protected class KeySet : IReadOnlyCollection<K>
    {
        private readonly ImmArrayDictionary<K, V> D;

        internal KeySet(ImmArrayDictionary<K, V> dictionary) => D = dictionary;

        IEnumerator IEnumerable.GetEnumerator() => D.segment.Select(e => e.Key).GetEnumerator();

        public IEnumerator<K> GetEnumerator() => D.segment.Select(e => e.Key).GetEnumerator();

        public int Count => D.Count;

        public bool Contains(K item) => D.ContainsKey(item);
    }

    /// <summary>
    /// The value collection.
    /// </summary>
    protected class ValueCollection : IReadOnlyCollection<V>
    {
        private readonly ImmArrayDictionary<K, V> D;

        internal ValueCollection(ImmArrayDictionary<K, V> dictionary) => D = dictionary;

        IEnumerator IEnumerable.GetEnumerator() => D.segment.Select(e => e.Value).GetEnumerator();

        public IEnumerator<V> GetEnumerator() => D.segment.Select(e => e.Value).GetEnumerator();

        public int Count => D.Count;
    }


}






/// <summary>
/// Immutable mini-dictionary for 1-3 entries.
/// Preserves origianl the order.
/// </summary>
/// <typeparam name="K">type of keys.</typeparam>
/// <typeparam name="V">type of values.</typeparam>
public sealed class ImmMiniDictionary<K, V> : ImmArrayDictionary<K, V>
{
    internal ImmMiniDictionary(ArraySegment<KeyValuePair<K, V>> segment) : base(segment) { }

    public override Found<V> Find(K key)
    {
        for (int i = 0, n = segment.Count; i < n; i++)
            if (KeyComparer.Equals(segment[i].Key, key))
                return new Found<V>(segment[i].Value, true);
        return notFound;
    }

    public override V Get(K key, V notFound = default(V))
    {
        for (int i = 0, n = segment.Count; i < n; i++)
            if (KeyComparer.Equals(segment[i].Key, key))
                return segment[i].Value;
        return notFound;
    }

    public override int IndexOfKey(K key)
    {
        for (int i = 0, n = segment.Count; i < n; i++)
            if (KeyComparer.Equals(segment[i].Key, key))
                return i;
        return -1;
    }

    public override bool ContainsKey(K key) => IndexOfKey(key) >= 0;

}



/// <summary>
/// Immutable hash dictionary.
/// Preserves origianl the order.
/// </summary>
/// <typeparam name="K">type of keys.</typeparam>
/// <typeparam name="V">type of values.</typeparam>
public sealed class ImmHashDictionary<K,V> : ImmArrayDictionary<K,V>
{
    /// <summary>
    /// Hash table.
    /// </summary>
    private readonly HashTableEntry[] hashTable;


    internal ImmHashDictionary(KeyValuePair<K,V>[] entries) : base(entries)
    {
        HashTableLogic.BuildHasTable<KeyValuePair<K,V>,K>(entries, e => e.Key, KeyComparer, out this.hashTable);
    }

    internal ImmHashDictionary(ArraySegment<KeyValuePair<K,V>> segment, HashTableEntry[] hashTable) : base(segment)
    {
        this.hashTable = hashTable;
    }


    public override Found<V> Find(K key)
    {
        int index = IndexOfKey(key);
        return index >= 0 ? new Found<V>(segment[index].Value, true) : notFound;
    }

    public override V Get(K key, V notFound = default(V))
    {
        int index = IndexOfKey(key);
        return index >= 0 ? segment[index].Value : notFound;
    }

    public override int IndexOfKey(K key) =>
        HashTableLogic.FindIndex(segment, hashTable, e => e.Key, KeyComparer, key);

    public override bool ContainsKey(K key) => IndexOfKey(key) >= 0;

}