using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Util.Collections;


public class ImmCompactHashDictionary<K,V> : IReadOnlyDictionary<K,V>
{

    private const uint BusyBit       = 0x40000000u;
    private const uint ContinueBit   = 0x20000000u;
    private const uint HasNextBit    = 0x10000000u;
    private const uint NextIndexBits = 0x0FFFFFFFu;

    /// <summary>
    /// Bit 31: always zero
    /// Bit 30: 0 — the cell is empty (during constructing only),
    ///         1 — busy.
    /// Bit 29: 0 — it is the first link in the chain, and the hash code true relates to the cell index,
    ///         1 — it's a continuation of another hash code.
    /// Bit 28: 0 — this cell is the last link in the chain,
    ///         1 — there are more chains (and bits 27..0 points to the next link)
    /// Bits 27..0: index of the next link cell.
    /// </summary>
    private uint[] links;

    /// <summary>
    /// Key-value pairs.
    /// </summary>
    private KeyValuePair<K,V>[] entries;


    private static EqualityComparer<K> comparer = EqualityComparer<K>.Default;


    public ImmCompactHashDictionary(KeyValuePair<K, V>[] pairs)
        : this(pairs, pairs.Length) { }

    private ImmCompactHashDictionary(IEnumerable<KeyValuePair<K,V>> pairs, int size)
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
            uint k = HashOf(p.Key, n);

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

        // then, handle unhappy ines
        uint i = 0u;
        foreach (var p in rest)
        {
            uint k = HashOf(p.Key, n);

            while ((links[k] & HasNextBit) != 0u) k = links[k] & NextIndexBits;

            while (links[i] != 0u) i++;

            entries[i] = p;
            links[i]   = BusyBit | ContinueBit;

            links[k] |= HasNextBit | i & NextIndexBits;
        }
    }

    private static uint HashOf(K key, uint n)
    {
        int h = comparer.GetHashCode(key);
        unchecked
        {
            return ((uint)h) % n;
        }
    }


    public bool ContainsKey(K key)
    {
        uint k = HashOf(key, (uint)links.Length);
        uint x = links[k];
        if ((x & ContinueBit) != 0) return false;

        while (true)
        {
            if (comparer.Equals(entries[k].Key, key)) return true;
            if ((x & HasNextBit) == 0u) return false;
            k = x & NextIndexBits;
            x = links[k];
        }
    }

    public V Get(K key, V noValue = default(V))
    {
        uint k = HashOf(key, (uint)links.Length);
        uint x = links[k];
        if ((x & ContinueBit) != 0) return noValue;

        while (true)
        {
            if (comparer.Equals(entries[k].Key, key)) return entries[k].Value;
            if ((x & HasNextBit) == 0u) return noValue;
            k = x & NextIndexBits;
            x = links[k];
        }
    }

    public V this[K key] => Get(key);

    public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value)
    {
        uint k = HashOf(key, (uint)links.Length);
        uint x = links[k];
        if ((x & ContinueBit) != 0) { value = default(V); return false; }

        while (true)
        {
            if (comparer.Equals(entries[k].Key, key)) { value = entries[k].Value; return true; }
            if ((x & HasNextBit) == 0u) { value = default(V); return false; }
            k = x & NextIndexBits;
            x = links[k];
        }
    }

    V IReadOnlyDictionary<K,V>.this[K key]
    {
        get
        {
            bool ok = TryGetValue(key, out var value);
            if (ok) return value;
            else throw new KeyNotFoundException($"This dictionary of {Count} entries has no key {key}");
        }
    }

    public int Count => entries.Length;

    public IEnumerable<K> Keys => entries.Select(e => e.Key);

    public IEnumerable<V> Values => entries.Select(e => e.Value);

    public IEnumerator<KeyValuePair<K,V>> GetEnumerator() => entries.AsEnumerable().GetEnumerator();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => entries.GetEnumerator();

}
