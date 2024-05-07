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

    protected ImmArrayDictionary(KeyValuePair<K, V>[] array, bool toCopy)
    {
        N = array.Length;

        if (toCopy)
        {
            EntriesArray = new KeyValuePair<K, V> [N];
            Array.Copy(array, EntriesArray, N);
        }
        else
        {
            EntriesArray = array;
        }

        Offset = 0;
        Limit  = N;
        Exact  = true;

        EntriesSegment = new ArraySegment<KeyValuePair<K, V>>(EntriesArray);
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

    /// <summary>
    /// Finds an index of the entry in the <see cref="EntriesArray"/>.
    /// The index should be between <see cref="Offset"/> and <see cref="Limit"/>.
    /// </summary>
    /// <param name="key">a key to find the index for.</param>
    /// <returns>found index, or <see cref="int.MinValue"/> when the key is not found.</returns>
    protected abstract int FindEntryIndex(K key);

    public override bool ContainsKey(K key) => FindEntryIndex(key) >= 0;

    public override Found<V> Find(K key)
    {
        int index = FindEntryIndex(key);
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
        int index = FindEntryIndex(key);
        return index >= 0 ? EntriesArray[index].Value : noValue;
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
