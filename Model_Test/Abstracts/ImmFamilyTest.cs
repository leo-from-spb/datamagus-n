using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Testing.Appliance.Assertions;
using Util.Extensions;

// ReSharper disable CoVariantArrayConversion
namespace Model.Abstracts;


[TestFixture]
public class ImmFamilyTest
{
    private readonly ImmTestRabbit   rabbitA;
    private readonly ImmTestRabbit   rabbitB;
    private readonly ImmTestRabbit   rabbitC;
    private readonly ImmTestRabbit[] rabbitsABC;

    public ImmFamilyTest()
    {
        rabbitA = new ImmTestRabbit(1001, 1, "Rabbit A");
        rabbitB = new ImmTestRabbit(1002, 1, "Rabbit B");
        rabbitC = new ImmTestRabbit(1003, 1, "Rabbit C");
        rabbitsABC = [rabbitA, rabbitB, rabbitC];
    }


    [Test]
    public void Basic()
    {
        var rabbitFamily = ImmFamily<TestRabbit>.Of(rabbitsABC);
        rabbitFamily.Verify(
            f => f.Any.ShouldBeTrue(),
            f => f.IsEmpty.ShouldBeFalse(),
            f => f.Count.ShouldBe(3)
        );
    }

    [Test]
    public void BasicAsEnumerable()
    {
        Family<TestRabbit>      rabbitFamily = ImmFamily<TestRabbit>.Of(rabbitsABC);
        IEnumerable<TestRabbit> enumerable   = rabbitFamily;
        TestRabbit[]            array        = enumerable.ToArray();
        array.ShouldBe(rabbitsABC);
    }

    [Test]
    public void ById()
    {

        var rabbitFamily = ImmFamily<TestRabbit>.Of(rabbitsABC);

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
        var rabbitFamily = ImmFamily<TestRabbit>.Of(rabbitsABC);
        var ids          = rabbitFamily.GetAllIds();

        ids.JoinToString(func: x => x.ToString()).ShouldBe("1001, 1002, 1003");
    }


    [Test]
    public void ToArray()
    {
        var rabbitFamily = ImmFamily<TestRabbit>.Of(rabbitsABC);

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
        var rabbitFamily = ImmFamily<TestRabbit>.Of(rabbitsABC);

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
        var rabbitFamily = ImmNamingFamily<TestRabbit>.Of(rabbitsABC);

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
        var rabbitNoNameX = new ImmTestRabbit(999, 1, null);
        var rabbitNoNameY = new ImmTestRabbit(998, 1, null);

        var rabbitFamily = ImmNamingFamily<TestRabbit>.Of(rabbitA, rabbitNoNameX, rabbitNoNameY, rabbitC);

        string[] names = rabbitFamily.GetAllNames();
        names.Verify(
            a => a.Length.ShouldBe(2),
            a => a[0].ShouldBe("Rabbit A"),
            a => a[1].ShouldBe("Rabbit C")
        );
    }
}
