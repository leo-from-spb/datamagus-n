using Testing.Appliance.Assertions;

namespace Util.Collections.Implementation;

[TestFixture]
public class CollectionLogicTest
{
    private readonly long[] EmptyArrayOfLong = [];

    [Test]
    public void UnionSet_SetPlusSet_Basic()
    {
        var setA = Imm.SetOf(10L, 20L, 30L, 50L);
        var setB = Imm.SetOf(20L, 30L, 40L, 60L);
        var theUnion = setA + setB;
        theUnion.ShouldContainAll(10L, 20L, 30L, 40L, 50L, 60L);
        theUnion.Count.ShouldBe(6);
    }

    [Test]
    public void UnionSet_UnionSet_AisEmptySet()
    {
        var setA     = EmptyArrayOfLong.ToImmSet();
        var setB     = Imm.SetOf(10L, 20L);
        var theUnion = setA + setB;
        theUnion.ShouldContainAll(10L, 20L);
        theUnion.ShouldBeSameAs(setB);
    }

    [Test]
    public void UnionSet_UnionSet_BisEmptySet()
    {
        var setA     = Imm.SetOf(10L, 20L);
        var setB     = EmptyArrayOfLong.ToImmSet();
        var theUnion = setA + setB;
        theUnion.ShouldContainAll(10L, 20L);
        theUnion.ShouldBeSameAs(setA);
    }

    [Test]
    public void UnionSet_SetPlusSet_AinB()
    {
        var setA = Imm.SetOf(20L, 40L);
        var setB = Imm.SetOf(10L, 20L, 30L, 40L, 50L);
        var theUnion = setA + setB;
        theUnion.ShouldContainAll(10L, 20L, 30L, 40L, 50L);
        theUnion.ShouldBeSameAs(setB);
    }

    [Test]
    public void UnionSet_SetPlusSet_BinA()
    {
        var setA = Imm.SetOf(10L, 20L, 30L, 40L, 50L);
        var setB = Imm.SetOf(20L, 40L);
        var theUnion = setA + setB;
        theUnion.ShouldContainAll(10L, 20L, 30L, 40L, 50L);
        theUnion.ShouldBeSameAs(setA);
    }

}
