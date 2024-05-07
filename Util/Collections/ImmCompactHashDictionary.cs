using System;
using System.Collections.Generic;

namespace Util.Collections;

public sealed class ImmCompactHashDictionary<K,V> : ImmArrayDictionary<K,V>
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


    public ImmCompactHashDictionary(KeyValuePair<K, V>[] pairs)
        : this(pairs, pairs.Length) { }

    private ImmCompactHashDictionary(IEnumerable<KeyValuePair<K, V>> pairs, int size)
        : base(size)
    {
        uint n = (uint)size;
        links = new uint[n];
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
                EntriesArray[k] = p;
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

            EntriesArray[i] = p;

            links[i] =  BusyBit | ContinueBit;
            links[k] |= HasNextBit | i & NextIndexBits;
        }
    }


    protected override int FindEntryIndex(K key)
    {
        int  k = HashIndexOf(key, (uint)links.Length);
        uint x = links[k];
        if ((x & ContinueBit) != 0) return int.MinValue;

        while (true)
        {
            if (comparer.Equals(EntriesArray[k].Key, key)) return k;
            if ((x & HasNextBit) == 0u) return int.MinValue;
            k = (int)(x & NextIndexBits);
            x = links[k];
        }
    }


    private static int HashIndexOf(K key, uint n)
    {
        uint h = HashOf(key);
        return unchecked((int)(h % n));
    }

}
