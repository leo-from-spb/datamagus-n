using System;

namespace Gena.Code;

[TestFixture]
public class CodeBuilderTest
{
    private readonly string nl = Environment.NewLine;

    [Test]
    public void Text_1Line()
    {
        CodeBuilder cb = new CodeBuilder();
        cb.Text("One_Line");
        cb.Result.ShouldBe($"One_Line{nl}");
    }

    [Test]
    public void Text_3Lines()
    {
        string text   = $"First_Line{nl}Second_Line{nl}Third_Line";
        string result = $"First_Line{nl}Second_Line{nl}Third_Line{nl}";

        CodeBuilder cb = new CodeBuilder();
        cb.Text(text);
        cb.Result.ShouldBe(result);
    }

    [Test]
    public void Text_3LinesWithEOL()
    {
        string text   = $"First_Line{nl}Second_Line{nl}Third_Line{nl}";
        string result = $"First_Line{nl}Second_Line{nl}Third_Line{nl}";

        CodeBuilder cb = new CodeBuilder();
        cb.Text(text);
        cb.Result.ShouldBe(result);
    }

    [Test]
    public void Text_3LinesIndented()
    {
        string text   = $"First_Line{nl}Second_Line{nl}Third_Line";
        string result = $"\tFirst_Line{nl}\tSecond_Line{nl}\tThird_Line{nl}";

        CodeBuilder cb = new CodeBuilder();
        cb.Indent();
        cb.Text(text);
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


    [Test]
    public void CurLine_Basic()
    {
        CodeBuilder cb = new CodeBuilder();
        cb.CurLine.ShouldBe(1);
        cb.Phrase("Line 1");
        cb.CurLine.ShouldBe(2);
        cb.Phrase("Line 2");
        cb.CurLine.ShouldBe(3);
    }

    [Test]
    public void CurLine_NL()
    {
        CodeBuilder cb = new CodeBuilder();
        cb.CurLine.ShouldBe(1);
        cb.Phrase("Line 1");
        cb.CurLine.ShouldBe(2);
        cb.EmptyLine();
        cb.CurLine.ShouldBe(3);
    }

    [Test]
    public void CurLine_MultiLineText()
    {
        CodeBuilder cb = new CodeBuilder();
        cb.CurLine.ShouldBe(1);
        cb.Text("""
                Your time is limited,
                so don't waste it living someone else's life.
                Steve Jobs
                """);
        cb.CurLine.ShouldBe(4);
    }


    [Test]
    public void CurPos_1()
    {
        CodeBuilder cb = new CodeBuilder();
        cb.CurPos.ShouldBe(1);
        cb.Phrase("Something new");
        cb.CurPos.ShouldBe(1);
    }

    [Test]
    public void CurPos_Indent()
    {
        CodeBuilder cb = new CodeBuilder();
        cb.Indentation = "    ";

        cb.CurPos.ShouldBe(1);
        cb.Indent();
        cb.CurPos.ShouldBe(5);
        cb.Indent("// ");
        cb.CurPos.ShouldBe(8);
        cb.Unindent();
        cb.CurPos.ShouldBe(5);
        cb.Unindent();
        cb.CurPos.ShouldBe(1);
    }


    [Test]
    public void TextWidth_Basic()
    {
        CodeBuilder cb = new CodeBuilder();
        cb.TextWidth.ShouldBe(0);

        cb.Phrase("Good");
        cb.TextWidth.ShouldBe(4);
        cb.Phrase("Mood");
        cb.TextWidth.ShouldBe(4);
        cb.Phrase("Flood");
        cb.TextWidth.ShouldBe(5);
        cb.Phrase("!");
        cb.TextWidth.ShouldBe(5);
    }


    [Test]
    public void TextWidth_MultiLineText()
    {
        CodeBuilder cb = new CodeBuilder();
        cb.Text("""
                Be yourself;
                everyone else
                is already taken.
                Oscar Wilde
                """);
        cb.TextWidth.ShouldBe(17);
    }


}
