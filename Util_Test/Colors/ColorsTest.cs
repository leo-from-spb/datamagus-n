namespace Util.Colors;

[TestFixture]
public class ColorsTest
{

    [Test]
    public void ConvertHueToRgb_Gray()
    {
        Colors.ConvertHslToRgb(Hue.BW, 0, 0  ).Value.ShouldBe(0x000000u); // black
        Colors.ConvertHslToRgb(Hue.BW, 0, 50 ).Value.ShouldBe(0x808080u); // gray
        Colors.ConvertHslToRgb(Hue.BW, 0, 100).Value.ShouldBe(0xFFFFFFu); // white
    }

    [Test]
    public void ConvertRgbToHsl_Etalons_Basic()
    {
        Colors.ConvertHslToRgb(Hue.Of(120), 100, 50).Value.ShouldBe(0x00FF00u); // lime
        Colors.ConvertHslToRgb(Hue.Of(120), 100, 25).Value.ShouldBe(0x008000u); // green
        Colors.ConvertHslToRgb(Hue.Of(180), 100, 50).Value.ShouldBe(0x00FFFFu); // aqua
        Colors.ConvertHslToRgb(Hue.Of(360), 100, 50).Value.ShouldBe(0xFF0000u); // red
        Colors.ConvertHslToRgb(Hue.Of(360), 100, 25).Value.ShouldBe(0x800000u); // maroon
        Colors.ConvertHslToRgb(Hue.Of(300), 100, 50).Value.ShouldBe(0xFF00FFu); // fuchsia
        Colors.ConvertHslToRgb(Hue.Of(300), 100, 25).Value.ShouldBe(0x800080u); // purple
    }

}
