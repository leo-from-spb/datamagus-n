using System.Collections.Generic;
using System.Linq;
using Testing.Appliance.Assertions;
using Util.Collections.Implementation;

namespace Util.Collections;

using StringULongPair = KeyValuePair<string,ulong>;

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

    private static void VerifyBasic3(ImmDict<string,ulong> dict)
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
            vs => vs.Contains(1uL).ShouldBeTrue(),
            vs => vs.Contains(2uL).ShouldBeTrue(),
            vs => vs.Contains(3uL).ShouldBeTrue(),
            vs => vs.Contains(10uL).ShouldBeFalse(),
            vs => (vs as IEnumerable<ulong>).ToArray().ShouldContainAll(1uL, 2uL, 3uL)
        );
    }

    #endregion
}
