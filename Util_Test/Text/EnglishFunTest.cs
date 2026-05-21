namespace Util.Text;

[TestFixture]
public class EnglishFunTest
{

    [Test]
    public void Plural_Basic()
    {
        Verify(
            () => "Apple".Plural.ShouldBe("Apples"),
            () => "City".Plural.ShouldBe("Cities"),
            () => "Bus".Plural.ShouldBe("Buses"),
            () => "Torch".Plural.ShouldBe("Torches"),
            () => "Dish".Plural.ShouldBe("Dishes"),
            () => "Box".Plural.ShouldBe("Boxes")
        );
    }

}
