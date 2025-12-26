using System;
using System.Collections.Generic;
using System.Linq;
using Util.Fun;
using static System.Math;

namespace Util.Collections.Implementation;


internal sealed class ImmutablePatchedDict<K,V> : ImmutableDictionary<K,V>, ImmDict<K,V>
{
    protected override string DictionaryWord => "Patched Dictionary";

    public ImmutableDictionary<K,V> Imp => this;

    private readonly ImmDict<K,V> Origin;
    private readonly ImmDict<K,V> Patch;
    private readonly ImmSet<K>    Removed;

    internal override byte CascadingLevel { get; }
    public override   int  Count          { get; }

    public ImmutablePatchedDict(ImmDict<K,V> origin, ImmDict<K,V> patch, ImmSet<K> removed)
    {
        Origin  = origin;
        Patch   = patch;
        Removed = removed;

        CascadingLevel = Max(Max(Origin.Imp.CascadingLevel, Patch.Imp.CascadingLevel), Removed.Imp.CascadingLevel).Succ;

        int replacedCnt = Patch.Keys.Count(k => Origin.ContainsKey(k));
        int newCnt = Patch.Count - replacedCnt;
        int deletedCnt = Removed.Count(k => Origin.ContainsKey(k));
        Count = Origin.Count + newCnt - deletedCnt;
    }

    public bool IsNotEmpty => Count != 0;
    public bool IsEmpty    => Count == 0;

    public bool ContainsKey(K key) => Patch.ContainsKey(key)
                                   || Origin.ContainsKey(key) && !Removed.Contains(key);

    public Found<V> Find(K key)
    {
        var f = Patch.Find(key);
        if (f.Ok) return f;

        f = Origin.Find(key);
        if (f.Ok && Removed.Contains(key))
            f = Found<V>.NotFound;
        return f;
    }

    public ImmSet<K>                        Keys    => new KeySet(this);
    public ImmCollection<V>                 Values  => new ValueCollection(this);
    public ImmCollection<KeyValuePair<K,V>> Entries => new EntryCollection(this);

    internal IEnumerable<KeyValuePair<K,V>> EnumerateEntries()
    {
        IEnumerable<KeyValuePair<K, V>> q1 = Origin.Entries.Select(e => AdjustEntry(e)).Where(f => f.Ok).Select(f => f.Item);
        IEnumerable<KeyValuePair<K,V>>  q2 = Patch.Entries.Where(e => !Origin.ContainsKey(e.Key));
        return q1.Concat(q2);
    }

    private Found<KeyValuePair<K,V>> AdjustEntry(KeyValuePair<K,V> originalEntry)
    {
        var p = Patch.Find(originalEntry.Key);
        if (p.Ok) return new Found<KeyValuePair<K,V>>(true, new KeyValuePair<K,V>(originalEntry.Key, p.Item));
        if (Removed.Contains(originalEntry.Key)) return Found<KeyValuePair<K,V>>.NotFound;
        return new Found<KeyValuePair<K,V>>(true, originalEntry);
    }

    /// <summary>
    /// Makes a copy of this view that doesn't reference this instance or its delegates.
    /// </summary>
    /// <returns>the copy.</returns>
    public ImmListDict<K,V> Repack()
    {
        var entries = EnumerateEntries().ToArray();
        return ImmutableArrayDictionary<K,V>.MakeListDict(entries, false);
    }



    public override string ToString() =>
        $"{base.ToString()} (origin {Origin.Count}, patch {Patch.Count}, removed {Removed.Count})";


    // INNER CLASSES \\


    private sealed class KeySet : ImmutableCollection<K>, ImmSet<K>
    {
        private readonly ImmutablePatchedDict<K,V> Dict;

        protected override string CollectionWord => "Patched KeySet";
        internal override  byte   CascadingLevel => Dict.CascadingLevel.Succ;

        public KeySet(ImmutablePatchedDict<K,V> dict)
        {
            Dict = dict;
        }

        public override int  Count      => Dict.Count;
        public          bool IsNotEmpty => Dict.IsNotEmpty;
        public          bool IsEmpty    => Dict.IsEmpty;

        public bool Contains(K item) => Dict.ContainsKey(item);

        public bool Contains(Predicate<K> predicate)
        {
            return Dict.Patch.Keys.Contains(predicate)
                || Dict.Origin.Keys.Any(k => predicate(k) && !Dict.Removed.Contains(k));
        }

        public Found<K> Find(Predicate<K> predicate)
        {
            var f = Dict.Patch.Keys.Find(predicate);
            if (f.Ok) return f;
            return Dict.Origin.Keys.Find(k => predicate(k) && !Dict.Removed.Contains(k));
        }

        public override IEnumerator<K> GetEnumerator()
        {
            IEnumerable<K> q1 = Dict.Origin.Keys.Where(k => !Dict.Removed.Contains(k));
            IEnumerable<K> q2 = Dict.Patch.Keys.Where(k => !Dict.Origin.ContainsKey(k));
            return q1.Concat(q2).GetEnumerator();
        }


        public bool IsProperSubsetOf(IEnumerable<K>   other)
        {
            throw new NotImplementedException();
        }
        public bool IsProperSupersetOf(IEnumerable<K> other)
        {
            throw new NotImplementedException();
        }
        public bool IsSubsetOf(IEnumerable<K>   other)
        {
            throw new NotImplementedException();
        }
        public bool IsSupersetOf(IEnumerable<K> other)
        {
            throw new NotImplementedException();
        }
        public bool Overlaps(IEnumerable<K>  other)
        {
            throw new NotImplementedException();
        }
        public bool SetEquals(IEnumerable<K> other)
        {
            throw new NotImplementedException();
        }
    }


    private sealed class ValueCollection : ImmutableCollection<V>, ImmCollection<V>
    {
        private readonly ImmutablePatchedDict<K,V> Dict;

        protected override string CollectionWord => "Patched Values";
        internal override  byte   CascadingLevel => Dict.CascadingLevel.Succ;

        public ValueCollection(ImmutablePatchedDict<K,V> dict)
        {
            Dict = dict;
        }

        public override int  Count      => Dict.Count;
        public          bool IsNotEmpty => Dict.IsNotEmpty;
        public          bool IsEmpty    => Dict.IsEmpty;

        public override IEnumerator<V> GetEnumerator() =>
            Dict.EnumerateEntries()
                .Select(e => e.Value)
                .GetEnumerator();

        public bool Contains(Predicate<V> predicate) =>
            Dict.EnumerateEntries()
                .Any(e => predicate(e.Value));

        public Found<V> Find(Predicate<V> predicate) =>
            Dict.EnumerateEntries()
                .Where(e => predicate(e.Value))
                .Select(e => new Found<V>(true, e.Value))
                .FirstOrDefault(Found<V>.NotFound);
    }


    private sealed class EntryCollection : ImmutableCollection<KeyValuePair<K,V>>, ImmCollection<KeyValuePair<K,V>>
    {
        private readonly ImmutablePatchedDict<K,V> Dict;

        protected override string CollectionWord => "Patched Entries";
        internal override  byte   CascadingLevel => Dict.CascadingLevel.Succ;

        public EntryCollection(ImmutablePatchedDict<K,V> dict)
        {
            Dict = dict;
        }

        public override int  Count      => Dict.Count;
        public          bool IsNotEmpty => Dict.IsNotEmpty;
        public          bool IsEmpty    => Dict.IsEmpty;

        public override IEnumerator<KeyValuePair<K,V>> GetEnumerator() =>
            Dict.EnumerateEntries()
                .GetEnumerator();

        public bool Contains(Predicate<KeyValuePair<K,V>> predicate)
        {
            return Dict.Patch.Entries.Contains(predicate)
                || Dict.Origin.Entries.Any(e => predicate(e) && !Dict.Removed.Contains(e.Key));
        }

        public Found<KeyValuePair<K,V>> Find(Predicate<KeyValuePair<K,V>> predicate)
        {
            var f = Dict.Patch.Entries.Find(predicate);
            if (f.Ok) return f;
            return Dict.Origin.Entries.Find(e => predicate(e) && !Dict.Removed.Contains(e.Key));
        }

    }

}
