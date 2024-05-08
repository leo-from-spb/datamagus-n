using System;
using System.Collections.Generic;

namespace Util.Collections;


public class ImmEmptyDictionary<K,V> : ImmDictionary<K,V>, ROrderDictionary<K,V>
{
    /// <summary>
    /// The empty dictionary.
    /// </summary>
    public static readonly ImmEmptyDictionary<K,V> Empty = new ImmEmptyDictionary<K,V>();

    #nullable disable
    private static readonly Found<V> NothingFound = new Found<V>(false, default(V));
    #nullable restore


    /// <summary>
    /// Instantiates an empty dictionary.
    /// Don't use this constructor directly, use <see cref="Empty"/> instead.
    /// </summary>
    internal ImmEmptyDictionary() {}


    public KeyValuePair<K, V> At(int index) =>
        throw new IndexOutOfRangeException($"Attempted tp get by index {index} from an empty dictionary");

    public override Found<V> Find(K key) => NothingFound;

    public override V Get(K key,
                          #nullable disable
                          V noValue = default(V)
                          #nullable restore
        ) => noValue;

    public override bool ContainsKey(K key) => false;
    public override bool IsNotEmpty         => false;
    public override bool IsEmpty            => true;
    public override int  Count              => 0;

    public override IReadOnlyCollection<K>                 Keys    => Array.Empty<K>();
    public override IReadOnlyCollection<V>                 Values  => Array.Empty<V>();
    public override IReadOnlyCollection<KeyValuePair<K,V>> Entries => Array.Empty<KeyValuePair<K,V>>();

    public override IEnumerator<KeyValuePair<K,V>> GetEnumerator() => Entries.GetEnumerator();

}
