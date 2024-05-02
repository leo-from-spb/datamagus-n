using System;
using System.Collections.Generic;

namespace Util.Collections;

[TestFixture]
public class ImmCompactHashDictionaryTest
{

    [Test]
    public void Basic_1()
    {
        const long k = 1234567890L;
        const long v = 9876543210L;

        var entries    = new KeyValuePair<long, long>[] { new KeyValuePair<long, long>(k, v) };
        var dictionary = new ImmCompactHashDictionary<long,long>(entries);

        long x = dictionary.Get(k);
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
            var  entry = new KeyValuePair<long,long>(k, v);
            entries[i] = entry;
        }

        var dictionary = new ImmCompactHashDictionary<long,long>(entries);

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
    public void Basic_3000()
    {
        int n       = 3000;
        var entries = new KeyValuePair<long, long>[n];

        var rnd = new Random();
        for (int i = 0; i < n; i++)
        {
            long k     = rnd.Next(10000000) * 1000L + i;
            long v     = rnd.NextInt64(100000000000);
            var  entry = new KeyValuePair<long,long>(k, v);
            entries[i] = entry;
        }

        var dictionary = new ImmCompactHashDictionary<long,long>(entries);

        foreach (var e in entries)
        {
            long x = dictionary.Get(e.Key);
            x.ShouldBe(e.Value);
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

        var dictionary = new ImmCompactHashDictionary<string?,string?>(entries);

        dictionary.Verify
        (
            d => d.Get("the key").ShouldBe("the value"),
            d => d.Get("mura").ShouldBeNull(),
            d => d.Get(null).ShouldBe("null entry"),
            d => d.Get("labuda", "nothing").ShouldBe("nothing")
        );
    }
}
