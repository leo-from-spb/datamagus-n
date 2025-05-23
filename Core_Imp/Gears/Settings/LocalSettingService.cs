using System;
using System.IO;
using System.Text.RegularExpressions;
using Util.Extensions;
using static Util.Fun.RegexFun;

namespace Core.Gears.Settings;


internal class LocalSettingService : SettingService
{
    public SystemSettings    SystemSettings    { get; }
    public WorkspaceSettings WorkspaceSettings { get; }

    internal LocalSettingService()
    {
        SystemSettings    = RealSystemSettings.InstantiateSystemSettingsForCurrentOS();
        WorkspaceSettings = new LocalWorkspaceSettings();
    }

    internal void Sunrise()
    {
        ((RealSystemSettings)SystemSettings).Setup();
        LoadAllSettings();
    }

    internal void SaveAllSettings()
    {
        SaveWorkspaceSettings();
    }

    private void SaveWorkspaceSettings()
    {
        SaveSettings(WorkspaceSettings, SystemSettings.ActualComputerSettingsPath, "Workspace.ini");
    }

    private void SaveSettings(AbstractSettings settings, string dirPath, string fileName)
    {
        var entries = settings.ExportEntries();
        var text    = entries.JoinToString(e => e.ToString(), separator: Environment.NewLine, suffix: Environment.NewLine);

        Directory.CreateDirectory(dirPath);
        string fileFullPath = Path.Combine(dirPath, fileName);
        using (StreamWriter writer = new StreamWriter(fileFullPath))
        {
            writer.Write(text);
        }
    }


    internal void LoadAllSettings()
    {
        LoadWorkspaceSettings();
    }

    private void LoadWorkspaceSettings()
    {
        LoadSettings(WorkspaceSettings, SystemSettings.ActualComputerSettingsPath, "Workspace.ini");
    }

    private void LoadSettings(AbstractSettings settings, string dirPath, string fileName)
    {
        if (!Directory.Exists(dirPath)) return;
        string fileFullPath = Path.Combine(dirPath, fileName);
        if (!File.Exists(fileFullPath)) return;

        string[] lines = File.ReadAllLines(fileFullPath);
        ImportTextLines(settings, lines);
    }

    private static void ImportTextLines(AbstractSettings settings, string[] lines)
    {
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            if (line.IsEmpty()) continue;
            if (line.StartsWith('|')) continue; // comment

            var m = SettingLinePattern.Match(line);
            if (m.Success)
            {
                string name  = m.Groups[1].Value;
                string data  = m.Groups[2].Value;
                var    entry = new SettingPair(name, data);

                settings.ImportEntry(entry, out string? error);
                if (error is not null) Console.Error.WriteLine($"Error on line {i+1}: {error}");
            }
        }
    }


    private static readonly Regex SettingLinePattern = RegEx(@"(\w[\w\d]+) = (\S.*\S) (|.*)?$");
}


