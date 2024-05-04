using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using BenchmarkDotNet.Attributes;

namespace Util.Collections;

[MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 5, iterationCount: 5, invocationCount:200, id: "QuickJob")]
public class ImmHashMapBenchmark
{
    private const int N = 4000;

    private const int Tries = 10000;

    private static ulong HashCounter;

    private static bool HashCounterReported_Compact, HashCounterReported_Dotnet;


    private readonly struct Hazke
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


    [Benchmark]
    public void Create_Basic_ImmCompactHashDictionary1()
    {
        new ImmCompactHashDictionary1<ulong,ulong>(BasicPairs);
    }

    [Benchmark]
    public void Create_Basic_ImmutableDictionary()
    {
        ImmutableDictionary.CreateRange(BasicPairs);
    }

    [Benchmark]
    public void Create_Basic_RegularDictionary()
    {
        new Dictionary<ulong,ulong>(BasicPairs);
    }


    [Benchmark]
    public void Create_Hazke_ImmCompactHashDictionary1()
    {
        HashCounter = 0uL;
        new ImmCompactHashDictionary1<Hazke,ulong>(HazkePairs);

        if (!HashCounterReported_Compact)
        {
            Console.WriteLine($"Create_Hazke_ImmCompactHashDictionary used hash {HashCounter} times.");
            HashCounterReported_Compact = true;
        }
    }

    [Benchmark]
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

    [Benchmark]
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


    [Benchmark]
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

    [Benchmark]
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

    [Benchmark]
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
