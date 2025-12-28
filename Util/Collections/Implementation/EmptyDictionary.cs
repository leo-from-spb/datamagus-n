using System;
using System.Collections.Generic;

namespace Util.Collections.Implementation;

/// <summary>
/// Empty dictionary.
/// </summary>
public class EmptyDictionary<K,V> : ImmutableDictionary<K,V>, ImmListDict<K,V>
{
    public ImmutableDictionary<K,V> Imp => this;

    internal override  byte   CascadingLevel => 0;
    protected override string DictionaryWord => "Empty Dictionary";

    public static readonly EmptyDictionary<K,V> Instance = new();

    public          bool Any => false;
    public          bool IsEmpty    => true;
    public override int  Count      => 0;

    public KeyValuePair<K,V> FirstEntry => throw new IndexOutOfRangeException("The dictionary is empty");
    public KeyValuePair<K,V> LastEntry  => throw new IndexOutOfRangeException("The dictionary is empty");

    public bool ContainsKey(K key) => false;

    public Found<V> Find(K key) => Found<V>.NotFound;

    public ImmListSet<K> Keys => EmptySet<K>.Instance;
    public ImmList<V> Values  => EmptySet<V>.Instance;

    public ImmList<KeyValuePair<K,V>> Entries => EmptySet<KeyValuePair<K,V>>.Instance;
}


public sealed class EmptySortedDictionary<K,V> : EmptyDictionary<K,V>, ImmSortedDict<K,V>
    where K : IComparable<K>
{
    public new static readonly EmptySortedDictionary<K,V> Instance = new();

    public K MinKey => throw new IndexOutOfRangeException("This dictionary is empty.");
    public K MaxKey => throw new IndexOutOfRangeException("This dictionary is empty.");
}

