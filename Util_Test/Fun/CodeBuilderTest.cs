using Util.Common.Fun;

namespace Util.Test.Fun;

[TestFixture]
public class CodeBuilderTest
{
    [Test]
    public void Append_1Line()
    {
        CodeBuilder cb = new CodeBuilder();
        cb.Append("One_Line");
        Assert.That(cb.Result, Is.EqualTo("One_Line\n"));
    }
    
    [Test]
    public void Append_3Lines()
    {
        const string text   = "First_Line\nSecond_Line\nThird_Line";
        const string result = "First_Line\nSecond_Line\nThird_Line\n";

        CodeBuilder cb = new CodeBuilder();
        cb.Append(text);
        Assert.That(cb.Result, Is.EqualTo(result));
    }
    
    [Test]
    public void Append_3LinesWithEOL()
    {
        const string text   = "First_Line\nSecond_Line\nThird_Line\n";
        const string result = "First_Line\nSecond_Line\nThird_Line\n";

        CodeBuilder cb = new CodeBuilder();
        cb.Append(text);
        Assert.That(cb.Result, Is.EqualTo(result));
    }
    
    [Test]
    public void Append_3LinesIndented()
    {
        const string text   = "First_Line\nSecond_Line\nThird_Line";
        const string result = "\tFirst_Line\n\tSecond_Line\n\tThird_Line\n";

        CodeBuilder cb = new CodeBuilder();
        cb.Indent();
        cb.Append(text);
        cb.Unindent();
        Assert.That(cb.Result, Is.EqualTo(result));
    }

    [Test]
    public void Phrase_AllExist()
    {
        CodeBuilder cb = new CodeBuilder();
        cb.Phrase("AAA", "BBB", "CCC");
        Assert.That(cb.Result, Is.EqualTo("AAA BBB CCC\n"));        
    }

    [Test]
    public void Phrase_WithNulls()
    {
        CodeBuilder cb = new CodeBuilder();
        cb.Phrase("AAA", null, "CCC", "DDD", null, null, "FFF", "GGG");
        Assert.That(cb.Result, Is.EqualTo("AAA CCC DDD FFF GGG\n"));        
    }
}