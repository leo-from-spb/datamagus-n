using System;

namespace Util.Fun;

[TestFixture]
public class CodeBuilderTest
{
    private string nl = Environment.NewLine;

    [Test]
    public void Append_1Line()
    {
        CodeBuilder cb = new CodeBuilder();
        cb.Append("One_Line");
        cb.Result.ShouldBe($"One_Line{nl}");
    }
    
    [Test]
    public void Append_3Lines()
    {
        string text   = $"First_Line{nl}Second_Line{nl}Third_Line";
        string result = $"First_Line{nl}Second_Line{nl}Third_Line{nl}";

        CodeBuilder cb = new CodeBuilder();
        cb.Append(text);
        cb.Result.ShouldBe(result);
    }
    
    [Test]
    public void Append_3LinesWithEOL()
    {
        string text   = $"First_Line{nl}Second_Line{nl}Third_Line{nl}";
        string result = $"First_Line{nl}Second_Line{nl}Third_Line{nl}";

        CodeBuilder cb = new CodeBuilder();
        cb.Append(text);
        cb.Result.ShouldBe(result);
    }
    
    [Test]
    public void Append_3LinesIndented()
    {
        string text   = $"First_Line{nl}Second_Line{nl}Third_Line";
        string result = $"\tFirst_Line{nl}\tSecond_Line{nl}\tThird_Line{nl}";

        CodeBuilder cb = new CodeBuilder();
        cb.Indent();
        cb.Append(text);
        cb.Unindent();
        cb.Result.ShouldBe(result);
    }

    [Test]
    public void Phrase_AllExist()
    {
        CodeBuilder cb = new CodeBuilder();
        cb.Phrase("AAA", "BBB", "CCC");
        cb.Result.ShouldBe($"AAA BBB CCC{nl}");
    }

    [Test]
    public void Phrase_WithNulls()
    {
        CodeBuilder cb = new CodeBuilder();
        cb.Phrase("AAA", null, "CCC", "DDD", null, null, "FFF", "GGG");
        cb.Result.ShouldBe($"AAA CCC DDD FFF GGG{nl}");
    }
}