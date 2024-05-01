using Util.Common.Geometry;
using static Util.Common.Geometry.Dimensions;

namespace Util.Test.Geometry;


[TestFixture]
public class DimTest
{

    [Test]
    public void Basic_InitWithConstructor_Int()
    {
        var m1 = new Dim(3, Unit.mk);
        var m2 = new Dim(3, Unit.mm);
        var m3 = new Dim(3, Unit.cm);
        var m4 = new Dim(3, Unit.dm);
        m1.L.ShouldBe(3);
        m2.L.ShouldBe(3_000);
        m3.L.ShouldBe(30_000);
        m4.L.ShouldBe(300_000);

        var m5 = new Dim(-3, Unit.mk);
        var m6 = new Dim(-3, Unit.mm);
        var m7 = new Dim(-3, Unit.cm);
        var m8 = new Dim(-3, Unit.dm);
        m5.L.ShouldBe(-3);
        m6.L.ShouldBe(-3_000);
        m7.L.ShouldBe(-30_000);
        m8.L.ShouldBe(-300_000);
    }

    [Test]
    public void Basic_InitWithConstructor_Double()
    {
        var m1 = new Dim(3.0, Unit.mk);
        var m2 = new Dim(3.1, Unit.mm);
        var m3 = new Dim(3.3, Unit.cm);
        var m4 = new Dim(3.5, Unit.dm);
        m1.L.ShouldBe(3);
        m2.L.ShouldBe(3_100);
        m3.L.ShouldBe(33_000);
        m4.L.ShouldBe(350_000);

        var m5 = new Dim(-3.0, Unit.mk);
        var m6 = new Dim(-3.1, Unit.mm);
        var m7 = new Dim(-3.3, Unit.cm);
        var m8 = new Dim(-3.5, Unit.dm);
        m5.L.ShouldBe(-3);
        m6.L.ShouldBe(-3_100);
        m7.L.ShouldBe(-33_000);
        m8.L.ShouldBe(-350_000);
    }

    [Test]
    public void Basic_InitWithUnit_Int()
    {
        var m1 = 3.mk();
        var m2 = 3.mm();
        var m3 = 3.cm();
        var m4 = 3.dm();
        m1.L.ShouldBe(3);
        m2.L.ShouldBe(3_000);
        m3.L.ShouldBe(30_000);
        m4.L.ShouldBe(300_000);

        var m5 = (-3).mk();
        var m6 = (-3).mm();
        var m7 = (-3).cm();
        var m8 = (-3).dm();
        m5.L.ShouldBe(-3);
        m6.L.ShouldBe(-3_000);
        m7.L.ShouldBe(-30_000);
        m8.L.ShouldBe(-300_000);
    }

    [Test]
    public void Basic_InitWithUnit_Double()
    {
        var m1 = 3.0.mk();
        var m2 = 3.1.mm();
        var m3 = 3.3.cm();
        var m4 = 3.5.dm();
        m1.L.ShouldBe(3);
        m2.L.ShouldBe(3_100);
        m3.L.ShouldBe(33_000);
        m4.L.ShouldBe(350_000);

        var m5 = (-3.0).mk();
        var m6 = (-3.1).mm();
        var m7 = (-3.3).cm();
        var m8 = (-3.5).dm();
        m5.L.ShouldBe(-3);
        m6.L.ShouldBe(-3_100);
        m7.L.ShouldBe(-33_000);
        m8.L.ShouldBe(-350_000);
    }

    [Test]
    public void ToString_Bit() => Verify
    (
        () => _0_mk.ToString().ShouldBe("0"),
        () => _1_mk.ToString().ShouldBe("1")
    );

    [Test]
    public void ToString_GroupSeparator() => Verify
    (
        () => _3_mm.ToString().ShouldBe("3 000"),
        () => _5_cm.ToString().ShouldBe("50 000")
    );


    [Test]
    public void Dimension_Constants_Test() => Verify
    (
        () => _1_mm.ShouldBe(new Dim(1, Unit.mm)),
        () => _2_mm.ShouldBe(new Dim(2, Unit.mm)),
        () => _3_mm.ShouldBe(new Dim(3, Unit.mm)),
        () => _4_mm.ShouldBe(new Dim(4, Unit.mm)),
        () => _5_mm.ShouldBe(new Dim(5, Unit.mm)),
        () => _6_mm.ShouldBe(new Dim(6, Unit.mm)),
        () => _7_mm.ShouldBe(new Dim(7, Unit.mm)),
        () => _8_mm.ShouldBe(new Dim(8, Unit.mm)),
        () => _9_mm.ShouldBe(new Dim(9, Unit.mm)),

        () => _1_cm.ShouldBe(new Dim(10, Unit.mm)),
        () => _2_cm.ShouldBe(new Dim(20, Unit.mm)),
        () => _3_cm.ShouldBe(new Dim(30, Unit.mm)),
        () => _4_cm.ShouldBe(new Dim(40, Unit.mm)),
        () => _5_cm.ShouldBe(new Dim(50, Unit.mm)),
        () => _6_cm.ShouldBe(new Dim(60, Unit.mm)),
        () => _7_cm.ShouldBe(new Dim(70, Unit.mm)),
        () => _8_cm.ShouldBe(new Dim(80, Unit.mm)),
        () => _9_cm.ShouldBe(new Dim(90, Unit.mm))
    );


}
