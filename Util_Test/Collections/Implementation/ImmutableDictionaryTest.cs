using System.Collections.Generic;
using System.Linq;
using Testing.Appliance.Assertions;

namespace Util.Collections.Implementation;


using StringLongPair = KeyValuePair<string, long>;
using UintStringPair = KeyValuePair<uint, string>;


[TestFixture]
public class ImmutableDictionaryTest
{
    #region COMMON_5

    private static readonly StringLongPair[] Pairs5 =
        [new("einz", 1L), new("zwei", 2L), new("drei", 3L), new("view", 4L), new("fünf", 5L)];


    [Test]
    public void Common_Mini()
    {
        var dict = new ImmutableMiniDictionary<string,long>(Pairs5);
        VerifyCommonFunctionality(dict);
    }

    [Test]
    public void Common_Hash()
    {
        var dict = new ImmutableMiniDictionary<string,long>(Pairs5);
        VerifyCommonFunctionality(dict);
    }

    private static void VerifyCommonFunctionality(ImmutableDictionary<string, long> dict)
    {
        dict.Verify
        (
            d => d.IsNotEmpty.ShouldBeTrue(),
            d => d.IsEmpty.ShouldBeFalse(),
            d => d.Count.ShouldBe(5),
            d => d.IndexOfKey("einz").ShouldBe(0),
            d => d.IndexOfKey("zwei").ShouldBe(1),
            d => d.IndexOfKey("drei").ShouldBe(2),
            d => d.IndexOfKey("view").ShouldBe(3),
            d => d.IndexOfKey("fünf").ShouldBe(4),
            d => d.IndexOfKey("sechs").ShouldBeNegative(),
            d => d.LastIndexOfKey("einz", -1).ShouldBe(0),
            d => d.LastIndexOfKey("zwei", -1).ShouldBe(1),
            d => d.LastIndexOfKey("drei", -1).ShouldBe(2),
            d => d.LastIndexOfKey("view", -1).ShouldBe(3),
            d => d.LastIndexOfKey("fünf", -1).ShouldBe(4),
            d => d.LastIndexOfKey("sechs", -99).ShouldBe(-99),
            d => d.Find("einz").ShouldBe(new Found<long>(true,1L)),
            d => d.Find("zwei").ShouldBe(new Found<long>(true,2L)),
            d => d.Find("drei").ShouldBe(new Found<long>(true,3L)),
            d => d.Find("view").ShouldBe(new Found<long>(true,4L)),
            d => d.Find("fünf").ShouldBe(new Found<long>(true,5L)),
            d => d.Find("sechs").ShouldBe(new Found<long>(false,0)),
            d => d.ContainsKey("drei").ShouldBeTrue(),
            d => d.ContainsKey("sechs").ShouldBeFalse()
        );

        dict.Keys.Verify
        (
            ks => ks.IsNotEmpty.ShouldBeTrue(),
            ks => ks.IsEmpty.ShouldBeFalse(),
            ks => ks.Count.ShouldBe(5),
            ks => (ks as IEnumerable<string>).ToArray().ShouldContainAll("einz", "zwei", "drei", "view", "fünf")
        );

        dict.Values.Verify
        (
            vs => vs.IsNotEmpty.ShouldBeTrue(),
            vs => vs.IsEmpty.ShouldBeFalse(),
            vs => vs.Count.ShouldBe(5),
            vs => (vs as IEnumerable<long>).ToArray().ShouldContainAll(1L, 2L, 3L, 4L, 5L)
        );
    }

    #endregion


    #region SORTED_6

    private static readonly UintStringPair[] Pairs6 =
        [new(10u, "A"), new(12u, "B"), new(26u, "C"), new(42u, "D"), new(66u, "E"), new(90u, "F")];

    private static readonly uint[] Keys6 =
        [10u, 12u, 26u, 42u, 66u, 90u];

    [Test]
    public void Sorted_Array()
    {
        var dict = new ImmutableSortedDictionary<uint,string>(Pairs6);
        VerifySortedFunctionality(dict);
    }

    [Test]
    public void Sorted_Flat()
    {
        var dict = new ImmutableFlatDictionary<string>(Pairs6);
        VerifySortedFunctionality(dict);
    }

    private void VerifySortedFunctionality(ImmSortedDict<uint,string> dict)
    {
        dict.Verify
        (
            d => d.IsNotEmpty.ShouldBeTrue(),
            d => d.IsEmpty.ShouldBeFalse(),
            d => d.Count.ShouldBe(6),
            d => d.MinKey.ShouldBe(10u),
            d => d.MaxKey.ShouldBe(90u),
            d => d.FirstEntry.ShouldBe(Pairs6[0]),
            d => d.LastEntry.ShouldBe(Pairs6[^1]),
            d => d.Find(10u).Item.ShouldBe("A"),
            d => d.Find(12u).Item.ShouldBe("B"),
            d => d.Find(26u).Item.ShouldBe("C"),
            d => d.Find(42u).Item.ShouldBe("D"),
            d => d.Find(66u).Item.ShouldBe("E"),
            d => d.Find(90u).Item.ShouldBe("F"),
            d => d.Find(09u).Ok.ShouldBeFalse(),
            d => d.Find(11u).Ok.ShouldBeFalse(),
            d => d.Find(89u).Ok.ShouldBeFalse(),
            d => d.Find(91u).Ok.ShouldBeFalse()
        );

        dict.Keys.Verify
        (
            ks => ks.IsNotEmpty.ShouldBeTrue(),
            ks => ks.IsEmpty.ShouldBeFalse(),
            ks => ks.Count.ShouldBe(6),
            ks => ks.First.ShouldBe(10u),
            ks => ks.Last.ShouldBe(90u),
            ks => (ks as IEnumerable<uint>).ToArray().ShouldContainAll(10u, 12u, 26u, 42u, 66u, 90u),
            ks => ks.IsProperSubsetOf(Keys6).ShouldBeFalse(),
            ks => ks.IsSubsetOf(Keys6).ShouldBeTrue(),
            ks => ks.IsSupersetOf(Keys6).ShouldBeTrue(),
            ks => ks.IsProperSupersetOf(Keys6).ShouldBeFalse(),
            ks => ks.Overlaps(Keys6).ShouldBeTrue(),
            ks => ks.SetEquals(Keys6).ShouldBeTrue()
        );

        dict.Values.Verify
        (
            vs => vs.IsNotEmpty.ShouldBeTrue(),
            vs => vs.IsEmpty.ShouldBeFalse(),
            vs => vs.Count.ShouldBe(6),
            vs => vs.First.ShouldBe("A"),
            vs => vs.Last.ShouldBe("F"),
            vs => (vs as IEnumerable<string>).ToArray().ShouldContainAll("A", "B", "C", "D", "E", "F")
        );

        dict.Entries.Verify
        (
            es => es.IsNotEmpty.ShouldBeTrue(),
            es => es.IsEmpty.ShouldBeFalse(),
            es => es.Count.ShouldBe(6),
            es => es.First.ShouldBe(Pairs6[0]),
            es => es.Last.ShouldBe(Pairs6[^1])
        );
    }

    #endregion
}
