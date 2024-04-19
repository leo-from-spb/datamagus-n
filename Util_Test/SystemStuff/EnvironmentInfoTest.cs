namespace Util.SystemStuff;

[TestFixture]
public class EnvironmentInfoTest
{

    [Test]
    public void OS_Detected()
    {
        EnvironmentInfo.OS.ShouldNotBe(OS.osUnknown);
    }

}
