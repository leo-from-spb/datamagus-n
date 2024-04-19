using System.Collections.Generic;
using System.Drawing;
using Util.Structures;

namespace Core.Gears.Settings;

public interface AbstractSettings
{

    public IEnumerable<Named<string>> ListEntries();

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
