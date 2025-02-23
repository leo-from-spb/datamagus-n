using System.Collections.Generic;

namespace Util.Collections.Implementation;

/// <summary>
/// Empty dictionary.
/// </summary>
public sealed class EmptyDictionary<K,V> : ImmDict<K,V>
{
    public static readonly EmptyDictionary<K,V> Instance = new();

    public bool IsNotEmpty => false;
    public bool IsEmpty    => true;
    public int  Count      => 0;

    public bool ContainsKey(K key) => false;

    public Found<V> Find(K key) => Found<V>.NotFound;

    public ImmSet<K>        Keys   => EmptySet<K>.Instance;
    public ImmCollection<V> Values => EmptySet<V>.Instance;

    public ImmCollection<KeyValuePair<K,V>> Entries => EmptySet<KeyValuePair<K,V>>.Instance;
}
