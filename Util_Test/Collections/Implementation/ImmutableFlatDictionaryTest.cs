using System.Collections.Generic;
using System.Linq;
using Testing.Appliance.Assertions;

namespace Util.Collections.Implementation;


[TestFixture]
public class ImmutableFlatDictionaryTest
{
    [Test]
    public void Basic_1()
    {
        var od   = new Dictionary<uint,string> { { 1974u, "year" } };
        var dict = new ImmutableFlatDictionary<string>(od);
        dict.Verify
        (
            d => d.MinKey.ShouldBe(1974u),
            d => d.MaxKey.ShouldBe(1974u),
            d => d.Capacity.ShouldBe(1u),
            d => d.Count.ShouldBe(1),
            d => d.IndexOfKey(1974u).ShouldBe(0),
            d => d.IndexOfKey(1000u).ShouldBeNegative(),
            d => d.IndexOfKey(9999u).ShouldBeNegative(),
            d => d.ContainsKey(1974u).ShouldBeTrue(),
            d => d.ContainsKey(1984u).ShouldBeFalse(),
            d => d.Find(1974u).ShouldBe(new Found<string>(true,"year")),
            d => d.Find(1984u).Ok.ShouldBeFalse(),
            d => d.Keys.ToArray().ShouldContainAll([1974u]),
            d => d.Values.ToArray().ShouldContainAll(["year"])
        );
    }


    [Test]
    public void Basic_3()
    {
        var od   = new Dictionary<uint,string> { { 26u, "Липецк" }, { 42u, "Москва" }, { 74u, "Елец" } };
        var dict = new ImmutableFlatDictionary<string>(od);
        dict.Verify
        (
            d => d.MinKey.ShouldBe(26u),
            d => d.MaxKey.ShouldBe(74u),
            d => d.Capacity.ShouldBe(49u),
            d => d.Count.ShouldBe(3),
            d => d.IndexOfKey(26u).ShouldBe(0),
            d => d.IndexOfKey(42u).ShouldBe(16),
            d => d.IndexOfKey(74u).ShouldBe(48),
            d => d.IndexOfKey(25u).ShouldBeNegative(),
            d => d.IndexOfKey(27u).ShouldBeNegative(),
            d => d.IndexOfKey(73u).ShouldBeNegative(),
            d => d.IndexOfKey(75u).ShouldBeNegative(),
            d => d.IndexOfKey(0u).ShouldBeNegative(),
            d => d.IndexOfKey(uint.MaxValue).ShouldBeNegative(),
            d => d.ContainsKey(26u).ShouldBeTrue(),
            d => d.ContainsKey(42u).ShouldBeTrue(),
            d => d.ContainsKey(74u).ShouldBeTrue(),
            d => d.ContainsKey(25u).ShouldBeFalse(),
            d => d.ContainsKey(27u).ShouldBeFalse(),
            d => d.ContainsKey(73u).ShouldBeFalse(),
            d => d.ContainsKey(75u).ShouldBeFalse(),
            d => d.ContainsKey(0u).ShouldBeFalse(),
            d => d.ContainsKey(uint.MaxValue).ShouldBeFalse(),
            d => d.Find(26u).ShouldBe(new Found<string>(true,"Липецк")),
            d => d.Find(42u).ShouldBe(new Found<string>(true,"Москва")),
            d => d.Find(74u).ShouldBe(new Found<string>(true,"Елец")),
            d => d.Find(0u).Ok.ShouldBeFalse(),
            d => d.Find(33u).Ok.ShouldBeFalse(),
            d => d.Keys.ToArray().ShouldContainAll([26u, 42u, 74u]),
            d => d.Keys.ToArray().Length.ShouldBe(3),
            d => d.Values.ToArray().ShouldContainAll(["Липецк", "Москва", "Елец"]),
            d => d.Values.ToArray().Length.ShouldBe(3)
        );
    }

}
