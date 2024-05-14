using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Util.Collections;

[MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 5, iterationCount: 5, invocationCount:200, id: "QuickJob")]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory), CategoriesColumn]
public class ImmHashMapBenchmark
{
    private const int N = 4000;

    private const int Tries = 10000;

    private static ulong HashCounter;

    private static bool HashCounterReported_Compact, HashCounterReported_Dotnet;


    private readonly record struct Hazke
    {
        public readonly ulong X;

        public Hazke(ulong x) => X = x;

        public override int GetHashCode()
        {
            Interlocked.Increment(ref HashCounter);
            return X.GetHashCode();
        }
    }

    private readonly KeyValuePair<ulong,ulong>[] BasicPairs = new KeyValuePair<ulong,ulong>[N];

    private readonly KeyValuePair<Hazke,ulong>[] HazkePairs = new KeyValuePair<Hazke,ulong>[N];



    [GlobalSetup]
    public void PrepareStuff()
    {
        Random rnd = new Random();

        // Basic
        for (uint i = 0; i < N; i++)
        {
            ulong k    = (ulong)rnd.Next(1000) * 1000000uL + i;
            ulong v    = (ulong)rnd.NextInt64(100000000000);
            var   pair = new KeyValuePair<ulong, ulong>(k, v);
            BasicPairs[i] = pair;
        }

        // Hash counting
        for (uint i = 0; i < N; i++)
        {
            Hazke k    = new Hazke((ulong)rnd.Next(1000) * 1000000uL + i);
            ulong v    = (ulong)rnd.NextInt64(100000000000);
            var   pair = new KeyValuePair<Hazke, ulong>(k, v);
            HazkePairs[i] = pair;
        }
    }


    [BenchmarkCategory("Create_Basic"), Benchmark(Baseline = true)]
    public void Create_Basic_ImmCompactHashDictionary1()
    {
        new ImmCompactHashDictionary1<ulong,ulong>(BasicPairs);
    }

    [BenchmarkCategory("Create_Basic"), Benchmark]
    public void Create_Basic_ImmCompactHashDictionary()
    {
        new ImmCompactHashDictionary<ulong,ulong>(BasicPairs);
    }

    [BenchmarkCategory("Create_Basic"), Benchmark]
    public void Create_Basic_ImmStableHashDictionary()
    {
        new ImmStableHashDictionary<ulong,ulong>(BasicPairs);
    }

    [BenchmarkCategory("Create_Basic"), Benchmark]
    public void Create_Basic_ImmutableDictionary()
    {
        ImmutableDictionary.CreateRange(BasicPairs);
    }

    [BenchmarkCategory("Create_Basic"), Benchmark]
    public void Create_Basic_RegularDictionary()
    {
        new Dictionary<ulong,ulong>(BasicPairs);
    }


    [BenchmarkCategory("Create_Hazke"), Benchmark(Baseline = true)]
    public void Create_Hazke_ImmCompactHashDictionary1()
    {
        HashCounter = 0uL;
        new ImmCompactHashDictionary1<Hazke,ulong>(HazkePairs);

        if (!HashCounterReported_Compact)
        {
            Console.WriteLine($"Create_Hazke_ImmCompactHashDictionary1 used hash {HashCounter} times.");
            HashCounterReported_Compact = true;
        }
    }

    [BenchmarkCategory("Create_Hazke"), Benchmark]
    public void Create_Hazke_ImmCompactHashDictionary()
    {
        HashCounter = 0uL;
        new ImmCompactHashDictionary<Hazke,ulong>(HazkePairs);

        if (!HashCounterReported_Compact)
        {
            Console.WriteLine($"Create_Hazke_ImmCompactHashDictionary used hash {HashCounter} times.");
            HashCounterReported_Compact = true;
        }
    }

    [BenchmarkCategory("Create_Hazke"), Benchmark]
    public void Create_Hazke_ImmStableHashDictionary()
    {
        HashCounter = 0uL;
        new ImmStableHashDictionary<Hazke,ulong>(HazkePairs);

        if (!HashCounterReported_Compact)
        {
            Console.WriteLine($"Create_Hazke_ImmStableHashDictionary used hash {HashCounter} times.");
            HashCounterReported_Compact = true;
        }
    }

    [BenchmarkCategory("Create_Hazke"), Benchmark]
    public void Create_Hazke_ImmutableDictionary()
    {
        HashCounter = 0uL;
        ImmutableDictionary.CreateRange(HazkePairs);

        if (!HashCounterReported_Dotnet)
        {
            Console.WriteLine($"Create_Hazke_ImmutableDictionary used hash {HashCounter} times.");
            HashCounterReported_Dotnet = true;
        }
    }

    [BenchmarkCategory("Create_Hazke"), Benchmark]
    public void Create_Hazke_RegularDictionary()
    {
        HashCounter = 0uL;
        new Dictionary<Hazke,ulong>(HazkePairs);

        if (!HashCounterReported_Dotnet)
        {
            Console.WriteLine($"Create_Hazke_RegularDictionary used hash {HashCounter} times.");
            HashCounterReported_Dotnet = true;
        }
    }


    [BenchmarkCategory("General"), Benchmark(Baseline = true)]
    public void General_Basic_ImmCompactHashDictionary1()
    {
        Random rnd = new Random();

        var dictionary = new ImmCompactHashDictionary1<ulong,ulong>(BasicPairs);

        for (int i = 0; i < Tries; i++)
        {
            ulong key = (i & 1) == 0 ? BasicPairs[i % N].Key : (ulong)rnd.NextInt64(10001000000);
            dictionary.TryGetValue(key, out ulong value);
        }
    }

    [BenchmarkCategory("General"), Benchmark]
    public void General_Basic_ImmCompactHashDictionary()
    {
        Random rnd = new Random();

        var dictionary = new ImmCompactHashDictionary<ulong,ulong>(BasicPairs);

        for (int i = 0; i < Tries; i++)
        {
            ulong key = (i & 1) == 0 ? BasicPairs[i % N].Key : (ulong)rnd.NextInt64(10001000000);
            dictionary.Find(key);
        }
    }

    [BenchmarkCategory("General"), Benchmark]
    public void General_Basic_ImmStableHashDictionary()
    {
        Random rnd = new Random();

        var dictionary = new ImmStableHashDictionary<ulong,ulong>(BasicPairs);

        for (int i = 0; i < Tries; i++)
        {
            ulong key = (i & 1) == 0 ? BasicPairs[i % N].Key : (ulong)rnd.NextInt64(10001000000);
            dictionary.Find(key);
        }
    }

    [BenchmarkCategory("General"), Benchmark]
    public void General_Basic_ImmutableDictionary()
    {
        Random rnd = new Random();

        var dictionary = ImmutableDictionary.CreateRange(BasicPairs);

        for (int i = 0; i < Tries; i++)
        {
            ulong key = (i & 1) == 0 ? BasicPairs[i % N].Key : (ulong)rnd.NextInt64(10001000000);
            dictionary.TryGetValue(key, out ulong value);
        }
    }

    [BenchmarkCategory("General"), Benchmark]
    public void General_Basic_RegularDictionary()
    {
        Random rnd = new Random();

        var dictionary = new Dictionary<ulong,ulong>(BasicPairs);

        for (int i = 0; i < Tries; i++)
        {
            ulong key = (i & 1) == 0 ? BasicPairs[i % N].Key : (ulong)rnd.NextInt64(10001000000);
            dictionary.TryGetValue(key, out ulong value);
        }
    }


}