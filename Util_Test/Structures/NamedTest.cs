using System.Collections.Generic;

namespace Util.Structures;


[TestFixture]
public class NamedTest
{

    [Test]
    public void Basic_Val()
    {
        var xx = new Named<byte>("MyByte", _26_);
        xx.Verify
        (
            x => x.Name.ShouldBe("MyByte"),
            x => x.Thing.ShouldBe(_26_)
        );
    }


    [Test]
    public void Equals_Val_Different()
    {
        var x = new Named<long>("First", 1L);
        var y = new Named<long>("Second", 2L);

        Verify
        (
            () => x.Equals(y).ShouldBeFalse(),
            () => y.Equals(x).ShouldBeFalse(),
            () => (x == y).ShouldBeFalse(),
            () => (x != y).ShouldBeTrue()
        );
    }

    [Test]
    public void Equals_Val_EqualNamesDifferentThings()
    {
        var x = new Named<long>("It", 1L);
        var y = new Named<long>("It", 2L);

        Verify
        (
            () => x.Equals(y).ShouldBeFalse(),
            () => y.Equals(x).ShouldBeFalse(),
            () => (x == y).ShouldBeFalse(),
            () => (x != y).ShouldBeTrue()
        );
    }

    [Test]
    public void Equals_Val_Equal()
    {
        var x = new Named<long>("It", 42L);
        var y = new Named<long>("It", 42L);

        Verify
        (
            () => x.Equals(y).ShouldBeTrue(),
            () => y.Equals(x).ShouldBeTrue(),
            () => (x == y).ShouldBeTrue(),
            () => (x != y).ShouldBeFalse()
        );
    }


    [Test]
    public void Deconstruct()
    {
        var x = _13_.WithName("TheByte");
        var (aName, aByte) = x;
        aName.ShouldBe("TheByte");
        aByte.ShouldBe(_13_);
    }


    [Test]
    public void AddToDictionary()
    {
        var x = 1234567890L.WithName("TheLongNumber");

        IDictionary<string, long> dictionary = new Dictionary<string, long>();

        dictionary.Add(x);

        dictionary.ShouldContainKeyAndValue("TheLongNumber", 1234567890L);
    }

}
