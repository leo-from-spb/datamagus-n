using Util.Common.Geometry;

namespace Util.Test.Geometry;


[TestFixture]
public class DimensionTest
{

    [Test]
    public void Basic_InitWithConstructor_Int()
    {
        var m1 = new Dimension(3, Unit.mk);
        var m2 = new Dimension(3, Unit.mm);
        var m3 = new Dimension(3, Unit.cm);
        var m4 = new Dimension(3, Unit.dm);
        Assert.That(m1.L, Is.EqualTo(3));
        Assert.That(m2.L, Is.EqualTo(3_000));
        Assert.That(m3.L, Is.EqualTo(30_000));
        Assert.That(m4.L, Is.EqualTo(300_000));

        var m5 = new Dimension(-3, Unit.mk);
        var m6 = new Dimension(-3, Unit.mm);
        var m7 = new Dimension(-3, Unit.cm);
        var m8 = new Dimension(-3, Unit.dm);
        Assert.That(m5.L, Is.EqualTo(-3));
        Assert.That(m6.L, Is.EqualTo(-3_000));
        Assert.That(m7.L, Is.EqualTo(-30_000));
        Assert.That(m8.L, Is.EqualTo(-300_000));
    }

    [Test]
    public void Basic_InitWithConstructor_Double()
    {
        var m1 = new Dimension(3.0, Unit.mk);
        var m2 = new Dimension(3.1, Unit.mm);
        var m3 = new Dimension(3.3, Unit.cm);
        var m4 = new Dimension(3.5, Unit.dm);
        Assert.That(m1.L, Is.EqualTo(3));
        Assert.That(m2.L, Is.EqualTo(3_100));
        Assert.That(m3.L, Is.EqualTo(33_000));
        Assert.That(m4.L, Is.EqualTo(350_000));

        var m5 = new Dimension(-3.0, Unit.mk);
        var m6 = new Dimension(-3.1, Unit.mm);
        var m7 = new Dimension(-3.3, Unit.cm);
        var m8 = new Dimension(-3.5, Unit.dm);
        Assert.That(m5.L, Is.EqualTo(-3));
        Assert.That(m6.L, Is.EqualTo(-3_100));
        Assert.That(m7.L, Is.EqualTo(-33_000));
        Assert.That(m8.L, Is.EqualTo(-350_000));
    }

    [Test]
    public void Basic_InitWithUnit_Int()
    {
        var m1 = 3.mk();
        var m2 = 3.mm();
        var m3 = 3.cm();
        var m4 = 3.dm();
        Assert.That(m1.L, Is.EqualTo(3));
        Assert.That(m2.L, Is.EqualTo(3_000));
        Assert.That(m3.L, Is.EqualTo(30_000));
        Assert.That(m4.L, Is.EqualTo(300_000));

        var m5 = (-3).mk();
        var m6 = (-3).mm();
        var m7 = (-3).cm();
        var m8 = (-3).dm();
        Assert.That(m5.L, Is.EqualTo(-3));
        Assert.That(m6.L, Is.EqualTo(-3_000));
        Assert.That(m7.L, Is.EqualTo(-30_000));
        Assert.That(m8.L, Is.EqualTo(-300_000));
    }

    [Test]
    public void Basic_InitWithUnit_Double()
    {
        var m1 = 3.0.mk();
        var m2 = 3.1.mm();
        var m3 = 3.3.cm();
        var m4 = 3.5.dm();
        Assert.That(m1.L, Is.EqualTo(3));
        Assert.That(m2.L, Is.EqualTo(3_100));
        Assert.That(m3.L, Is.EqualTo(33_000));
        Assert.That(m4.L, Is.EqualTo(350_000));

        var m5 = (-3.0).mk();
        var m6 = (-3.1).mm();
        var m7 = (-3.3).cm();
        var m8 = (-3.5).dm();
        Assert.That(m5.L, Is.EqualTo(-3));
        Assert.That(m6.L, Is.EqualTo(-3_100));
        Assert.That(m7.L, Is.EqualTo(-33_000));
        Assert.That(m8.L, Is.EqualTo(-350_000));
    }


}
