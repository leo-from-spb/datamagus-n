using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Shouldly;
using Util.Extensions;

namespace Testing.Appliance.Assertions;


/// <summary>
/// More useful assertion methods.
/// </summary>
[ShouldlyMethods]
public static class Assertions
{

    /// <summary>
    /// Asserts that the array contains all of the specified expected items.
    /// Reports which items are missing if the assertion fails.
    /// </summary>
    /// <param name="array">the actual array to check.</param>
    /// <param name="expectedItems">items that must all be present in the array.</param>
    /// <typeparam name="E">the element type.</typeparam>
    public static void ShouldContainAll<E>(this E[]? array, params E[] expectedItems)
    {
        if (array is null) Fail($"Actual array is null when expect an array with the following items " + expectedItems.Describe());
        IReadOnlySet<E> set = array.IsNotEmpty() ? array.ToHashSet() : ImmutableSortedSet<E>.Empty;
        CheckContainsAll("array", set, expectedItems);
    }

    /// <summary>
    /// Asserts that the collection contains all of the specified expected items.
    /// Reports which items are missing if the assertion fails.
    /// </summary>
    /// <param name="collection">the actual collection to check.</param>
    /// <param name="expectedItems">items that must all be present in the collection.</param>
    /// <typeparam name="E">the element type.</typeparam>
    public static void ShouldContainAll<E>(this IReadOnlyCollection<E>? collection, params E[] expectedItems)
    {
        if (collection is null) Fail("Actual collection is null when expect a collection with the following items " + expectedItems.Describe());
        CheckContainsAll("collection", collection, expectedItems);
    }

    /// <summary>
    /// Asserts that the enumerable contains all of the specified expected items.
    /// Materializes the enumerable into a set for containment checks.
    /// Reports which items are missing if the assertion fails.
    /// </summary>
    /// <param name="enumerable">the actual enumerable to check.</param>
    /// <param name="expectedItems">items that must all be present in the enumerable.</param>
    /// <typeparam name="E">the element type.</typeparam>
    public static void ShouldContainAll<E>(this IEnumerable<E>? enumerable, params E[] expectedItems)
    {
        if (enumerable is null) Fail("Actual collection is null when expect a collection with the following items " + expectedItems.Describe());
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


    /// <summary>
    /// Asserts that this text contains the given string in the proper order.
    /// </summary>
    /// <param name="text">a text to verify.</param>
    /// <param name="expectedStrings">string that should be present in the text, in the order to find them in the text.</param>
    public static void ShouldContainInOrder(this string? text, params string[] expectedStrings)
    {
        int n = expectedStrings.Length;
        if (text is null)
        {
            Fail($"Expected text that contains {n} sub-strings but the text is null");
            return;
        }
        if (text.IsEmpty)
        {
            Fail($"Expected text that contains {n} sub-strings but the text is empty");
            return;
        }

        int[] positions = new int[n];
        int   lookFrom  = 0;
        int   failures  = 0;
        for (int i = 0; i < n; i++)
        {
            string es = expectedStrings[i];
            int    p  = text.IndexOf(es, lookFrom);
            positions[i] = p;
            if (p >= 0) lookFrom = p + es.Length;
            else failures++;
        }

        if (failures > 0)
        {
            var b = new StringBuilder();
            b.AppendLine("Expected text with sub-strings in the proper order:");
            for (int i = 0; i < n; i++)
            {
                b.Append('\t');
                b.AppendPad(i + 1, 4);
                int p1 = positions[i];
                var es = expectedStrings[i];
                int p2 = p1 + es.Length - 1;
                var pp = p1 >= 0 ? $"[{p1}..{p2}]" : "-missed-";
                b.AppendPad(pp, 14);
                b.Append('"').Append(es).Append('"').AppendLine();
            }
            b.AppendLine("Actual text:");
            b.AppendLine("------8<------");
            b.Append(text).EnsureEoln();
            b.AppendLine("------>8------");
            b.Append(failures).AppendLine(" sub-strings missed.");
            Fail(b.ToString());
        }
    }


    private static string Describe<E>(this E[] items)
    {
        if (items.Length == 0) return "Empty array of items";
        return items.JoinToString(func: i => $"\t{i}", separator: "\n");
    }

    [DoesNotReturn]
    private static void Fail(string message) =>
        throw new ShouldAssertException(message);

}
