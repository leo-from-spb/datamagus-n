using System.Drawing;
using Util.Structures;

namespace Core.Gears.Settings;

public interface AbstractSettings
{

    public Named<string?>[] ExportEntries();

    public void ImportEntry(Named<string> entry, out string? error);

}



public interface SystemSettings : AbstractSettings
{
    public string UserName             { get; }
    public string PersonalSettingsPath { get; }
}


public interface WorkspaceSettings : AbstractSettings
{

    public Rectangle? MainWindowPlace { get; set; }

}
