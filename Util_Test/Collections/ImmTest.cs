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
            d => d.ShouldBeOfType<ImmMicroDictionary<ulong, string>>()
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

}
