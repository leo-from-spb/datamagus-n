using Model.Imp.Abstracts;
using static NUnit.Framework.Assert;

namespace Model.Test.Abstracts;

[TestFixture]
public class MatterTest
{
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var r1 = new ImmTestRabbit(11, "Rabbit A");
        var r2 = new ImmTestRabbit(12, "Rabbit B");
        var r3 = new ImmTestRabbit(13, "Rabbit C");
        var rabbits = ImmNamingFamily<ImmTestRabbit>.of(r1, r2, r3);
        
        var p1 = new ImmTestGuineaPig(31, "Pig A");
        var p2 = new ImmTestGuineaPig(32, "Pig B");
        var p3 = new ImmTestGuineaPig(33, "Pig C");
        var p4 = new ImmTestGuineaPig(34, "Pig D");
        var pigs = ImmNamingFamily<ImmTestGuineaPig>.of(p1, p2, p3, p4);

        var yard = new ImmTestYard(1, rabbits, pigs);
        
        Assert.Multiple(() =>
        {
            That(yard.Rabbits, Has.Count.EqualTo(3));
            That(yard.GuineaPigs, Has.Count.EqualTo(4));
            That(yard.Rabbits["Rabbit A"], Is.SameAs(r1));
            That(yard.Rabbits["Rabbit B"], Is.SameAs(r2));
            That(yard.GuineaPigs["Pig A"], Is.SameAs(p1));
            That(yard.GuineaPigs["Pig D"], Is.SameAs(p4));
        });
    }
}