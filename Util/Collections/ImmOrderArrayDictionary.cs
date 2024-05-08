using System.Collections.Generic;

namespace Util.Collections;

public abstract class ImmOrderArrayDictionary<K,V> :ImmArrayDictionary<K,V>, ROrderDictionary<K,V>
{

    protected ImmOrderArrayDictionary(KeyValuePair<K, V>[] array, int offset, int limit) : base(array, offset, limit) { }

    protected ImmOrderArrayDictionary(KeyValuePair<K, V>[] array, bool toCopy) : base(array, toCopy) { }

    protected ImmOrderArrayDictionary(int n) : base(n) { }


    public KeyValuePair<K,V> At(int index) => EntriesArray[index];

}
