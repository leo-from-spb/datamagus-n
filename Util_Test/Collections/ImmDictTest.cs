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
        var dict = new ImmutableSingletonSortedDictionary<string,ulong>("thing", 42uL);
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
    public void Basic1_Sorted()
    {
        var dict = new ImmutableSortedDictionary<string,ulong>([new("thing", 42uL)]);
        VerifyBasic1(dict);
    }

    [Test]
    public void Basic1_1_Param()
    {
        var dict = Imm.DictOf("thing", 42uL);
        VerifyBasic1(dict);
    }

    [Test]
    public void Basic1_2_ParamsSame()
    {
        var dict = Imm.DictOf("thing", 42uL, "thing", 13uL);
        VerifyBasic1(dict);
    }

    [Test]
    public void Basic1_3_ParamsSame()
    {
        var dict = Imm.DictOf("thing", 42uL, "thing", 13uL, "thing", 1uL);
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

    [Test]
    public void Basic1_From_Array_byKey()
    {
        ulong[] array42 = [42uL];
        ImmOrdDict<string, ulong> dict = array42.ToImmDict(_ => "thing");
        VerifyBasic1(dict);
    }

    [Test]
    public void Basic1_From_Array_byKeyAndValue()
    {
        byte[] arrayX = [_26_];
        ImmOrdDict<string, ulong> dict = arrayX.ToImmDict(_ => "thing", x => (ulong)(x + 16u));
        VerifyBasic1(dict);
    }

    private static void VerifyBasic1(ImmOrdDict<string,ulong> dict)
    {
        dict.Verify
        (
            d => d.IsNotEmpty.ShouldBeTrue(),
            d => d.IsEmpty.ShouldBeFalse(),
            d => d.Count.ShouldBe(1),
            d => d.FirstEntry.ShouldBe(new StringULongPair("thing", 42uL)),
            d => d.LastEntry.ShouldBe(new StringULongPair("thing", 42uL)),
            d => d.Find("thing").ShouldBe(new Found<ulong>(true, 42uL)),
            d => d.Find("nothing").ShouldBe(new Found<ulong>(false, 0)),
            d => d.ContainsKey("thing").ShouldBeTrue(),
            d => d.ContainsKey("nothing").ShouldBeFalse()
        );

        dict.Keys.Verify
        (
            ks => ks.IsNotEmpty.ShouldBeTrue(),
            ks => ks.IsEmpty.ShouldBeFalse(),
            ks => ks.Count.ShouldBe(1),
            ks => ks.First.ShouldBe("thing"),
            ks => ks.Last.ShouldBe("thing"),
            ks => ks.Contains("thing").ShouldBeTrue(),
            ks => ks.Contains("nothing").ShouldBeFalse(),
            ks => (ks as IEnumerable<string>).ToArray().ShouldContainAll("thing")
        );

        dict.Values.Verify
        (
            vs => vs.IsNotEmpty.ShouldBeTrue(),
            vs => vs.IsEmpty.ShouldBeFalse(),
            vs => vs.Count.ShouldBe(1),
            vs => vs.First.ShouldBe(42uL),
            vs => vs.Last.ShouldBe(42uL),
            vs => vs.Contains(42uL).ShouldBeTrue(),
            vs => vs.Contains(13uL).ShouldBeFalse(),
            vs => (vs as IEnumerable<ulong>).ToArray().ShouldContainAll(42uL)
        );
    }

    #endregion


    #region 3 ELEMENTS

    private static readonly StringULongPair[] Pairs3 =
        [new("один", 1uL), new("пара", 2uL), new("тройка", 3uL)];

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
    public void Basic3_Sorted()
    {
        var dict = new ImmutableSortedDictionary<string,ulong>(Pairs3);
        VerifyBasic3(dict);
    }

    [Test]
    public void Basic3_Param()
    {
        var dict = Imm.DictOf("один", 1uL, "пара", 2uL, "тройка", 3uL);
        VerifyBasic3(dict);
    }

    [Test]
    public void Basic3_From_Array_byKey()
    {
        ulong[] array123 = [1uL, 2uL, 3uL];
        ImmListDict<string, ulong> dict =
            array123.ToImmDict(x => x switch { 1uL => "один", 2uL => "пара", 3uL => "тройка", _ => "?" });
        VerifyBasic3(dict);
    }

    [Test]
    public void Basic3_From_Array_byKeyAndValue()
    {
        byte[] array123 = [_1_, _2_, _3_];
        ImmListDict<string, ulong> dict =
            array123.ToImmDict(x => x switch { _1_ => "один", _2_ => "пара", _3_ => "тройка", _ => "?" },
                               y => y switch { _1_ => 1uL,    _2_ => 2uL,    _3_ => 3uL,      _ => 0uL });
        VerifyBasic3(dict);
    }

    [Test]
    public void Basic3_From_IEnumerable_byKey()
    {
        IEnumerable<ulong> ie123 = new List<ulong> { 1uL, 2uL, 3uL };
        ImmListDict<string, ulong> dict =
            ie123.ToImmDict(x => x switch { 1uL => "один", 2uL => "пара", 3uL => "тройка", _ => "?" });
        VerifyBasic3(dict);
    }

    [Test]
    public void Basic3_From_IEnumerable_byKeyAndValue()
    {
        IEnumerable<byte> ie123 = new List<byte> { _1_, _2_, _3_ };
        ImmListDict<string, ulong> dict =
            ie123.ToImmDict(x => x switch { _1_ => "один", _2_ => "пара", _3_ => "тройка", _ => "?" },
                            y => y switch { _1_ => 1uL,    _2_ => 2uL,    _3_ => 3uL,      _ => 0uL });
        VerifyBasic3(dict);
    }

    private static void VerifyBasic3(ImmListDict<string,ulong> dict)
    {
        dict.Verify
        (
            d => d.IsNotEmpty.ShouldBeTrue(),
            d => d.IsEmpty.ShouldBeFalse(),
            d => d.Count.ShouldBe(3),
            d => d.FirstEntry.ShouldBe(Pairs3[0]),
            d => d.LastEntry.ShouldBe(Pairs3[^1]),
            d => d.Find("один").ShouldBe(new Found<ulong>(true, 1uL)),
            d => d.Find("пара").ShouldBe(new Found<ulong>(true, 2uL)),
            d => d.Find("тройка").ShouldBe(new Found<ulong>(true, 3uL)),
            d => d.Find("десять").ShouldBe(new Found<ulong>(false, 0uL)),
            d => d.ContainsKey("один").ShouldBeTrue(),
            d => d.ContainsKey("пара").ShouldBeTrue(),
            d => d.ContainsKey("тройка").ShouldBeTrue(),
            d => d.ContainsKey("восемь").ShouldBeFalse()
        );

        dict.Keys.Verify
        (
            ks => ks.IsNotEmpty.ShouldBeTrue(),
            ks => ks.IsEmpty.ShouldBeFalse(),
            ks => ks.Count.ShouldBe(3),
            ks => ks.First.ShouldBe("один"),
            ks => ks.Last.ShouldBe("тройка"),
            ks => ks[0].ShouldBe("один"),
            ks => ks[1].ShouldBe("пара"),
            ks => ks[2].ShouldBe("тройка"),
            ks => ks.Contains("один").ShouldBeTrue(),
            ks => ks.Contains("пара").ShouldBeTrue(),
            ks => ks.Contains("тройка").ShouldBeTrue(),
            ks => ks.Contains("шесть").ShouldBeFalse(),
            ks => (ks as IEnumerable<string>).ToArray().ShouldContainAll("один", "пара", "тройка")
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

    private readonly UIntStringPair[] Uints5 =
        [new(26u, "Lipetsk"), new(33u, "Piter"), new(42u, "Moscow"), new(66u, "Tambov"), new(74u, "Elets")];

    [Test]
    public void Uint5_Mini()
    {
        var dict = new ImmutableMiniDictionary<uint,string>(Uints5);
        VerifyDict5(dict);
    }

    [Test]
    public void Uint5_Hash()
    {
        var dict = new ImmutableHashDictionary<uint,string>(Uints5);
        VerifyDict5(dict);
    }

    [Test]
    public void Uint5_Flat()
    {
        var dict = new ImmutableFlatDictionary<string>(Uints5);
        VerifySortedDict5(dict);
    }

    [Test]
    public void Uint5_FromIReadOnlyDictionary1()
    {
        IDictionary<uint,string> dictionary =
            new Dictionary<uint,string> { {26u, "Lipetsk"}, {33u, "Piter"}, {42u, "Moscow"}, {66u, "Tambov"}, {74u, "Elets"} };
        var dict = dictionary.ToImmDict();
        VerifyDict5(dict);
    }

    [Test]
    public void Uint5_FromIReadOnlyDictionary2()
    {
        IReadOnlyDictionary<uint,string> dictionary =
            new Dictionary<uint,string> { {26u, "Lipetsk"}, {33u, "Piter"}, {42u, "Moscow"}, {66u, "Tambov"}, {74u, "Elets"} };
        var dict = dictionary.ToImmSortedDict();
        VerifySortedDict5(dict);
    }

    [Test]
    public void Uint5_FromIReadOnlyDictionary3()
    {
        IReadOnlyDictionary<uint,string> dictionary1 =
            new Dictionary<uint,string> { {26u, "Lipetsk"}, {33u, "Piter"}, {42u, "Moscow"}, {66u, "Tambov"}, {74u, "Elets"} };
        IReadOnlyDictionary<uint, string> dictionary2 =
            dictionary1.ToImmSortedDict();
        var dict = dictionary2.ToImmSortedDict();
        VerifySortedDict5(dict);
    }

    [Test]
    public void Uint5_FromIDictionary1()
    {
        IReadOnlyDictionary<uint,string> dictionary =
            new Dictionary<uint,string> { {26u, "Lipetsk"}, {33u, "Piter"}, {42u, "Moscow"}, {66u, "Tambov"}, {74u, "Elets"} };
        var dict = dictionary.ToImmDict();
        VerifyDict5(dict);
    }

    [Test]
    public void Uint5_FromIDictionary2()
    {
        IDictionary<uint,string> dictionary =
            new Dictionary<uint,string> { {26u, "Lipetsk"}, {33u, "Piter"}, {42u, "Moscow"}, {66u, "Tambov"}, {74u, "Elets"} };
        var dict = dictionary.ToImmSortedDict();
        VerifySortedDict5(dict);
    }

    [Test]
    public void Uint5_FromDictionary1()
    {
        var dictionary = new Dictionary<uint,string> { {26u, "Lipetsk"}, {33u, "Piter"}, {42u, "Moscow"}, {66u, "Tambov"}, {74u, "Elets"} };
        var dict       = dictionary.ToImmDict();
        VerifyDict5(dict);
    }

    [Test]
    public void Uint5_FromDictionary2()
    {
        var dictionary = new Dictionary<uint,string> { {26u, "Lipetsk"}, {33u, "Piter"}, {42u, "Moscow"}, {66u, "Tambov"}, {74u, "Elets"} };
        var dict       = dictionary.ToImmSortedDict();
        VerifySortedDict5(dict);
    }

    [Test]
    public void Uint5_FromArray_byKey()
    {
        var helpingDictionary =
            new Dictionary<string, uint> { {"Lipetsk", 26u}, {"Piter", 33u}, {"Moscow", 42u}, {"Tambov", 66u}, {"Elets", 74u} };
        string[] array = ["Lipetsk", "Piter", "Moscow", "Tambov", "Elets"];
        var dict = array.ToImmSortedDict(k => helpingDictionary[k]);
        VerifySortedDict5(dict);
    }

    [Test]
    public void Uint5_FromArray_byKeyAndValue()
    {
        var helpingDictionary =
            new Dictionary<int, string> { {26, "Lipetsk"}, {33, "Piter"}, {42, "Moscow"}, {66, "Tambov"}, {74, "Elets"} };
        int[] array = [26, 74, 42, 33, 66];
        var   dict  = array.ToImmSortedDict(x => (uint)x, x => helpingDictionary[x]);
        VerifySortedDict5(dict);
    }

    [Test]
    public void Uint5_FromEnumerable_byKey()
    {
        var helpingDictionary =
            new Dictionary<string, uint> { {"Lipetsk", 26u}, {"Piter", 33u}, {"Moscow", 42u}, {"Tambov", 66u}, {"Elets", 74u} };
        IEnumerable<string> ie   =
            new HashSet<string> {"Lipetsk", "Piter", "Moscow", "Tambov", "Elets"};
        var              dict = ie.ToImmSortedDict(k => helpingDictionary[k]);
        VerifySortedDict5(dict);
    }

    [Test]
    public void Uint5_FromEnumerable_byKeyAndValue()
    {
        var helpingDictionary =
            new Dictionary<int, string> { {26, "Lipetsk"}, {33, "Piter"}, {42, "Moscow"}, {66, "Tambov"}, {74, "Elets"} };
        IEnumerable<int> ie
            = new HashSet<int> {26, 74, 42, 33, 66};
        ImmSortedDict<uint, string> dict
            = ie.ToImmSortedDict(x => (uint)x, x => helpingDictionary[x]);
        VerifySortedDict5(dict);
    }

    private void VerifySortedDict5(ImmSortedDict<uint, string> dict)
    {
        VerifyDict5(dict);

        dict.Verify
        (
            d => d.MinKey.ShouldBe(26u),
            d => d.MaxKey.ShouldBe(74u),
            d => d.FirstEntry.ShouldBe(new UIntStringPair(26u, "Lipetsk")),
            d => d.LastEntry.ShouldBe(new UIntStringPair(74u, "Elets"))
        );

        dict.Keys.Verify
        (
            ks => ks.First.ShouldBe(26u),
            ks => ks.Last.ShouldBe(74u)
        );

        dict.Values.Verify
        (
            vs => vs.First.ShouldBe("Lipetsk"),
            vs => vs.Last.ShouldBe("Elets")
        );
    }

    private void VerifyDict5(ImmDict<uint,string> dict)
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
