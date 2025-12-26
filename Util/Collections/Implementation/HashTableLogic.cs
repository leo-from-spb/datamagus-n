using System;
using System.Collections.Generic;

namespace Util.Collections.Implementation;

internal static class HashTableLogic
{

    internal static void BuildHashTable<E,K>(ArraySegment<E> data,
                                             Func<E,K> keyGet,
                                             EqualityComparer<K> comparer,
                                             bool checkForDuplicates,
                                             out HashTableEntry[] hashTable)
    {
        int N = data.Count;
        uint M = CalculateM(N);

        HashTableEntry[] ht = new HashTableEntry[M];
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
                if (checkForDuplicates)
                    CheckForDuplicates(data, keyGet, comparer, ht, key, k, i);
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

    private static void CheckForDuplicates<E,K>(ArraySegment<E>     data,
                                                Func<E,K>           keyGet,
                                                EqualityComparer<K> comparer,
                                                HashTableEntry[]    hashTable,
                                                K                   key,
                                                uint                hashIndex,
                                                int                 entryIndex)
    {
        uint k = hashIndex;
        bool toContinue;
        do
        {
            var e          = hashTable[k];
            int dataIndex  = e.TargetIndex;
            E   dataEntry  = data[dataIndex];
            K   anotherKey = keyGet(dataEntry);
            if (comparer.Equals(anotherKey, key))
                throw new KeyDuplicationException(dataIndex, entryIndex, key);
            k          = e.NextIndex;
            toContinue = e.HasNext;
        } while (toContinue);
    }


    internal static int FindIndex<E,K>(ArraySegment<E> data,
                                       HashTableEntry[] hashTable,
                                       Func<E,K> keyGet,
                                       EqualityComparer<K> comparer,
                                       K key,
                                       int notFound)
    {
        uint           M = (uint)hashTable.Length;
        uint           k = HashIndexOf(key, M, comparer);
        HashTableEntry e = hashTable[k];
        if (e.IsContinue) return notFound;

        while (e.IsBusy)
        {
            K itemKey = keyGet(data[e.TargetIndex]);
            if (comparer.Equals(itemKey, key)) return e.TargetIndex;
            if (!e.HasNext) return notFound;
            k = e.NextIndex;
            e = hashTable[k];
        }

        return notFound;
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


    private static readonly HashTableEntry ZeroEntry = new HashTableEntry();

}
