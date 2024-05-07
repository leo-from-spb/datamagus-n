using System;
using System.Collections.Generic;
using System.Linq;
using Testing.Appliance.Assertions;

namespace Util.Collections;


public abstract class ImmHashDictionaryTest
{

    [TestFixture]
    public sealed class ImmMicroDictionaryTest : ImmHashDictionaryTest
    {
        protected override ImmDictionary<K,V> MakeDictionary<K,V>(KeyValuePair<K,V>[] entries)
        {
            return new ImmMicroDictionary<K,V>(entries);
        }

    }

    [TestFixture]
    public sealed class ImmCompactHashDictionaryTest : ImmHashDictionaryTest
    {
        protected override ImmDictionary<K,V> MakeDictionary<K,V>(KeyValuePair<K,V>[] entries)
        {
            return new ImmCompactHashDictionary<K,V>(entries);
        }

    }

    [TestFixture]
    public sealed class ImmStableHashDictionaryTest : ImmHashDictionaryTest
    {
        protected override ImmDictionary<K,V> MakeDictionary<K,V>(KeyValuePair<K,V>[] entries)
        {
            return new ImmStableHashDictionary<K,V>(entries);
        }

    }


    protected abstract ImmDictionary<K, V> MakeDictionary<K, V>(KeyValuePair<K, V>[] entries);


    [Test]
    public void Basic_1()
    {
        const long k = 1234567890L;
        const long v = 9876543210L;

        var entries    = new KeyValuePair<long, long>[] { kvp<long, long>(k, v) };
        var dictionary = MakeDictionary(entries);

        var f = dictionary.Find(k);
        f.Ok.ShouldBeTrue();
        f.Item.ShouldBe(v);
        var x = dictionary[k];
        x.ShouldBe(v);

        dictionary.ContainsKey(k).ShouldBeTrue();
        dictionary.ContainsKey(v).ShouldBeFalse();
        dictionary.ContainsKey(0L).ShouldBeFalse();
    }

    [Test]
    public void Basic_7()
    {
        var entries = new KeyValuePair<long, long>[7];

        var rnd = new Random();
        for (int i = 0; i < 7; i++)
        {
            long k     = rnd.Next(100000000) * 10L + i;
            long v     = rnd.NextInt64(100000000000);
            var  entry = kvp<long,long>(k, v);
            entries[i] = entry;
        }

        var dictionary = MakeDictionary(entries);

        dictionary.Verify
        (
            d => d[entries[0].Key].ShouldBe(entries[0].Value),
            d => d[entries[1].Key].ShouldBe(entries[1].Value),
            d => d[entries[2].Key].ShouldBe(entries[2].Value),
            d => d[entries[3].Key].ShouldBe(entries[3].Value),
            d => d[entries[4].Key].ShouldBe(entries[4].Value),
            d => d[entries[5].Key].ShouldBe(entries[5].Value),
            d => d[entries[6].Key].ShouldBe(entries[6].Value),
            d => d.ContainsKey(entries[0].Key).ShouldBeTrue(),
            d => d.ContainsKey(entries[1].Key).ShouldBeTrue(),
            d => d.ContainsKey(entries[2].Key).ShouldBeTrue(),
            d => d.ContainsKey(entries[3].Key).ShouldBeTrue(),
            d => d.ContainsKey(entries[4].Key).ShouldBeTrue(),
            d => d.ContainsKey(entries[5].Key).ShouldBeTrue(),
            d => d.ContainsKey(entries[6].Key).ShouldBeTrue(),
            d => d.ContainsKey(long.MinValue).ShouldBeFalse(),
            d => d.ContainsKey(long.MaxValue).ShouldBeFalse(),
            d => d.ContainsKey(0L).ShouldBeFalse()
        );
    }

    [Test]
    public void Basic_256()
    {
        int n       = 256;
        var entries = new KeyValuePair<long, long>[n];

        var rnd = new Random();
        for (int i = 0; i < n; i++)
        {
            long k     = rnd.Next(10000000) * 1000L + i;
            long v     = rnd.NextInt64(100000000000);
            var  entry = kvp<long,long>(k, v);
            entries[i] = entry;
        }

        var dictionary = MakeDictionary(entries);

        foreach (var e in entries)
        {
            var f = dictionary.Find(e.Key);
            f.Ok.ShouldBeTrue();
            f.Item.ShouldBe(e.Value);
        }
    }


    [Test]
    public void NullableKeyNullableValue()
    {
        var entries = new KeyValuePair<string?, string?>[]
                      {
                          new("the key", "the value"),
                          new("mura", null),
                          new(null, "null entry")
                      };

        var dictionary = MakeDictionary(entries);

        dictionary.Verify
        (
            d => d.Get("the key").ShouldBe("the value"),
            d => d.Get("mura").ShouldBeNull(),
            d => d.Get(null).ShouldBe("null entry"),
            d => d.Get("labuda", "nothing").ShouldBe("nothing")
        );
    }


    [Test]
    public void Keys()
    {
        var dictionary = prepareSimpleDictionary3();
        dictionary.Keys.Verify
        (
            ks => ks.ShouldContainAll(1001uL, 2002ul, 3003ul),
            ks => ks.ToArray().ShouldContainAll(1001uL, 2002ul, 3003ul),
            ks => ks.Count.ShouldBe(3)
        );
    }

    [Test]
    public void Values()
    {
        var dictionary = prepareSimpleDictionary3();
        dictionary.Values.Verify
        (
            vs => vs.ShouldContainAll("einz", "zwei", "drei"),
            vs => vs.ToArray().ShouldContainAll("einz", "zwei", "drei"),
            vs => vs.Count.ShouldBe(3)
        );
    }

    [Test]
    public void Entries()
    {
        var dictionary = prepareSimpleDictionary3();
        dictionary.Entries.Verify
        (
            entries => entries.ShouldContainAll(kvp(1001uL, "einz"), kvp(2002uL, "zwei"), kvp(3003uL, "drei")),
            entries => entries.ToArray().ShouldContainAll(kvp(1001uL, "einz"), kvp(2002uL, "zwei"), kvp(3003uL, "drei")),
            entries => entries.Count.ShouldBe(3)
        );
    }

    private ImmDictionary<ulong, string> prepareSimpleDictionary3()
    {
        var entries    = new[] { kvp(1001uL, "einz"), kvp(2002uL, "zwei"), kvp(3003uL, "drei") };
        var dictionary = MakeDictionary(entries);
        return dictionary;
    }


    private static KeyValuePair<K,V> kvp<K,V>(K key, V value) => new KeyValuePair<K,V>(key, value);
}
