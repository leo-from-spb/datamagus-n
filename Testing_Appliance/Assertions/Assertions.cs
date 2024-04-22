using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Shouldly;
using Util.Extensions;

namespace Testing.Appliance.Assertions;


[ShouldlyMethods]
public static class Assertions
{

    public static void ShouldContainAll<E>(this E[]? array, params E[] expectedItems)
    {
        if (array is null) Fail($"Actual array is null when expect an array with the following items " + expectedItems.Describe());
        IReadOnlySet<E> set = array.IsNotEmpty() ? array.ToHashSet() : ImmutableSortedSet<E>.Empty;
        CheckContainsAll("array", set, expectedItems);
    }

    public static void ShouldContainAll<E>(this IReadOnlyCollection<E>? collection, params E[] expectedItems)
    {
        if (collection is null) Fail($"Actual collection is null when expect a collection with the following items " + expectedItems.Describe());
        CheckContainsAll("collection", collection, expectedItems);
    }

    public static void ShouldContainAll<E>(this IEnumerable<E>? enumerable, params E[] expectedItems)
    {
        if (enumerable is null) Fail($"Actual collection is null when expect a collection with the following items " + expectedItems.Describe());
        var set = enumerable.ToHashSet();
        CheckContainsAll("collection", set, expectedItems);
    }

    private static void CheckContainsAll<E>(string collectionWord, IReadOnlyCollection<E>? collection, E[] expectedItems)
    {
        if (collection is null) Fail($"Actual {collectionWord} is null when expect a {collectionWord} with the following items: " + expectedItems.Describe());
        if (collection.IsEmpty()) Fail($"Actual {collectionWord} is empty when expect a {collectionWord} with the following items: " + expectedItems.Describe());

        var actualItems = collection is IReadOnlySet<E> ? collection : collection.ToHashSet();
        var missedItems = expectedItems.Where(item => !actualItems.Contains(item)).ToList();

        if (missedItems.IsNotEmpty())
        {
            var message =
                $"Expected a {collectionWord} containing {expectedItems.Length} specified items\n"
              + $"but got one ({collection.GetType().Name}) that contains {collection.Count} items that misses {missedItems.Count} of expected ones.\n"
              + $"-------- Given items ----------\n"
              + $"{collection.JoinToString(func: i => $"\t{i}", separator: "\n")}\n"
              + $"-------- Expected items -------\n"
              + $"{expectedItems.JoinToString(func: i => $"\t{i.IsIncludedAsChar(actualItems)} {i}", separator: "\n")}\n"
              + $"-------------------------------";
            Fail(message);
        }
    }

    private static char IsIncludedAsChar<E>(this E item, IReadOnlyCollection<E> collection) =>
        collection.Contains(item) ? '+' : '-';


    private static string Describe<E>(this E[] items)
    {
        if (items.Length == 0) return "Empty array of items";
        return items.JoinToString(func: i => $"\t{i}", separator: "\n");
    }

    [DoesNotReturn]
    private static void Fail(string message) =>
        throw new ShouldAssertException(message);

}
