using System.Collections.Generic;

namespace Util.Collections.Implementation;



internal class ImmutableSingletonDictionary<K,V> : ImmutableDictionary<K,V>, ImmDict<K,V>
{
    private readonly K Key;
    private readonly V Value;

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

    public bool ContainsKey(K key) => keyEq.Equals(key, Key);

    public Found<V> Find(K key) =>
        keyEq.Equals(key, Key)
            ? new Found<V>(true, Value)
            : Found<V>.NotFound;

    public ImmSet<K> Keys => new ImmutableSingleton<K>(Key);
    public ImmCollection<V> Values => new ImmutableSingleton<V>(Value);

    public ImmCollection<KeyValuePair<K,V>> Entries =>
        new ImmutableSingleton<KeyValuePair<K,V>>(new KeyValuePair<K,V>(Key, Value));
}
