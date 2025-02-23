using System;
using System.Collections;
using System.Collections.Generic;
using Util.Structures;
using static Util.Collections.ImmConst;
using static Util.Collections.Implementation.CollectionLogic;

namespace Util.Collections.Implementation;


/// <summary>
/// Fast special implementation of the dictionary for a case
/// when the keys are near each other.
/// This dictionary doesn't preserve the original order, it sorts the keys instead.
/// </summary>
/// <typeparam name="V">type of the value.</typeparam>
internal class ImmutableFlatDictionary<V> : ImmutableDictionary<uint,V>, ImmDict<uint,V>
{
    private  readonly BitArray Spots;
    private  readonly V[]      Cells;
    internal readonly uint     MinKey;
    internal readonly uint     MaxKey;
    internal readonly uint     Capacity;
    private  readonly int      Cnt;


    internal ImmutableFlatDictionary(IReadOnlyDictionary<uint,V> source)
        : this(CollectInterval(source), source)
    { }

    internal ImmutableFlatDictionary(IReadOnlyCollection<KeyValuePair<uint,V>> source)
        : this(CollectInterval(source), source)
    { }

    internal ImmutableFlatDictionary(UIntInterval keyInterval, IEnumerable<KeyValuePair<uint,V>> source)
    {
        MinKey   = keyInterval.Min;
        MaxKey   = keyInterval.Max;
        Capacity = keyInterval.Length;

        int cnt = 0;
        Spots = new BitArray((int)Capacity);
        Cells = new V[Capacity];
        foreach (KeyValuePair<uint,V> pair in source)
        {
            int offset = (int)(pair.Key - MinKey);
            if (!Spots[offset])
            {
                cnt++;
                Spots[offset] = true;
                Cells[offset] = pair.Value;
            }
        }

        Cnt = cnt;
    }

    internal static UIntInterval CollectInterval(IEnumerable<KeyValuePair<uint,V>> source)
    {
        uint min = 0u; // just for compiler, will be re-assigned in the first iteration
        uint max = 0u;
        bool neo = true;
        foreach (KeyValuePair<uint,V> pair in source)
        {
            uint key = pair.Key;
            if (neo)
            {
                min = key;
                max = key;
                neo = false;
            }
            else
            {
                if (key < min) min = key;
                if (key > max) max = key;
            }
        }

        return new UIntInterval(min, max);
    }


    public int  Count      => Cnt;
    public bool IsNotEmpty => Cnt != 0;
    public bool IsEmpty    => Cnt == 0;

    public int IndexOfKey(uint key) => IndexOfKey(key, notFoundIndex);

    public int IndexOfKey(uint key, int notFound)
    {
        if (key >= MinKey && key <= MaxKey)
        {
            int candidate = (int)(key - MinKey);
            return Spots[candidate] ? candidate : notFound;
        }
        else
        {
            return notFound;
        }
    }

    public bool ContainsKey(uint key) => IndexOfKey(key) >= 0;

    public Found<V> Find(uint key) => key >= MinKey && key <= MaxKey
        ? new Found<V>(Spots[(int)(key - MinKey)], Cells[key - MinKey])
        : Found<V>.NotFound;


    public ImmSet<uint>                        Keys    => new KeySet(this);
    public ImmCollection<V>                    Values  => new ValueCollection(this);
    public ImmCollection<KeyValuePair<uint,V>> Entries => new EntryCollection(this);


    // INNER CLASSES \\

    private sealed class KeySet : ImmutableCollection<uint>, ImmSet<uint>
    {
        private readonly ImmutableFlatDictionary<V> Dict;

        internal KeySet(ImmutableFlatDictionary<V> dict)
        {
            Dict = dict;
        }

        public int  Count      => Dict.Count;
        public bool IsNotEmpty => Dict.IsNotEmpty;
        public bool IsEmpty    => Dict.IsEmpty;

        public bool Contains(uint item) => Dict.ContainsKey(item);

        public bool Contains(Predicate<uint> predicate)
        {
            for (uint j = 0; j < Dict.Capacity; j++)
            {
                if (Dict.Spots[(int)j] && predicate(Dict.MinKey + j)) return true;
            }
            return false;
        }

        public Found<uint> Find(Predicate<uint> predicate)
        {
            for (uint j = 0; j < Dict.Capacity; j++)
            {
                if (Dict.Spots[(int)j])
                {
                    uint x = Dict.MinKey + j;
                    if (predicate(x)) return new Found<uint>(true,x);
                }
            }
            return Found<uint>.NotFound;
        }


        public bool IsSubsetOf(IEnumerable<uint> other)
            => IsTheSetSubsetOf((int)Dict.Capacity, Dict.Cnt, Dict.IndexOfKey, other, false);

        public bool IsProperSubsetOf(IEnumerable<uint> other)
            => IsTheSetSubsetOf((int)Dict.Capacity, Dict.Cnt, Dict.IndexOfKey, other, true);

        public bool IsProperSupersetOf(IEnumerable<uint> other)
            => IsTheSetSupersetOf((int)Dict.Capacity, Dict.Cnt, Dict.IndexOfKey, other, true);

        public bool IsSupersetOf(IEnumerable<uint> other)
            => IsTheSetSupersetOf((int)Dict.Capacity, Dict.Cnt, Dict.IndexOfKey, other, false);

        public bool Overlaps(IEnumerable<uint> other)
            => IsTheSetOverlapping(Contains, other);

        public bool SetEquals(IEnumerable<uint> other)
            => IsTheSetEqualTo((int)Dict.Capacity, Dict.Cnt, Dict.IndexOfKey, other);

        public IEnumerator<uint> GetEnumerator()
        {
            for (uint x = 0; x < Dict.Capacity; x++)
            {
                int index = (int)x;
                if (Dict.Spots[index]) yield return Dict.MinKey + x;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }



    private sealed class ValueCollection : ImmutableCollection<V>, ImmCollection<V>
    {
        private readonly ImmutableFlatDictionary<V> Dict;

        internal ValueCollection(ImmutableFlatDictionary<V> dict)
        {
            Dict = dict;
        }

        public int  Count      => Dict.Count;
        public bool IsNotEmpty => Dict.IsNotEmpty;
        public bool IsEmpty    => Dict.IsEmpty;

        public Found<V> Find(Predicate<V> predicate)
        {
            for (int i = 0; i < Dict.Capacity; i++)
            {
                if (Dict.Spots[i])
                {
                    V value = Dict.Cells[i];
                    if (predicate(value)) return new Found<V>(true, value);
                }
            }
            return Found<V>.NotFound;
        }

        public bool Contains(Predicate<V> predicate) => Find(predicate).Ok;

        public IEnumerator<V> GetEnumerator()
        {
            for (int i = 0; i < Dict.Capacity; i++)
                if (Dict.Spots[i]) yield return Dict.Cells[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }


    private sealed class EntryCollection : ImmutableCollection<KeyValuePair<uint,V>>, ImmCollection<KeyValuePair<uint,V>>
    {
        private readonly ImmutableFlatDictionary<V> Dict;

        internal EntryCollection(ImmutableFlatDictionary<V> dict)
        {
            Dict = dict;
        }

        public int  Count      => Dict.Count;
        public bool IsNotEmpty => Dict.IsNotEmpty;
        public bool IsEmpty    => Dict.IsEmpty;

        public Found<KeyValuePair<uint,V>> Find(Predicate<KeyValuePair<uint,V>> predicate)
        {
            for (int i = 0; i < Dict.Capacity; i++)
            {
                if (Dict.Spots[i])
                {
                    uint key   = Dict.MinKey + (uint)i;
                    V    value = Dict.Cells[i];
                    var  pair  = new KeyValuePair<uint, V>(key, value);
                    if (predicate(pair)) return new Found<KeyValuePair<uint,V>>(true, pair);
                }
            }
            return Found<KeyValuePair<uint,V>>.NotFound;
        }

        public bool Contains(Predicate<KeyValuePair<uint,V>> predicate) => Find(predicate).Ok;

        public IEnumerator<KeyValuePair<uint,V>> GetEnumerator()
        {
            for (int i = 0; i < Dict.Capacity; i++)
            {
                if (Dict.Spots[i])
                {
                    uint key   = Dict.MinKey + (uint)i;
                    V    value = Dict.Cells[i];
                    var  pair  = new KeyValuePair<uint, V>(key, value);
                    yield return pair;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
