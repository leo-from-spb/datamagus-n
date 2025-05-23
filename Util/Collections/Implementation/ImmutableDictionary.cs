using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Util.Extensions;
using static Util.Collections.ImmConst;
using static Util.Collections.Implementation.CollectionLogic;
using static Util.Collections.Implementation.HashTableLogic;


namespace Util.Collections.Implementation;


/// <summary>
/// Array-based dictionary.
/// </summary>
/// <typeparam name="K">type of the key.</typeparam>
/// <typeparam name="V">type of the associated value.</typeparam>
internal abstract class ImmutableDictionary<K,V> : Collections.ImmutableDictionary<K,V>, ImmListDict<K,V>
{
    protected override string DictionaryWord => "ArrayDictionary";

    /// <summary>
    /// Pairs.
    /// </summary>
    protected readonly KeyValuePair<K,V>[] Pairs;

    /// <summary>
    /// Number of pairs.
    /// </summary>
    public override int Count { get; }


    protected ImmutableDictionary(KeyValuePair<K,V>[] pairs)
    {
        Debug.Assert(pairs.Length > 0);
        Pairs = pairs;
        Count = pairs.Length;
    }

    public bool IsNotEmpty => true;
    public bool IsEmpty    => false;

    public KeyValuePair<K, V> FirstEntry => Pairs[0];
    public KeyValuePair<K, V> LastEntry  => Pairs[^1];

    public abstract bool ContainsKey(K key);

    public abstract Found<V> Find(K key);

    public int IndexOfKey(K key) => IndexOfKey(key, notFoundIndex);

    public abstract int IndexOfKey(K key, int notFound);

    public abstract int LastIndexOfKey(K key, int notFound);


    public virtual ImmListSet<K> Keys   => new KeySet(this);
    public virtual ImmList<V>       Values => new ValueCollection(this);

    public virtual ImmList<KeyValuePair<K,V>> Entries => new ImmutableArrayList<KeyValuePair<K,V>>(Pairs);

    public IEnumerator<KeyValuePair<K,V>> GetEnumerator() => Pairs.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Pairs.GetEnumerator();



    // INNER CLASSES \\

    private class KeySet : ImmutableCollection<K>, ImmListSet<K>
    {
        private readonly ImmutableDictionary<K,V> Dict;

        protected override string CollectionWord =>  Dict.DictionaryWord + "KeySet";

        internal KeySet(ImmutableDictionary<K,V> dict)
        {
            Dict = dict;
        }

        public override int Count => Dict.Count;

        public bool IsNotEmpty => true;
        public bool IsEmpty    => false;

        public int IndexOf(K key) => Dict.IndexOfKey(key);

        public int IndexOf(K key, int notFound) => Dict.IndexOfKey(key, notFound);

        public int LastIndexOf(K key, int notFound) => Dict.LastIndexOfKey(key, notFound);

        public Found<K> Find(Predicate<K> predicate) => Dict.Pairs.FindElementPart(e => e.Key, predicate);

        public Found<K> FindFirst(Predicate<K> predicate, int fromIndex = 0) => Dict.Pairs.FindElementPart(e => e.Key, predicate, fromIndex);

        public Found<K> FindLast(Predicate<K> predicate) => Dict.Pairs.FindLastElementPart(e => e.Key, predicate);

        public bool Contains(Predicate<K> predicate) => Find(predicate).Ok;

        public bool Contains(K item) => IndexOf(item) >= 0;

        public K First => Dict.Pairs[0].Key;
        public K Last  => Dict.Pairs[^1].Key;

        public K At(int   index) => Dict.Pairs[index].Key;
        public K this[int index] => Dict.Pairs[index].Key;

        public bool IsSubsetOf(IEnumerable<K>         other) => IsTheSetSubsetOf(Dict.Count, IndexOf, other, false);
        public bool IsProperSubsetOf(IEnumerable<K>   other) => IsTheSetSubsetOf(Dict.Count, IndexOf, other, true);
        public bool IsProperSupersetOf(IEnumerable<K> other) => IsTheSetSupersetOf(Dict.Count, IndexOf, other, true);
        public bool IsSupersetOf(IEnumerable<K>       other) => IsTheSetSupersetOf(Dict.Count, IndexOf, other, false);

        public bool Overlaps(IEnumerable<K>  other) => IsTheSetOverlapping(Contains, other);
        public bool SetEquals(IEnumerable<K> other) => IsTheSetEqualTo(Dict.Count, IndexOf, other);

        public override IEnumerator<K> GetEnumerator() => new AdaptingEnumerator<KeyValuePair<K,V>,K>(Dict.GetEnumerator(), p => p.Key);
    }


    private class ValueCollection : ImmutableCollection<V>, ImmList<V>
    {
        private readonly ImmutableDictionary<K,V> Dict;

        protected override string CollectionWord => Dict.DictionaryWord + "ValueCollection";

        internal ValueCollection(ImmutableDictionary<K,V> dict)
        {
            Dict = dict;
        }

        public override int Count => Dict.Count;

        public bool IsNotEmpty => true;
        public bool IsEmpty    => false;

        public V First => Dict.Pairs[0].Value;
        public V Last  => Dict.Pairs[^1].Value;

        public V At(int   index) => Dict.Pairs[index].Value;
        public V this[int index] => Dict.Pairs[index].Value;

        public int IndexOf(V     value)               => Dict.Pairs.IndexOf(p => valueEq.Equals(p.Value, value));
        public int IndexOf(V     value, int notFound) => Dict.Pairs.IndexOf(p => valueEq.Equals(p.Value, value), notFound: notFound);
        public int LastIndexOf(V value, int notFound) => Dict.Pairs.LastIndexOf(p => valueEq.Equals(p.Value, value), notFound: notFound);

        public Found<V> Find(Predicate<V> predicate) => Dict.Pairs.FindElementPart(e => e.Value, predicate);

        public Found<V> FindFirst(Predicate<V> predicate, int fromIndex = 0) => Dict.Pairs.FindElementPart(e => e.Value, predicate, fromIndex: fromIndex);

        public Found<V> FindLast(Predicate<V>  predicate) => Dict.Pairs.FindLastElementPart(e => e.Value, predicate);

        public bool Contains(Predicate<V> predicate) => Find(predicate).Ok;

        public bool Contains(V item) => IndexOf(item) >= 0;

        public override IEnumerator<V> GetEnumerator() => new AdaptingEnumerator<KeyValuePair<K,V>,V>(Dict.GetEnumerator(), p => p.Value);
    }

}



internal sealed class ImmutableMiniDictionary<K,V> : ImmutableDictionary<K,V>
{
    protected override string DictionaryWord => "MiniDictionary";

    internal ImmutableMiniDictionary(KeyValuePair<K,V>[] entries) : base(entries) { }

    public override int IndexOfKey(K key, int notFound) =>
        Pairs.IndexOf(e => keyEq.Equals(e.Key, key), notFound: notFound);

    public override int LastIndexOfKey(K key, int notFound) =>
        Pairs.LastIndexOf(e => keyEq.Equals(e.Key, key), notFound: notFound);

    public override Found<V> Find(K key)
    {
        int index = IndexOfKey(key, notFoundIndex);
        return index >= 0
            ? new Found<V>(true, Pairs[index].Value)
            : Found<V>.NotFound;
    }

    public override bool ContainsKey(K key) => IndexOfKey(key) >= 0;

}



internal sealed class ImmutableHashDictionary<K,V> : ImmutableDictionary<K,V>
{
    protected override string DictionaryWord => "HashDictionary";

    private readonly HashTableEntry[] HashTable;

    internal ImmutableHashDictionary(KeyValuePair<K,V>[] pairs)
        : base(pairs)
    {
        BuildHashTable<KeyValuePair<K,V>,K>(Pairs, e => e.Key, keyEq, out HashTable);
    }

    public override int IndexOfKey(K key, int notFound)
    {
        return FindIndex<KeyValuePair<K,V>,K>(Pairs, HashTable, e => e.Key, keyEq, key, notFound);
    }

    public override int LastIndexOfKey(K key, int notFound)
    {
        return FindIndex<KeyValuePair<K,V>,K>(Pairs, HashTable, e => e.Key, keyEq, key, notFound);
    }

    public override Found<V> Find(K key)
    {
        int index = FindIndex<KeyValuePair<K,V>,K>(Pairs, HashTable, e => e.Key, keyEq, key, notFoundIndex);
        return index >= 0
            ? new Found<V>(true, Pairs[index].Value)
            : Found<V>.NotFound;
    }

    public override bool ContainsKey(K key)
    {
        return FindIndex<KeyValuePair<K,V>,K>(Pairs, HashTable, e => e.Key, keyEq, key, -1) >= 0;
    }
}



/// <summary>
/// Sorted dictionary.
/// </summary>
internal sealed class ImmutableSortedDictionary<K,V> : ImmutableDictionary<K,V>, ImmSortedDict<K,V>
    where K : IComparable<K>
{
    protected override string DictionaryWord => "SortedDictionary";

    /// <summary>
    /// Trivial constructor.
    /// </summary>
    /// <param name="pairs">the array of pairs, the pairs should be already sorted and deduplicated.</param>
    public ImmutableSortedDictionary(KeyValuePair<K,V>[] pairs) : base(pairs) { }

    public K MinKey => Pairs[0].Key;
    public K MaxKey => Pairs[^1].Key;

    public override int IndexOfKey(K key, int notFound)
    {
        int result = Pairs.BinarySearchByPart(key, e => e.Key, Comparer<K>.Default);
        return result >= 0 ? result : notFound;
    }

    public override int LastIndexOfKey(K key, int notFound)
        => IndexOfKey(key, notFound);

    public override bool ContainsKey(K key)
        => IndexOfKey(key) >= 0;

    public override Found<V> Find(K key)
    {
        int index = IndexOfKey(key);
        return index >= 0 ? new Found<V>(true, Pairs[index].Value) : Found<V>.NotFound;
    }
}
