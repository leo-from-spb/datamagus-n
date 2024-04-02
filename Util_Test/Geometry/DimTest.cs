using Util.Common.Geometry;

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


}
