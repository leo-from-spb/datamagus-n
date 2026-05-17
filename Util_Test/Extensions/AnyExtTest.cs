using System.Collections.Generic;

namespace Util.Extensions;

[TestFixture]
public class AnyExtTest
{
    private static readonly string[]        arrayOfPets = ["Cat", "Dog"];
    private static readonly HashSet<string> setOfPets   = ["Cat", "Dog"];


    [Test]
    public void IsEmpty()
    {
        long[] array1 = [1L];
        long[] array0 = [];

        array0.IsEmpty.ShouldBeTrue();
        array1.IsEmpty.ShouldBeFalse();
    }

    [Test]
    public void Some_Nothing_Basic()
    {
        long[]? array1 = [1L];
        long[]? array0 = [];

        array1.Some.ShouldBeTrue();
        array0.Some.ShouldBeFalse();

        array1.Nothing.ShouldBeFalse();
        array0.Nothing.ShouldBeTrue();
    }

    [Test]
    public void Some_Nothing_Null()
    {
        long[]? array = null;

        array.Some.ShouldBeFalse();
        array.Nothing.ShouldBeTrue();
    }


    [Test]
    public void IsIn_Basic() => Verify
    (
        () => "Cat".IsIn(setOfPets).ShouldBeTrue(),
        () => "Cow".IsIn(setOfPets).ShouldBeFalse(),
        () => "Cat".IsIn(arrayOfPets).ShouldBeTrue(),
        () => "Cow".IsIn(arrayOfPets).ShouldBeFalse()
    );

    [Test]
    public void IsIn_Null()
    {
        string? s = null;
        s.IsIn(setOfPets).ShouldBeFalse();
    }

    [Test]
    public void IsNotIn_Basic() => Verify
    (
        () => "Cow".IsNotIn(setOfPets).ShouldBeTrue(),
        () => "Cat".IsNotIn(setOfPets).ShouldBeFalse(),
        () => "Cow".IsNotIn(arrayOfPets).ShouldBeTrue(),
        () => "Cat".IsNotIn(arrayOfPets).ShouldBeFalse()
    );

}
