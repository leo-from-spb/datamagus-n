using System;
using System.IO;
using Util.Extensions;
using Util.SystemStuff;

namespace Core.Gears.Settings;


internal class LocalSettingService : SettingService
{
    public SystemSettings    SystemSettings    { get; }
    public WorkspaceSettings WorkspaceSettings { get; }

    public LocalSettingService()
    {
        SystemSettings = EnvironmentInfo.OS switch
                         {
                             OS.osMac => new MacSystemSettings(),
                             _        => new UnknownOperatingSystemSettings()
                         };
        WorkspaceSettings = new LocalWorkspaceSettings();
    }

    public void SaveAllSettings()
    {
        SaveWorkspaceSettings();
    }

    private void SaveWorkspaceSettings()
    {
        SaveSettings(WorkspaceSettings, SystemSettings.PersonalSettingsPath, "Workspace.ini");
    }

    private void SaveSettings(AbstractSettings settings, string dirPath, string fileName)
    {
        var entries = settings.ListEntries();
        var text    = entries.JoinToString(e => e.ToString(), separator: Environment.NewLine, suffix: Environment.NewLine);

        Directory.CreateDirectory(dirPath);
        string fileFullPath = Path.Combine(dirPath, fileName);
        using (StreamWriter writer = new StreamWriter(fileFullPath))
        {
            writer.Write(text);
        }
    }

}


