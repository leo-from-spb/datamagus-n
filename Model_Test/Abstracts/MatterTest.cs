using Shouldly;
using Testing.Appliance.Assertions;

namespace Model.Abstracts;

[TestFixture]
public class MatterTest
{
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Yard()
    {
        var r1 = new ImmTestRabbit(11, "Rabbit A");
        var r2 = new ImmTestRabbit(12, "Rabbit B");
        var r3 = new ImmTestRabbit(13, "Rabbit C");
        var rabbits = ImmNamingFamily<ImmTestRabbit>.Of(r1, r2, r3);
        
        var p1 = new ImmTestGuineaPig(31, "Pig A");
        var p2 = new ImmTestGuineaPig(32, "Pig B");
        var p3 = new ImmTestGuineaPig(33, "Pig C");
        var p4 = new ImmTestGuineaPig(34, "Pig D");
        var pigs = ImmNamingFamily<ImmTestGuineaPig>.Of(p1, p2, p3, p4);

        var yard = new ImmTestYard(1, rabbits, pigs);
        
        yard.Verify
        (
            y => y.Rabbits.Count.ShouldBe(3),
            y => y.GuineaPigs.Count.ShouldBe(4),
            y => y.Rabbits["Rabbit A"].ShouldBeSameAs(r1),
            y => y.Rabbits["Rabbit B"].ShouldBeSameAs(r2),
            y => y.GuineaPigs["Pig A"].ShouldBeSameAs(p1),
            y => y.GuineaPigs["Pig D"].ShouldBeSameAs(p4)
        );
    }
}