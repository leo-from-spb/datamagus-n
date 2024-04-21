using System;
using System.Drawing;
using Core.Stationery;
using Testing.Appliance.FileSystem;

namespace Core.Gears.Settings;

[TestFixture]
public class LocalSettingServiceTest
{
    private TempDirectory TempWorkspaceDir;

    [OneTimeSetUp]
    public void Setup()
    {
        TempWorkspaceDir = new TempDirectory("Workspace-");
        //Environment.SetEnvironmentVariable(DataMagusEnvironmentVariables.EvarWorkspaceDir, TempWorkspaceDir.FullPath);
    }

    [OneTimeTearDown]
    public void Down()
    {
        Environment.SetEnvironmentVariable(DataMagusEnvironmentVariables.EvarConfigDir, null);
        TempWorkspaceDir.Dispose();
    }


    [Test]
    public void SaveAllSettings1()
    {
        var service = new LocalSettingService();
        service.Sunrise();
        service.WorkspaceSettings.MainWindowPlace = new Rectangle(100, 200, 300, 400);

        service.SaveAllSettings();
        service.LoadAllSettings();
    }

}
