namespace Util.Colors;

[TestFixture]
public class RgbTest
{

    [Test]
    public void Components()
    {
        Rgb rgb = Rgb.Of(0x123456);
        rgb.Red.ShouldBe((byte)0x12);
        rgb.Green.ShouldBe((byte)0x34);
        rgb.Blue.ShouldBe((byte)0x56);
    }

    [Test]
    public void ToStringHex()
    {
        Rgb rgb = Rgb.Of(0x2A3B4C);
        rgb.ToString().ShouldBe("2A3B4C");
    }

}
