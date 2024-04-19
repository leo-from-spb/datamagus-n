using System.Drawing;
using static Core.Gears.Settings.SettingsConversion;

namespace Core.Gears.Settings;


[TestFixture]
public class SettingsConversionTest
{

    [Test,
     TestCase("(10,20,30,40)", 10,20,30,40),
     TestCase("(10, 20, 30, 40) ", 10,20,30,40),
     TestCase(" (10,20,30,40) ", 10,20,30,40),
     TestCase("( 10 , 20 , 30 , 40 ) ", 10,20,30,40),
     TestCase("(+10,+20,+30,+40)", 10,20,30,40),
     TestCase("(-10,-20,-30,-40)", -10,-20,-30,-40),
     TestCase("(1000,2000,3000,4000)", 1000,2000,3000,4000)]
    public void ImportRectangle_Basic(string input, int x, int y, int w, int h)
    {
        string?    error;
        Rectangle? rectangle = ImportRectangle(input, out error);

        error.ShouldBeNull();
        rectangle.HasValue.ShouldBeTrue();
        rectangle!.Value.ShouldBe(new Rectangle(x, y, w, h));
    }


}
