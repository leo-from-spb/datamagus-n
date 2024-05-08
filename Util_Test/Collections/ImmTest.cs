using System.Collections.Generic;

namespace Util.Collections;

[TestFixture]
public class ImmTest
{

    [Test]
    public void Dictionary_Imm_Basic_0()
    {
        var origin = new Dictionary<ulong, string>();

        var immD = origin.ToImm();
        immD.Verify
        (
            d => d.IsEmpty.ShouldBeTrue(),
            d => d.Count.ShouldBe(0),
            d => d.ShouldBeOfType<ImmEmptyDictionary<ulong, string>>()
        );
    }

    [Test]
    public void Dictionary_Imm_Basic_1()
    {
        var origin = new Dictionary<ulong, string>
                     {
                         { 5uL, "fünf" }
                     };


        var immD = origin.ToImm();
        immD.Verify
        (
            d => d.IsNotEmpty.ShouldBeTrue(),
            d => d.Count.ShouldBe(1),
            d => d.ShouldBeOfType<ImmMicroDictionary<ulong, string>>(),
            d => d[5uL].ShouldBe("fünf")
        );
    }

    [Test]
    public void Dictionary_Imm_Basic_3()
    {
        var origin = new Dictionary<ulong, string>
                     {
                         { 1uL, "einz" },
                         { 2uL, "zwei" },
                         { 3uL, "drei" },
                     };

        var immD = origin.ToImm();
        immD.Verify
        (
            d => d.IsNotEmpty.ShouldBeTrue(),
            d => d.Count.ShouldBe(3),
            d => d.ShouldBeOfType<ImmMicroDictionary<ulong, string>>(),
            d => d[1uL].ShouldBe("einz"),
            d => d[2uL].ShouldBe("zwei"),
            d => d[3uL].ShouldBe("drei")
        );
    }

    [Test]
    public void Dictionary_Imm_Basic_N()
    {
        const int N = 20;

        var origin = new Dictionary<ulong, string>(N);

        for (int i = 1; i <= N; i++) origin[(ulong)i] = i.ToString();

        var immD = origin.ToImm();
        immD.Verify
        (
            d => d.IsNotEmpty.ShouldBeTrue(),
            d => d.Count.ShouldBe(N),
            d => d.ShouldBeOfType<ImmCompactHashDictionary<ulong, string>>(),
            d => d[3uL].ShouldBe("3")
        );
    }


    [Test]
    public void R_InterfaceMethods()
    {
        ROrderDictionary<ulong, string> dictionary =
            Imm.Dictionary<ulong, string>(26uL, "Not 42", 42uL, "This is 42");
        dictionary.Verify
        (
            d => d[0].Key.ShouldBe(26uL),
            d => d[1].Key.ShouldBe(42uL),
            d => d.Find(26uL).Item.ShouldBe("Not 42"),
            d => d.Get(26uL).ShouldBe("Not 42"),
            d => d[26uL].ShouldBe("Not 42"),
            d => d[42uL].ShouldBe("This is 42"),
            d => d.ContainsKey(26uL).ShouldBeTrue(),
            d => d.Count.ShouldBe(2),
            d => d.IsNotEmpty.ShouldBeTrue(),
            d => d.IsEmpty.ShouldBeFalse(),
            d => d.Keys.ShouldContain(26uL),
            d => d.Values.ShouldContain("Not 42"),
            d => d.Entries.ShouldNotBeEmpty()
        );
    }

    [Test]
    public void I_InterfaceMethods_IReadOnlyDictionary()
    {
        IReadOnlyDictionary<ulong, string> dictionary = Imm.Dictionary<ulong, string>(26uL, "Not 42");
        dictionary.Verify
        (
            d => d.TryGetValue(26uL, out _).ShouldBeTrue(),
            d => d[26uL].ShouldBe("Not 42"),
            d => d.ContainsKey(26uL).ShouldBeTrue(),
            d => d.Count.ShouldBe(1),
            d => d.Keys.ShouldContain(26uL),
            d => d.Values.ShouldContain("Not 42"),
            d => d.GetEnumerator().Dispose()
        );
    }
}
