using System;
using System.Collections.Generic;

namespace Util.Collections;

internal static class HashTableLogic
{

    internal static void BuildHasTable<E,K>(ArraySegment<E> data, Func<E,K> keyGet, EqualityComparer<K> comparer, out HashTableEntry[] hashTable)
    {
        int N = data.Count;
        uint M = CalculateM(N);

        HashTableEntry[]  ht = new HashTableEntry[M];
        Array.Fill(ht, ZeroEntry);

        // the queue for unhappy entries' indices
        var rest = new List<int>(N/2+1);

        // first, handle all happy entries
        for (int i = 0; i < N; i++)
        {
            K key = keyGet(data[i]);
            uint k = HashIndexOf(key, M, comparer);

            if (ht[k].Link == 0u)
            {
                ht[k] = new HashTableEntry { Link = HashTableEntry.BusyBit, TargetIndex = i };
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
            K    key = keyGet(data[i]);
            uint k   = HashIndexOf(key, M, comparer);

            while (ht[k].HasNext) k = ht[k].Link & HashTableEntry.NextIndexBits;

            while (ht[j].Link != 0u) j++;

            ht[j]      =  new HashTableEntry { Link = HashTableEntry.BusyBit | HashTableEntry.ContinueBit, TargetIndex = i };
            ht[k].Link |= HashTableEntry.HasNextBit | (j & HashTableEntry.NextIndexBits);
        }

        // done
        hashTable = ht;
    }


    internal static int FindIndex<E,K>(ArraySegment<E> data, HashTableEntry[] hashTable, Func<E,K> keyGet, EqualityComparer<K> comparer, K key)
    {
        uint           M = (uint)hashTable.Length;
        uint           k = HashIndexOf(key, M, comparer);
        HashTableEntry e = hashTable[k];
        if (e.IsContinue) return -1;

        while (e.IsBusy)
        {
            K itemKey = keyGet(data[e.TargetIndex]);
            if (comparer.Equals(itemKey, key)) return e.TargetIndex;
            if (!e.HasNext) return -1;
            k = e.NextIndex;
            e = hashTable[k];
        }

        return -1;
    }


    private static uint CalculateM(int n)
    {
        uint m = (uint)n * 2u + 7u;
        //if ((m & 1u) == 0) m++;
        return m;
    }


    private static uint HashIndexOf<K>(K key, uint m, EqualityComparer<K> comparer)
    {
        #nullable disable
        int h = comparer.GetHashCode(key);
        #nullable restore
        unchecked
        {
            return ((uint)h) % m;
        }
    }


    private static HashTableEntry ZeroEntry = new HashTableEntry();

}
