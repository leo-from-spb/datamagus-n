// ReSharper disable ConvertToConstant.Local
namespace Util.Extensions;

[TestFixture]
public class StringExtTest
{
    [Test]
    public void PartBefore_Basic()
    {
        string? text = "One|Two|Three";
        var     part = text.PartBefore('|');
        part.ShouldBe("One");
    }

    [Test]
    public void PartBefore_Trim()
    {
        string? text = " One | Two | Three ";
        var     part = text.PartBefore('|', trim: true);
        part.ShouldBe("One");
    }

    [Test]
    public void PartBefore_NoMarker()
    {
        string? text = "Something unexpectable";
        var     part = text.PartBefore('|', whenNoMarker: "X");
        part.ShouldBe("X");
    }

    [Test]
    public void PartAfter_Basic()
    {
        string? text = "One|Two|Three";
        var     part = text.PartAfter('|');
        part.ShouldBe("Two|Three");
    }

    [Test]
    public void PartAfter_Trim()
    {
        string? text = " One | Two | Three ";
        var     part = text.PartAfter('|', trim: true);
        part.ShouldBe("Two | Three");
    }

    [Test]
    public void PartAfter_NoMarker()
    {
        string? text = "Something unexpectable";
        var     part = text.PartAfter('|', whenNoMarker: "X");
        part.ShouldBe("X");
    }

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
    public void FirstChar_Last_Char()
    {
        string? strN = null, strE = "", strX = "Y -- Z";
        Verify
        (
            () => strN.FirstChar().ShouldBe('\0'),
            () => strN.FirstChar('#').ShouldBe('#'),
            () => strE.FirstChar().ShouldBe('\0'),
            () => strE.FirstChar('#').ShouldBe('#'),
            () => strX.FirstChar().ShouldBe('Y'),
            () => strN.LastChar().ShouldBe('\0'),
            () => strN.LastChar('#').ShouldBe('#'),
            () => strE.LastChar().ShouldBe('\0'),
            () => strE.LastChar('#').ShouldBe('#'),
            () => strX.LastChar().ShouldBe('Z')
        );
    }


    [Test]
    public void LastWord_basic() => Verify
    (
        "The last word",
        () => "TheLastWord".LastWord.ShouldBe("Word"),
        () => "Word".LastWord.ShouldBe("Word"),
        () => "word".LastWord.ShouldBe("word")
    );


    [Test]
    public void IsNotEmpty_IsNotBlank_IsEmpty()
    {
        string? strN = null, strE = "", strB = "   \t ", strX = "Something here";
        Verify
        (
            () => strN.IsNotEmpty.ShouldBeFalse(),
            () => strE.IsNotEmpty.ShouldBeFalse(),
            () => strB.IsNotEmpty.ShouldBeTrue(),
            () => strX.IsNotEmpty.ShouldBeTrue(),
            () => strN.IsNotBlank.ShouldBeFalse(),
            () => strE.IsNotBlank.ShouldBeFalse(),
            () => strB.IsNotBlank.ShouldBeFalse(),
            () => strX.IsNotBlank.ShouldBeTrue(),
            () => strN.IsEmpty.ShouldBeTrue(),
            () => strE.IsEmpty.ShouldBeTrue(),
            () => strB.IsEmpty.ShouldBeFalse(),
            () => strX.IsEmpty.ShouldBeFalse()
        );
    }


    /// <summary>
    /// This test tests in fact nothing;
    /// it's purpose is to show that there are no inspection "possible nul reference"
    /// because the argument "this" is marked with the special attribute.
    /// </summary>
    [Test]
    public void IsNotEmpty_IsNotBlank_IsEmpty_Annotated()
    {
        string? str1 = "ABC".TakeIf(true);
        if (str1.IsNotEmpty)
        {
            char c = str1[1]; // expecting no inspection
        }

        string? str2 = "ABC".TakeIf(true);
        if (str2.IsNotBlank)
        {
            char c = str2[1]; // expecting no inspection
        }

        string? str3 = "ABC".TakeIf(false);
        if (!str3.IsEmpty)
        {
            char c = str3[1]; // expecting no inspection
        }
    }


    [Test]
    public void Nullize_TrimNullize()
    {
        string? strN = null, strE = "", strB = " \t ", strX = "Something here";
        Verify
        (
            () => strN.Nullize().ShouldBeNull(),
            () => strE.Nullize().ShouldBeNull(),
            () => strB.Nullize().ShouldBe(strB),
            () => strX.Nullize().ShouldBe(strX),
            () => strN.TrimNullize().ShouldBeNull(),
            () => strE.TrimNullize().ShouldBeNull(),
            () => strB.TrimNullize().ShouldBeNull(),
            () => strX.TrimNullize().ShouldBe(strX)
        );
    }
}