using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Util.Collections;

/// <summary>
/// Base class for an array-based dictionary.
/// All entries are held in an array of key-value pairs.
/// </summary>
/// <typeparam name="K">type of key.</typeparam>
/// <typeparam name="V">type of value.</typeparam>
public abstract class ImmArrayDictionary<K,V> : ImmDictionary<K,V>
{
    /// <summary>
    /// Key-value pairs.
    /// This array can be "alien", so when using this array,
    /// the implementation must consider both the <see cref="Offset"/> and the <see cref="Limit"/>.
    /// Alternatively — the <see cref="EntriesSegment"/> is already adjusted.
    /// </summary>
    protected readonly KeyValuePair<K,V>[] EntriesArray;

    protected readonly ArraySegment<KeyValuePair<K,V>> EntriesSegment;

    protected readonly int Offset;

    protected readonly int Limit;

    protected readonly int N;

    protected readonly bool Exact;


    protected ImmArrayDictionary(KeyValuePair<K, V>[] array, int offset, int limit)
    {
        int arrayN = array.Length;
        Debug.Assert(offset < limit);
        Debug.Assert(limit <= arrayN);

        EntriesArray  = array;
        Offset = offset;
        Limit  = limit;
        N      = limit - offset;
        Exact  = offset == 0 && N == arrayN;

        EntriesSegment = Exact
            ? new ArraySegment<KeyValuePair<K, V>>(EntriesArray)
            : new ArraySegment<KeyValuePair<K, V>>(EntriesArray, Offset, N);
    }


    protected ImmArrayDictionary(int n)
    {
        Debug.Assert(n > 0);

        EntriesArray  = new KeyValuePair<K, V>[n];
        Offset = 0;
        Limit  = n;
        N      = n;
        Exact  = true;

        EntriesSegment = new ArraySegment<KeyValuePair<K, V>>(EntriesArray);
    }


    public override int  Count      => N;
    public override bool IsNotEmpty => true;
    public override bool IsEmpty    => false;

    public override IEnumerator<KeyValuePair<K, V>> GetEnumerator() => EntriesSegment.GetEnumerator();

    public override IReadOnlyCollection<KeyValuePair<K, V>> Entries => EntriesSegment;

    public override IReadOnlyCollection<K> Keys   => new KeySet(this);
    public override IReadOnlyCollection<V> Values => new ValueCollection(this);



    protected class KeySet : IReadOnlyCollection<K>
    {
        private readonly ImmArrayDictionary<K, V> D;

        internal KeySet(ImmArrayDictionary<K, V> dictionary) => D = dictionary;

        IEnumerator IEnumerable.GetEnumerator() => D.EntriesSegment.Select(e => e.Key).GetEnumerator();

        public IEnumerator<K> GetEnumerator() => D.EntriesSegment.Select(e => e.Key).GetEnumerator();

        public int Count => D.Count;

        public bool Contains(K item) => D.ContainsKey(item);
    }


    protected class ValueCollection : IReadOnlyCollection<V>
    {
        private readonly ImmArrayDictionary<K, V> D;

        internal ValueCollection(ImmArrayDictionary<K, V> dictionary) => D = dictionary;

        IEnumerator IEnumerable.GetEnumerator() => D.EntriesSegment.Select(e => e.Value).GetEnumerator();

        public IEnumerator<V> GetEnumerator() => D.EntriesSegment.Select(e => e.Value).GetEnumerator();

        public int Count => D.Count;
    }

}
