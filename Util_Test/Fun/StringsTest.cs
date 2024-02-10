namespace Util.Test.Fun;

using static Common.Fun.Strings;

[TestFixture]
public class StringsTest
{
    [Test]
    public void With_Char_Basic()
    {
        string? s = "ABC";
        Assert.Multiple(() =>
        {
            Assert.That(s.with('z', 'x'),    Is.EqualTo("zABCx"));
            Assert.That(s.with(prefix: '-'), Is.EqualTo("-ABC"));
            Assert.That(s.with(suffix: '-'), Is.EqualTo("ABC-"));
        });
    }
    
    [Test]
    public void With_Char_Null()
    {
        string? s = null;
        Assert.Multiple(() =>
        {
            Assert.That(s.with('z', 'x'),    Is.Null);
            Assert.That(s.with(prefix: '-'), Is.Null);
            Assert.That(s.with(suffix: '-'), Is.Null);
        });
    }
    
    [Test]
    public void With_Str_Basic()
    {
        string? s = "ABC";
        Assert.Multiple(() =>
        {
            Assert.That(s.with("zz", "xx"), Is.EqualTo("zzABCxx"));
            Assert.That(s.with(prefix: "--"), Is.EqualTo("--ABC"));
            Assert.That(s.with(suffix: "--"), Is.EqualTo("ABC--"));
        });
    }
    
    [Test]
    public void With_Str_Null()
    {
        string? s = null;
        Assert.Multiple(() =>
        {
            Assert.That(s.with("zz", "xx"), Is.Null);
            Assert.That(s.with(prefix: "--"), Is.Null);
            Assert.That(s.with(suffix: "--"), Is.Null);
        });
    }
}