namespace Util.Extensions;

[TestFixture]
public class StringExtTest
{
    [Test]
    public void With_Char_Basic()
    {
        string? str = "ABC";
        Verify
        (
            () => str.With('z', 'x').ShouldBe("zABCx"),
            () => str.With(prefix: '-').ShouldBe("-ABC"), 
            () => str.With(suffix: '-').ShouldBe("ABC-") 
        );
    }
    
    [Test]
    public void With_Char_Null()
    {
        string? str = null;
        Verify
        (
            () => str.With('z', 'x').ShouldBeNull(),
            () => str.With(prefix: '-').ShouldBeNull(),
            () => str.With(suffix: '-').ShouldBeNull()
        );
    }
    
    [Test]
    public void With_Str_Basic()
    {
        string? str = "ABC";
        str.Verify
        (
            s => s.With("zz", "xx").ShouldBe("zzABCxx"),
            s => s.With(prefix: "--").ShouldBe("--ABC"),
            s => s.With(suffix: "--").ShouldBe("ABC--")
        );
    }
    
    [Test]
    public void With_Str_Null()
    {
        string? str = null;
        Verify
        (
           () => str.With("zz", "xx").ShouldBeNull(),
           () => str.With(prefix: "--").ShouldBeNull(),
           () => str.With(suffix: "--").ShouldBeNull()
        );
    }

    [Test]
    public void LastWord_basic() => Verify
    (
        "The last word",
        () => "TheLastWord".LastWord().ShouldBe("Word"),
        () => "Word".LastWord().ShouldBe("Word"),
        () => "word".LastWord().ShouldBe("word")
    );

}