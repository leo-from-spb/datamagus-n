using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Util.Collections;


public class ImmCompactHashDictionary1<K,V> : IReadOnlyDictionary<K,V>
{

    private const uint BusyBit       = 0x40000000u;  // bit 30
    private const uint ContinueBit   = 0x20000000u;  // bit 29
    private const uint HasNextBit    = 0x10000000u;  // bit 28
    private const uint NextIndexBits = 0x0FFFFFFFu;  // bits 27..0

    /// <summary>
    /// Bit 31: always zero
    /// Bit 30: 0 — the cell is empty (during constructing only),
    ///         1 — busy.
    /// Bit 29: 0 — it is the first link in the chain, and the hash code true relates to the cell index,
    ///         1 — it's a continuation of another hash code.
    /// Bit 28: 0 — this cell is the last link in the chain,
    ///         1 — there are more chains (and bits 27..0 point to the next link)
    /// Bits 27..0: index of the next link cell.
    /// </summary>
    private readonly uint[] links;

    /// <summary>
    /// Key-value pairs.
    /// </summary>
    private readonly KeyValuePair<K,V>[] entries;


    private static readonly EqualityComparer<K> comparer = EqualityComparer<K>.Default;


    public ImmCompactHashDictionary1(KeyValuePair<K, V>[] pairs)
        : this(pairs, pairs.Length) { }

    private ImmCompactHashDictionary1(IEnumerable<KeyValuePair<K,V>> pairs, int size)
    {
        uint n  = (uint)size;
        entries = new KeyValuePair<K,V>[n];
        links   = new uint[n];
        Array.Fill(links, 0u);

        // the queue for unhappy entries
        var rest = new List<KeyValuePair<K, V>>(size/2+1);

        // first, handle all happy entries
        uint cnt = 0;
        foreach (var p in pairs)
        {
            int k = HashIndexOf(p.Key, n);

            if (links[k] == 0u)
            {
                entries[k] = p;
                links[k]   = BusyBit;
            }
            else
            {
                rest.Add(p);
            }

            cnt++;
            if (cnt >= n) break;
        }

        // then, handle unhappy ones
        uint i = 0u;
        foreach (var p in rest)
        {
            int k = HashIndexOf(p.Key, n);

            while ((links[k] & HasNextBit) != 0u) k = (int)(links[k] & NextIndexBits);

            while (links[i] != 0u) i++;

            entries[i] = p;
            links[i]   = BusyBit | ContinueBit;

            links[k] |= HasNextBit | i & NextIndexBits;
        }
    }


    private int FindIndex(K key)
    {
        int k = HashIndexOf(key, (uint)links.Length);
        uint x = links[k];
        if ((x & ContinueBit) != 0) return int.MinValue;

        while (true)
        {
            if (comparer.Equals(entries[k].Key, key)) return k;
            if ((x & HasNextBit) == 0u) return int.MinValue;
            k = (int)(x & NextIndexBits);
            x = links[k];
        }
    }


    private static int HashIndexOf(K key, uint n)
    {
        #nullable disable
        int h = comparer.GetHashCode(key);
        #nullable restore
        unchecked
        {
            return (int)(((uint)h) % n);
        }
    }


    public bool ContainsKey(K key) => FindIndex(key) >= 0;

    #nullable disable
    public V Get(K key, V noValue = default(V))
    {
        int index = FindIndex(key);
        return index >= 0 ? entries[index].Value : noValue;
    }
    #nullable restore

    public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value)
    {
        int index = FindIndex(key);
        bool ok = index >= 0;
        value = ok ? entries[index].Value : default(V);
        return ok;
    }

    public V this[K key] => Get(key);

    V IReadOnlyDictionary<K,V>.this[K key]
    {
        get
        {
            int index = FindIndex(key);
            if (index >= 0) return entries[index].Value;
            else throw new KeyNotFoundException($"This dictionary of {Count} entries has no key {key}");
        }
    }

    public IReadOnlyCollection<K> Keys => new KeySet(this);

    public IReadOnlyCollection<V> Values => new ValueCollection(this);

    public IReadOnlyCollection<KeyValuePair<K, V>> Entries => entries;
    
    public int Count => entries.Length;

    IEnumerable<K> IReadOnlyDictionary<K, V>.Keys => entries.Select(e => e.Key);

    IEnumerable<V> IReadOnlyDictionary<K, V>.Values => entries.Select(e => e.Value);

    public IEnumerator<KeyValuePair<K,V>> GetEnumerator() => entries.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => entries.GetEnumerator();


    private class KeySet : IReadOnlyCollection<K>
    {
        private readonly ImmCompactHashDictionary1<K, V> D;

        internal KeySet(ImmCompactHashDictionary1<K, V> dictionary) => D = dictionary;

        IEnumerator IEnumerable.GetEnumerator() => D.entries.Select(e => e.Key).GetEnumerator();

        public IEnumerator<K> GetEnumerator() => D.entries.Select(e => e.Key).GetEnumerator();

        public int Count => D.Count;

        public bool Contains(K item) => D.ContainsKey(item);
    }


    private class ValueCollection : IReadOnlyCollection<V>
    {
        private readonly ImmCompactHashDictionary1<K, V> D;

        internal ValueCollection(ImmCompactHashDictionary1<K, V> dictionary) => D = dictionary;

        IEnumerator IEnumerable.GetEnumerator() => D.entries.Select(e => e.Value).GetEnumerator();

        public IEnumerator<V> GetEnumerator() => D.entries.Select(e => e.Value).GetEnumerator();

        public int Count => D.Count;
    }
}
