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
        {
            MyBool   = new BoolSetting(this, "MyBool");
            MyByte   = new ByteSetting(this, "MyByte");
            MyInt    = new IntSetting(this, "MyInt");
            MyString = new StringSetting(this, "MyString");
            Init();
        }
    }


    [Test]
    public void M1()
    {
        var settings = new TestSettings();

        settings.AllSettingsByNames.Keys.ShouldContain("MyBool");
        settings.AllSettingsByNames.Keys.ShouldContain("MyByte");
        settings.AllSettingsByNames.Keys.ShouldContain("MyInt");
        settings.AllSettingsByNames.Keys.ShouldContain("MyString");
    }


}
