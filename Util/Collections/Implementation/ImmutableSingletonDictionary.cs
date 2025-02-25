using System;
using System.Collections.Generic;

namespace Util.Collections.Implementation;



internal class ImmutableSingletonDictionary<K,V> : Collections.ImmutableDictionary<K,V>, ImmListDict<K,V>
{
    protected readonly K Key;
    protected readonly V Value;

    internal ImmutableSingletonDictionary(K key, V value)
    {
        Key   = key;
        Value = value;
    }

    internal ImmutableSingletonDictionary(KeyValuePair<K, V> pair)
    {
        this.Key   = pair.Key;
        this.Value = pair.Value;
    }

    public bool IsNotEmpty => true;
    public bool IsEmpty    => false;
    public int  Count      => 1;

    public KeyValuePair<K, V> FirstEntry => new KeyValuePair<K,V>(Key, Value);
    public KeyValuePair<K, V> LastEntry  => new KeyValuePair<K,V>(Key, Value);

    public bool ContainsKey(K key) => keyEq.Equals(key, Key);

    public Found<V> Find(K key) =>
        keyEq.Equals(key, Key)
            ? new Found<V>(true, Value)
            : Found<V>.NotFound;

    public ImmListSet<K> Keys => new ImmutableSingleton<K>(Key);
    public ImmList<V> Values => new ImmutableSingleton<V>(Value);

    public ImmList<KeyValuePair<K,V>> Entries =>
        new ImmutableSingleton<KeyValuePair<K,V>>(new KeyValuePair<K,V>(Key, Value));
}



internal class ImmutableSingletonSortedDictionary<K,V> : ImmutableSingletonDictionary<K,V>, ImmSortedDict<K,V>
    where K : IComparable<K>
{
    internal ImmutableSingletonSortedDictionary(K key, V value) : base(key, value) { }

    internal ImmutableSingletonSortedDictionary(KeyValuePair<K, V> pair) : base(pair) { }

    public K MinKey => Key;
    public K MaxKey => Key;
}