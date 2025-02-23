using System.Collections.Generic;
using System.Linq;
using Testing.Appliance.Assertions;

namespace Util.Collections.Implementation;


using StringLongPair = KeyValuePair<string, long>;


[TestFixture]
public class ImmutableArrayDictionaryTest
{

    private readonly StringLongPair[] Pairs5 =
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


    private static void VerifyCommonFunctionality(ImmutableArrayDictionary<string, long> dict)
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

}
