using Util.Fun;

namespace Util.Test.Fun;

using static Strings;


[TestFixture]
public class StringsTest
{
    [Test]
    public void With_Char_Basic()
    {
        string? str = "ABC";
        Verify
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
        Verify
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
        str.Verify
        (
            s => s.with("zz", "xx").ShouldBe("zzABCxx"),
            s => s.with(prefix: "--").ShouldBe("--ABC"),
            s => s.with(suffix: "--").ShouldBe("ABC--")
        );
    }
    
    [Test]
    public void With_Str_Null()
    {
        string? str = null;
        Verify
        (
           () => str.with("zz", "xx").ShouldBeNull(),
           () => str.with(prefix: "--").ShouldBeNull(),
           () => str.with(suffix: "--").ShouldBeNull()
        );
    }

    [Test]
    public void lastWord_basic() => Verify
    (
        "The last word",
        () => "TheLastWord".lastWord().ShouldBe("Word"),
        () => "Word".lastWord().ShouldBe("Word"),
        () => "word".lastWord().ShouldBe("word")
    );
}