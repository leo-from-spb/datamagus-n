using System.Drawing;

namespace Core.Gears.Settings;

[TestFixture]
public class LocalSettingServiceTest
{

    [Test]
    public void SaveAllSettings1()
    {
        var service = new LocalSettingService();
        service.WorkspaceSettings.MainWindowPlace = new Rectangle(100, 200, 300, 400);

        // service.SaveAllSettings(); // TODO configure test path
    }

}
