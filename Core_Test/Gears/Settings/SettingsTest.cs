using System.Linq;
using System.Text;

namespace Core.Gears.Settings;


[TestFixture]
public class SettingsTest
{

    private class TestSettings : Settings
    {
        public readonly BoolSetting   MyBool;
        public readonly ByteSetting   MyByte;
        public readonly IntSetting    MyInt;
        public readonly StringSetting MyString;

        public TestSettings()
            : base("TestSettings")
        {
            MyBool   = new BoolSetting(this, "MyBool");
            MyByte   = new ByteSetting(this, "MyByte");
            MyInt    = new IntSetting(this, "MyInt");
            MyString = new StringSetting(this, "MyString");
            Init();
        }
    }


    [Test]
    public void SettingsShouldPresent()
    {
        var settings = new TestSettings();
        var names = settings.AllSettingsByNames.Keys.ToArray();
        names.ShouldContain("MyBool");
        names.ShouldContain("MyByte");
        names.ShouldContain("MyInt");
        names.ShouldContain("MyString");
    }


    [Test]
    public void Export_Basic()
    {
        var settings = new TestSettings();

        settings.MyBool.Value   = true;
        settings.MyByte.Value   = 26;
        settings.MyInt.Value    = 1234567890;
        settings.MyString.Value = "Paradox";

        StringBuilder b = new StringBuilder();
        settings.Export(b);
        string text = b.ToString();

        text.Verify
        (
            t => t.ShouldStartWith("[TestSettings]"),
            t => t.ShouldContain("MyBool = +"),
            t => t.ShouldContain("MyByte = 26"),
            t => t.ShouldContain("MyInt = 1234567890"),
            t => t.ShouldContain("MyString = Paradox")
        );
    }


}
