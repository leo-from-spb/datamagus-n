using System.Collections.Generic;
using System.Linq;
using Testing.Appliance.Assertions;
using Util.Collections.Implementation;

namespace Util.Collections;

using StringULongPair = KeyValuePair<string,ulong>;
using UIntStringPair = KeyValuePair<uint,string>;


[TestFixture]
public class ImmDictTest
{
    #region 1 ELEMENT

    [Test]
    public void Basic1_Singleton()
    {
        var dict = new ImmutableSingletonDictionary<string,ulong>("thing", 42uL);
        VerifyBasic1(dict);
    }

    [Test]
    public void Basic1_Mini()
    {
        var dict = new ImmutableMiniDictionary<string,ulong>([new("thing", 42uL)]);
        VerifyBasic1(dict);
    }

    [Test]
    public void Basic1_Hash()
    {
        var dict = new ImmutableHashDictionary<string,ulong>([new("thing", 42uL)]);
        VerifyBasic1(dict);
    }

    [Test]
    public void Basic1_1_Param()
    {
        var dict = Imm.Dict("thing", 42uL);
        VerifyBasic1(dict);
    }

    [Test]
    public void Basic1_2_ParamsSame()
    {
        var dict = Imm.Dict("thing", 42uL, "thing", 13uL);
        VerifyBasic1(dict);
    }

    [Test]
    public void Basic1_3_ParamsSame()
    {
        var dict = Imm.Dict("thing", 42uL, "thing", 13uL, "thing", 1uL);
        VerifyBasic1(dict);
    }

    [Test]
    public void Basic1_From_IDictionary()
    {
        Dictionary<string,ulong> d = new Dictionary<string,ulong>();
        d.Add("thing", 42uL);
        IDictionary<string,ulong> id = d;
        var dict = id.ToImmDict();
        VerifyBasic1(dict);
    }

    [Test]
    public void Basic1_From_IReadOnlyDictionary()
    {
        Dictionary<string,ulong> d = new Dictionary<string,ulong>();
        d.Add("thing", 42uL);
        IReadOnlyDictionary<string,ulong> rd = d;
        var dict = rd.ToImmDict();
        VerifyBasic1(dict);
    }

    [Test]
    public void Basic1_From_Dictionary()
    {
        Dictionary<string,ulong> d = new Dictionary<string,ulong>();
        d.Add("thing", 42uL);
        var dict = d.ToImmDict();
        VerifyBasic1(dict);
    }

    private static void VerifyBasic1(ImmDict<string,ulong> dict)
    {
        dict.Verify
        (
            d => d.IsNotEmpty.ShouldBeTrue(),
            d => d.IsEmpty.ShouldBeFalse(),
            d => d.Count.ShouldBe(1),
            d => d.Find("thing").ShouldBe(new Found<ulong>(true, 42L)),
            d => d.Find("nothing").ShouldBe(new Found<ulong>(false, 0)),
            d => d.ContainsKey("thing").ShouldBeTrue(),
            d => d.ContainsKey("nothing").ShouldBeFalse()
        );

        dict.Keys.Verify
        (
            ks => ks.IsNotEmpty.ShouldBeTrue(),
            ks => ks.IsEmpty.ShouldBeFalse(),
            ks => ks.Count.ShouldBe(1),
            ks => ks.Contains("thing").ShouldBeTrue(),
            ks => ks.Contains("nothing").ShouldBeFalse(),
            ks => (ks as IEnumerable<string>).ToArray().ShouldContainAll("thing")
        );

        dict.Values.Verify
        (
            vs => vs.IsNotEmpty.ShouldBeTrue(),
            vs => vs.IsEmpty.ShouldBeFalse(),
            vs => vs.Count.ShouldBe(1),
            vs => vs.Contains(42uL).ShouldBeTrue(),
            vs => vs.Contains(13uL).ShouldBeFalse(),
            vs => (vs as IEnumerable<ulong>).ToArray().ShouldContainAll(42uL)
        );
    }

    #endregion


    #region 3 ELEMENTS

    private readonly StringULongPair[] Pairs3 =
        [new("раз", 1uL), new("два", 2uL), new("три", 3uL)];

    [Test]
    public void Basic3_Mini()
    {
        var dict = new ImmutableMiniDictionary<string,ulong>(Pairs3);
        VerifyBasic3(dict);
    }

    [Test]
    public void Basic3_Hash()
    {
        var dict = new ImmutableHashDictionary<string,ulong>(Pairs3);
        VerifyBasic3(dict);
    }

    [Test]
    public void Basic3_Param()
    {
        var dict = Imm.Dict("раз", 1uL, "два", 2uL, "три", 3uL);
        VerifyBasic3(dict);
    }

    private static void VerifyBasic3(ImmOrderedDict<string,ulong> dict)
    {
        dict.Verify
        (
            d => d.IsNotEmpty.ShouldBeTrue(),
            d => d.IsEmpty.ShouldBeFalse(),
            d => d.Count.ShouldBe(3),
            d => d.Find("раз").ShouldBe(new Found<ulong>(true, 1uL)),
            d => d.Find("два").ShouldBe(new Found<ulong>(true, 2uL)),
            d => d.Find("три").ShouldBe(new Found<ulong>(true, 3uL)),
            d => d.Find("десять").ShouldBe(new Found<ulong>(false, 0uL)),
            d => d.ContainsKey("раз").ShouldBeTrue(),
            d => d.ContainsKey("два").ShouldBeTrue(),
            d => d.ContainsKey("три").ShouldBeTrue(),
            d => d.ContainsKey("восемь").ShouldBeFalse()
        );

        dict.Keys.Verify
        (
            ks => ks.IsNotEmpty.ShouldBeTrue(),
            ks => ks.IsEmpty.ShouldBeFalse(),
            ks => ks.Count.ShouldBe(3),
            ks => ks[0].ShouldBe("раз"),
            ks => ks[1].ShouldBe("два"),
            ks => ks[2].ShouldBe("три"),
            ks => ks.Contains("раз").ShouldBeTrue(),
            ks => ks.Contains("два").ShouldBeTrue(),
            ks => ks.Contains("три").ShouldBeTrue(),
            ks => ks.Contains("шесть").ShouldBeFalse(),
            ks => (ks as IEnumerable<string>).ToArray().ShouldContainAll("раз", "два", "три")
        );

        dict.Values.Verify
        (
            vs => vs.IsNotEmpty.ShouldBeTrue(),
            vs => vs.IsEmpty.ShouldBeFalse(),
            vs => vs.Count.ShouldBe(3),
            vs => vs[0].ShouldBe(1uL),
            vs => vs[1].ShouldBe(2uL),
            vs => vs[2].ShouldBe(3uL),
            vs => vs.Contains(1uL).ShouldBeTrue(),
            vs => vs.Contains(2uL).ShouldBeTrue(),
            vs => vs.Contains(3uL).ShouldBeTrue(),
            vs => vs.Contains(10uL).ShouldBeFalse(),
            vs => (vs as IEnumerable<ulong>).ToArray().ShouldContainAll(1uL, 2uL, 3uL)
        );
    }

    #endregion


    #region 5 UINT

    private UIntStringPair[] Uints5 =
        [new(26u, "Lipetsk"), new(33u, "Piter"), new(42u, "Moscow"), new(66u, "Tambov"), new(74u, "Elets")];

    [Test]
    public void Uint5_Mini()
    {
        var dict = new ImmutableMiniDictionary<uint,string>(Uints5);
        VerifyUints5(dict);
    }

    [Test]
    public void Uint5_Hash()
    {
        var dict = new ImmutableHashDictionary<uint,string>(Uints5);
        VerifyUints5(dict);
    }

    [Test]
    public void Uint5_Flat()
    {
        var dict = new ImmutableFlatDictionary<string>(Uints5);
        VerifyUints5(dict);
    }

    [Test]
    public void Uint5_FromDictionary()
    {
        var dictionary = new Dictionary<uint,string> { {26u, "Lipetsk"}, {33u, "Piter"}, {42u, "Moscow"}, {66u, "Tambov"}, {74u, "Elets"} };
        var dict       = dictionary.ToImmDict();
        VerifyUints5(dict);
    }

    private void VerifyUints5(ImmDict<uint,string> dict)
    {
        dict.Verify
        (
            d => d.IsNotEmpty.ShouldBeTrue(),
            d => d.IsEmpty.ShouldBeFalse(),
            d => d.Count.ShouldBe(5),
            d => d.Find(26u).ShouldBe(new Found<string>(true, "Lipetsk")),
            d => d.Find(33u).ShouldBe(new Found<string>(true, "Piter")),
            d => d.Find(42u).ShouldBe(new Found<string>(true, "Moscow")),
            d => d.Find(66u).ShouldBe(new Found<string>(true, "Tambov")),
            d => d.Find(74u).ShouldBe(new Found<string>(true, "Elets")),
            d => d.Find(25u).Ok.ShouldBeFalse(),
            d => d.Find(27u).Ok.ShouldBeFalse(),
            d => d.Find(73u).Ok.ShouldBeFalse(),
            d => d.Find(75u).Ok.ShouldBeFalse()
        );

        dict.Keys.Verify
        (
            ks => ks.IsNotEmpty.ShouldBeTrue(),
            ks => ks.IsEmpty.ShouldBeFalse(),
            ks => ks.Count.ShouldBe(5),
            ks => ks.Contains(26u).ShouldBeTrue(),
            ks => ks.Contains(33u).ShouldBeTrue(),
            ks => ks.Contains(42u).ShouldBeTrue(),
            ks => ks.Contains(66u).ShouldBeTrue(),
            ks => ks.Contains(74u).ShouldBeTrue(),
            ks => ks.Contains(25u).ShouldBeFalse(),
            ks => ks.Contains(27u).ShouldBeFalse(),
            ks => ks.Contains(73u).ShouldBeFalse(),
            ks => ks.Contains(75u).ShouldBeFalse(),
            ks => ks.ToArray().ShouldContainAll(26u, 33u, 42u, 66u, 74u)
            //ks => ks.ToArray().ShouldContainAll("Lipetsk", "Piter", "Moscow", "Tambov", "Elets")
        );

        dict.Values.Verify
        (
            vs => vs.IsNotEmpty.ShouldBeTrue(),
            vs => vs.IsEmpty.ShouldBeFalse(),
            vs => vs.Count.ShouldBe(5),
            vs => vs.Contains("Lipetsk").ShouldBeTrue(),
            vs => vs.Contains("Piter").ShouldBeTrue(),
            vs => vs.Contains("Moscow").ShouldBeTrue(),
            vs => vs.Contains("Tambov").ShouldBeTrue(),
            vs => vs.Contains("Elets").ShouldBeTrue(),
            vs => vs.Contains("X").ShouldBeFalse(),
            vs => vs.ToArray().ShouldContainAll("Lipetsk", "Piter", "Moscow", "Tambov", "Elets")
        );
    }

    #endregion
}
