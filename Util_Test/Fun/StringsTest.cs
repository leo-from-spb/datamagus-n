namespace Util.Test.Fun;

using static Common.Fun.Strings;

[TestFixture]
public class StringsTest
{
    [Test]
    public void With_Char_Basic()
    {
        string? str = "ABC";
        str.ShouldSatisfyAllConditions
        (
            () => str.with('z', 'x').ShouldBe("zABCx"),
            () => str.with(prefix: '-').ShouldBe("-ABC"), 
            () => str.with(suffix: '-').ShouldBe("ABC-") 
        );
    }
    
    [Test]
    public void With_Char_Null()
    {
        string? str = null;
        str.ShouldSatisfyAllConditions
        (
            () => str.with('z', 'x').ShouldBeNull(),
            () => str.with(prefix: '-').ShouldBeNull(),
            () => str.with(suffix: '-').ShouldBeNull()
        );
    }
    
    [Test]
    public void With_Str_Basic()
    {
        string? str = "ABC";
        str.ShouldSatisfyAllConditions
        (
            () => str.with("zz", "xx").ShouldBe("zzABCxx"),
            () => str.with(prefix: "--").ShouldBe("--ABC"),
            () => str.with(suffix: "--").ShouldBe("ABC--")
        );
    }
    
    [Test]
    public void With_Str_Null()
    {
        string? str = null;
        str.ShouldSatisfyAllConditions
        (
           () => str.with("zz", "xx").ShouldBeNull(),
           () => str.with(prefix: "--").ShouldBeNull(),
           () => str.with(suffix: "--").ShouldBeNull()
        );
    }

    [Test]
    public void lastWord_basic()
    {
        0.ShouldSatisfyAllConditions
        (
            () => "TheLastWord".lastWord().ShouldBe("Word"),
            () => "Word".lastWord().ShouldBe("Word"),
            () => "word".lastWord().ShouldBe("word")
        );
    }
}