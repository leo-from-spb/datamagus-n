namespace Util.Extensions;


[TestFixture]
public class CollectionExtTest
{

    [Test]
    public void JoinToString_Strings_Default()
    {
        string[] strings = ["Einz", "Zwei", "Drei"];
        strings.JoinToString().ShouldBe("Einz, Zwei, Drei");
    }

    [Test]
    public void JoinToString_Strings_SpecifyAllOptions()
    {
        string[] strings = ["Einz", "Zwei", "Drei"];
        strings.JoinToString(separator: ":", prefix: "<<", suffix: ">>", empty: "NO")
               .ShouldBe("<<Einz:Zwei:Drei>>");
    }

    [Test]
    public void JoinToString_Strings_WhenEmpty()
    {
        string[] strings = [];
        strings.JoinToString(empty: "is_empty")
               .ShouldBe("is_empty");
    }

    [Test]
    public void JoinToString_Longs()
    {
        long[] longs = [13L, 44L, 186L, 989L];
        longs.JoinToString(func: x => x.ToString(), separator: "+", prefix: "<<", suffix: ">>")
             .ShouldBe("<<13+44+186+989>>");
    }


}
