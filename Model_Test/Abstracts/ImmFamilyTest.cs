using System.Collections.Generic;
using Model.Imp.Abstracts;
using Shouldly;
using Test.Appliance.Assertions;
using Util.Fun;

namespace Model.Test.Abstracts;


[TestFixture]
public class ImmFamilyTest
{
    private readonly ImmTestRabbit   rabbitA;
    private readonly ImmTestRabbit   rabbitB;
    private readonly ImmTestRabbit   rabbitC;
    private readonly ImmTestRabbit[] rabbits;

    public ImmFamilyTest()
    {
        rabbitA = new ImmTestRabbit(1001, "Rabbit A");
        rabbitB = new ImmTestRabbit(1002, "Rabbit B");
        rabbitC = new ImmTestRabbit(1003, "Rabbit C");
        rabbits = [rabbitA, rabbitB, rabbitC];
    }


    [Test]
    public void ById()
    {

        var rabbitFamily = new ImmFamily<TestRabbit>(rabbits);

        rabbitFamily.Verify
        (
            rs => rs.ById(1001).ShouldBe(rabbitA),
            rs => rs.ById(1002).ShouldBe(rabbitB),
            rs => rs.ById(1003).ShouldBe(rabbitC)
        );
    }

    [Test]
    public void GetAllIds()
    {
        var rabbitFamily = new ImmFamily<TestRabbit>(rabbits);
        var ids          = rabbitFamily.GetAllIds();

        ids.JoinToString(func: x => x.ToString()).ShouldBe("1001, 1002, 1003");
    }


    [Test]
    public void ToArray()
    {
        var rabbitFamily = new ImmFamily<TestRabbit>(rabbits);

        TestRabbit[] arr = rabbitFamily.ToArray();
        arr.Verify(
            a => a.Length.ShouldBe(3),
            a => a[0].ShouldBe(rabbitA),
            a => a[1].ShouldBe(rabbitB),
            a => a[2].ShouldBe(rabbitC)
        );
    }


    [Test]
    public void AsList()
    {
        var rabbitFamily = new ImmFamily<TestRabbit>(rabbits);

        IReadOnlyList<TestRabbit> list = rabbitFamily.AsList();
        list.Verify(
            a => a.Count.ShouldBe(3),
            a => a[0].ShouldBe(rabbitA),
            a => a[1].ShouldBe(rabbitB),
            a => a[2].ShouldBe(rabbitC)
        );
    }


    [Test]
    public void GetAllNames_All()
    {
        var rabbitFamily = new ImmNamingFamily<TestRabbit>(rabbits);

        string[] names = rabbitFamily.GetAllNames();
        names.Verify(
            a => a.Length.ShouldBe(3),
            a => a[0].ShouldBe("Rabbit A"),
            a => a[1].ShouldBe("Rabbit B"),
            a => a[2].ShouldBe("Rabbit C")
        );
    }


    [Test]
    public void GetAllNames_Partially()
    {
        var rabbitNoNameX = new ImmTestRabbit(999, null);
        var rabbitNoNameY = new ImmTestRabbit(998, null);

        var rabbitFamily = new ImmNamingFamily<TestRabbit>([rabbitA, rabbitNoNameX, rabbitNoNameY, rabbitC]);

        string[] names = rabbitFamily.GetAllNames();
        names.Verify(
            a => a.Length.ShouldBe(2),
            a => a[0].ShouldBe("Rabbit A"),
            a => a[1].ShouldBe("Rabbit C")
        );
    }
}
