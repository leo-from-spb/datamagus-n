using System.Collections.Generic;
using System.Drawing;
using Util.Structures;

namespace Core.Gears.Settings;


public class LocalWorkspaceSettings : WorkspaceSettings
{

    public Rectangle? MainWindowPlace { get; set; }


    public IEnumerable<Named<string>> ListEntries()
    {
        var list = new List<Named<string>>();

        if (MainWindowPlace != null) list.Add(SettingsConversion.ExportRectangle(MainWindowPlace.Value).WithName("MainWindowPlace"));

        return list;
    }


}
