using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Gears.Settings;


public abstract class Settings
{
    public readonly string SectionName;

    public  IReadOnlyList<Setting>  AllSettings => allSettings ?? throw new Exception("Settings are not initialized yet");
    private IReadOnlyList<Setting>? allSettings;

    public  IReadOnlyDictionary<string, Setting>  AllSettingsByNames => allSettingsByNames ?? throw new Exception("Settings are not initialized yet");
    private IReadOnlyDictionary<string, Setting>? allSettingsByNames;

    public  int Counter => counter;
    private int counter = 0;


    protected Settings(string sectionName)
    {
        SectionName = sectionName;
    }

    public void Init()
    {
        allSettings = this.GetType().GetFields()
                          .Where(f => f.IsPublic && f.FieldType.IsAssignableTo(typeof(Setting)))
                          .Select(f => (f.GetValue(this) as Setting)!)
                          .ToArray();
        allSettingsByNames = AllSettings.ToDictionary(keySelector: setting => setting.Name,
                                                      comparer: StringComparer.InvariantCultureIgnoreCase);
    }

    public void Export(StringBuilder b)
    {
        b.Append('[').Append(SectionName).Append(']').AppendLine();
        foreach (var setting in AllSettings)
        {
            string name  = setting.Name;
            string value = setting.Export();
            b.Append(name).Append(" = ").Append(value).AppendLine();
        }
    }


    public void ImportSetting(string name, string value, out string? error)
    {
        bool ok = AllSettingsByNames.TryGetValue(name, out var setting);
        if (!ok)
        {
            error = $"No such setting: {name}";
            return;
        }

        string? s = value.Trim();
        if (s.Length == 0) s = null;
        setting!.Import(s, out error);
    }




    internal void increment()
    {
        counter++;
    }

    public void ResetCounter()
    {
        counter = 0;
    }

}
