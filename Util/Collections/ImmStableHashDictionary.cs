using System;
using System.Collections.Generic;
using System.Linq;

namespace Util.Collections;


public sealed class ImmStableHashDictionary<K,V> : ImmArrayDictionary<K,V>
{

    private const uint BusyBit       = 0x40000000u;  // bit 30
    private const uint ContinueBit   = 0x20000000u;  // bit 29
    private const uint HasNextBit    = 0x10000000u;  // bit 28
    private const uint NextIndexBits = 0x0FFFFFFFu;  // bits 27..0


    /// <summary>
    /// Entry in the Hash Table (see <see cref="Links"/>).
    /// </summary>
    private struct LinkEntry
    {
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
        internal uint Link;

        /// <summary>
        /// Index of the pair entry in the target array (<see cref="ImmArrayDictionary{K,V}.EntriesArray"/>).
        /// </summary>
        internal int TargetIndex;
    }


    /// <summary>
    /// Size of the hash table.
    /// Must be larger than <see cref="N"/>.
    /// </summary>
    private readonly uint M;

    /// <summary>
    /// The Hash Table.
    /// </summary>
    private readonly LinkEntry[] Links;


    public ImmStableHashDictionary(IEnumerable<KeyValuePair<K, V>> pairs)
        : this(pairs.ToArray(), false) { }

    public ImmStableHashDictionary(KeyValuePair<K, V>[] pairs)
        : this(pairs, true) { }

    private ImmStableHashDictionary(KeyValuePair<K, V>[] array, bool toCopy)
        : base(array, toCopy)
    {
        M     = CalculateM(N);
        Links = new LinkEntry[M];
        Array.Fill(Links, ZeroLinkEntry);

        // the queue for unhappy entries' indices
        var rest = new List<int>(N/2+1);

        // first, handle all happy entries
        for (int i = 0; i < N; i++)
        {
            K  key = EntriesArray[i].Key;
            uint k = HashIndexOf(key, M);

            if (Links[k].Link == 0u)
            {
                Links[k] = new LinkEntry { Link = BusyBit, TargetIndex = i };
            }
            else
            {
                rest.Add(i);
            }
        }

        // then, handle unhappy ones
        uint j = 0u;
        foreach (int i in rest)
        {
            K  key = EntriesArray[i].Key;
            uint k = HashIndexOf(key, M);

            while ((Links[k].Link & HasNextBit) != 0u) k = Links[k].Link & NextIndexBits;

            while (Links[j].Link != 0u) j++;

            Links[j] =  new LinkEntry { Link = BusyBit | ContinueBit, TargetIndex = i };
            Links[k].Link |= HasNextBit | (j & NextIndexBits);
        }
    }

    private static uint CalculateM(int n)
    {
        uint m = (uint)n * 2u + 7u;
        if ((m & 1u) == 0) m++;
        return m;
    }

    private int FindIndex(K key)
    {
        uint      k = HashIndexOf(key, M);
        LinkEntry e = Links[k];
        if ((e.Link & ContinueBit) != 0) return int.MinValue;

        while (true)
        {
            if (comparer.Equals(EntriesArray[e.TargetIndex].Key, key)) return e.TargetIndex;
            if ((e.Link & HasNextBit) == 0u) return int.MinValue;
            k = e.Link & NextIndexBits;
            e = Links[k];
        }
    }

    private static uint HashIndexOf(K key, uint m)
    {
        #nullable disable
        int h = comparer.GetHashCode(key);
        #nullable restore
        unchecked
        {
            return ((uint)h) % m;
        }
    }

    public override bool ContainsKey(K key) => FindIndex(key) >= 0;

    public override Found<V> Find(K key)
    {
        int index = FindIndex(key);
        #nullable disable
        V   value  = index >= 0 ? EntriesArray[index].Value : default(V);
        var result = new Found<V>(index >= 0, value);
        #nullable restore
        return result;
    }

    public override V Get(K key,
                          #nullable disable
                          V noValue = default(V))
                          #nullable restore
    {
        int index = FindIndex(key);
        return index >= 0 ? EntriesArray[index].Value : noValue;
    }


    private static readonly LinkEntry ZeroLinkEntry = new LinkEntry { Link = 0u, TargetIndex = 0 };

}
