using static Util.Fun.NumberConstants;

namespace Core.Gears.Settings;


public class SettingTest
{
    private class TestSettings : Settings
    {
        public TestSettings() : base("TestSettings") { }
    }

    private readonly TestSettings ourSettings = new TestSettings();


    [SetUp]
    public void ResetCounter() => ourSettings.ResetCounter();


    #region Byte Setting

    [Test]
    public void Byte_InitName()
    {
        var setting = new ByteSetting(ourSettings, "MyByte");
        setting.Name.ShouldBe("MyByte");
    }

    [Test]
    public void Byte_InitWithDefault()
    {
        var setting = new ByteSetting(ourSettings, "MyByte");
        setting.IsSet.ShouldBeFalse();
        setting.DefaultValue.ShouldBe(_0_);
        setting.Value.ShouldBe(_0_);
    }

    [Test]
    public void Byte_SetAndGet()
    {
        var setting = new ByteSetting(ourSettings, "MyByte");
        setting.Value = 26;
        setting.Value.ShouldBe(_26_);
        setting.IsSet.ShouldBeTrue();
    }

    [Test]
    public void Byte_SetAndGet_fromSetting()
    {
        var setting = new ByteSetting(ourSettings, "MyByte");
        setting.Value = 15;
        byte b = setting;
        b.ShouldBe(_15_);
    }

    [Test]
    public void Byte_Export()
    {
        var setting = new ByteSetting(ourSettings, "MyByte");
        setting.Export().ShouldBeNull();

        setting.Value = 26;
        setting.Export().ShouldBe("26");
    }

    [Test]
    public void Byte_Import_basic()
    {
        var setting = new ByteSetting(ourSettings, "MyByte");
        setting.Import("12", out var error);
        setting.Value.ShouldBe(_12_);
        error.ShouldBeNull();
    }

    [Test]
    public void Byte_Import_null()
    {
        var setting = new ByteSetting(ourSettings, "MyByte", 255);
        setting.Value = _13_;
        setting.Import(null, out var error);
        setting.Value.ShouldBe(_255_);
        error.ShouldBeNull();
    }

    #endregion



    #region Int Setting

    [Test]
    public void Int_InitWithDefault()
    {
        var setting = new IntSetting(ourSettings, "MyInt");
        setting.IsSet.ShouldBeFalse();
        setting.DefaultValue.ShouldBe(0);
        setting.Value.ShouldBe(0);
    }

    [Test]
    public void Int_InitWithDefaultNegative()
    {
        var setting = new IntSetting(ourSettings, "MyInt", -99);
        setting.IsSet.ShouldBeFalse();
        setting.DefaultValue.ShouldBe(-99);
        setting.Value.ShouldBe(-99);
    }

    [Test]
    public void Int_SetAndGet()
    {
        var setting = new IntSetting(ourSettings, "MyInt", -1);
        setting.Value = 186989;
        setting.Value.ShouldBe(186989);
        setting.IsSet.ShouldBeTrue();
    }

    [Test]
    public void Int_Export()
    {
        var setting = new IntSetting(ourSettings, "MyInt", -1);
        setting.Export().ShouldBeNull();

        setting.Value = 26;
        setting.Export().ShouldBe("26");
    }

    [Test]
    public void Int_Import_basic()
    {
        var setting = new IntSetting(ourSettings, "MyInt");
        setting.Import("26", out var error);
        setting.Value.ShouldBe(26);
        error.ShouldBeNull();
    }

    [Test]
    public void Int_Import_null()
    {
        var setting = new IntSetting(ourSettings, "MyInt", -100);
        setting.Value = 13;
        setting.Import(null, out var error);
        setting.Value.ShouldBe(-100);
        error.ShouldBeNull();
    }

    [Test]
    public void Int_Import_withPlus()
    {
        var setting = new IntSetting(ourSettings, "MyInt");
        setting.Import("+74", out var error);
        setting.Value.ShouldBe(+74);
        error.ShouldBeNull();
    }

    [Test]
    public void Int_Import_withMinus()
    {
        var setting = new IntSetting(ourSettings, "MyInt");
        setting.Import("-42", out var error);
        setting.Value.ShouldBe(-42);
        error.ShouldBeNull();
    }

    #endregion



    #region String Setting

    [Test]
    public void String_DefaultIsNull()
    {
        var setting = new StringSetting(ourSettings, "MyString");
        setting.IsSet.ShouldBeFalse();
        setting.Value.ShouldBeNull();
    }

    [Test]
    public void String_NullSetToNull()
    {
        var setting = new StringSetting(ourSettings, "MyString");
        ourSettings.Counter.ShouldBe(0);
        setting.Value = null;
        ourSettings.Counter.ShouldBe(0);
    }


    [Test]
    public void String_SetAndGet()
    {
        var setting = new StringSetting(ourSettings, "MyString");

        setting.Value = "Gray";

        string? s = setting;

        s.ShouldBe("Gray");
    }

    [Test]
    public void String_IsSet()
    {
        var setting = new StringSetting(ourSettings, "MyString");
        setting.IsSet.ShouldBeFalse();

        setting.Value = "White";
        setting.IsSet.ShouldBeTrue();

        setting.Value = null;
        setting.IsSet.ShouldBeFalse();
    }

    [Test]
    public void String_SetInvokesCounter()
    {
        var setting = new StringSetting(ourSettings, "MyString");

        int cnt1 = ourSettings.Counter;

        setting.Value = "A";

        ourSettings.Counter.ShouldBe(cnt1 + 1);
    }

    [Test]
    public void String_SetSame()
    {
        var setting = new StringSetting(ourSettings, "MyString");
        setting.Value = "AB";

        int cnt1 = ourSettings.Counter;
        setting.Value = "A" + "B"; // an equal string

        ourSettings.Counter.ShouldBe(cnt1);
    }

    [Test]
    public void String_Reset()
    {
        var setting = new StringSetting(ourSettings, "MyString");
        setting.Value = "AB";
        setting.Reset();
        setting.Value.ShouldBeNull();
    }

    [Test]
    public void String_Export()
    {
        var setting = new StringSetting(ourSettings, "MyString");
        setting.Export().ShouldBeNull();

        setting.Value = "TheThing";
        setting.Export().ShouldBe("TheThing");
    }

    [Test]
    public void String_Import()
    {
        var setting = new StringSetting(ourSettings, "MyString");
        setting.Import("GreatName", out var error);
        setting.Value.ShouldBe("GreatName");
        error.ShouldBeNull();
        ourSettings.Counter.ShouldBe(1);
    }

    #endregion



}

