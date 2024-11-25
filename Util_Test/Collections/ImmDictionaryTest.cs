using System.Collections.Generic;

namespace Util.Collections;

[TestFixture]
public class ImmDictionaryTest
{
    [Test]
    public void Empty_fromImm() => CheckEmptyDictionary(new Dictionary<string, uint>().ToImmDict());

    [Test]
    public void Empty_Zero() => CheckEmptyDictionary(ImmZeroDictionary<string, uint>.Instance);

    [Test]
    public void Empty_Mini() => CheckEmptyDictionary(new ImmMiniDictionary<string, uint>(new KeyValuePair<string, uint>[0]));

    [Test]
    public void Empty_Hash() => CheckEmptyDictionary(new ImmHashDictionary<string, uint>(new KeyValuePair<string, uint>[0]));

    private void CheckEmptyDictionary(ImmDictionary<string, uint> dictionary) =>
        dictionary.Verify
        (
            d => d.IsEmpty.ShouldBeTrue(),
            d => d.IsNotEmpty.ShouldBeFalse(),
            d => d.Count.ShouldBe(0),
            d => d.Keys.ShouldBeEmpty(),
            d => d.Values.ShouldBeEmpty(),
            d => d.Entries.ShouldBeEmpty(),
            d => d.Find("").Ok.ShouldBeFalse(),
            d => d.IndexOfKey("").ShouldBeNegative()
        );


    [Test]
    public void D3_fromImm()
    {
        var d3 = new Dictionary<string, uint>();
        d3.Add("einz", 1u);
        d3.Add("zwei", 2u);
        d3.Add("drei", 3u);
        CheckDictionaryOf3(d3.ToImmDict());
    }

    [Test]
    public void D3_Mini()
    {
        KeyValuePair<string, uint>[] a3 = new KeyValuePair<string, uint>[]
                                          {
                                              new("einz", 1u),
                                              new("zwei", 2u),
                                              new("drei", 3u)
                                          };
        ImmDictionary<string, uint> d3 = new ImmMiniDictionary<string, uint>(a3);
        CheckDictionaryOf3(d3);
    }

    [Test]
    public void D3_Hash()
    {
        KeyValuePair<string, uint>[] a3 = new KeyValuePair<string, uint>[]
                                          {
                                              new("einz", 1u),
                                              new("zwei", 2u),
                                              new("drei", 3u)
                                          };
        ImmDictionary<string, uint> d3 = new ImmHashDictionary<string, uint>(a3);
        CheckDictionaryOf3(d3);
    }

    private void CheckDictionaryOf3(ImmDictionary<string, uint> dictionary) =>
        dictionary.Verify
        (
            d => d.IsNotEmpty.ShouldBeTrue(),
            d => d.IsEmpty.ShouldBeFalse(),
            d => d.Count.ShouldBe(3),
            d => d.Find("einz").Ok.ShouldBeTrue(),
            d => d.Find("zwei").Ok.ShouldBeTrue(),
            d => d.Find("drei").Ok.ShouldBeTrue(),
            d => d.Find("einz").Item.ShouldBe(1u),
            d => d.Find("zwei").Item.ShouldBe(2u),
            d => d.Find("drei").Item.ShouldBe(3u),
            d => d.IndexOfKey("einz").ShouldBe(0),
            d => d.IndexOfKey("zwei").ShouldBe(1),
            d => d.IndexOfKey("drei").ShouldBe(2)
        );
}
