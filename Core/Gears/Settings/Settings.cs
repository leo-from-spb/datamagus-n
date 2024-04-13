using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Gears.Settings;


public abstract class Settings
{
    public  IReadOnlyList<Setting>  AllSettings => allSettings ?? throw new Exception("Settings are not initialized yet");
    private IReadOnlyList<Setting>? allSettings;

    public  IReadOnlyDictionary<string, Setting>  AllSettingsByNames => allSettingsByNames ?? throw new Exception("Settings are not initialized yet");
    private IReadOnlyDictionary<string, Setting>? allSettingsByNames;

    public  int Counter => counter;
    private int counter = 0;


    public void Init()
    {
        allSettings = this.GetType().GetFields()
                          .Where(f => f.IsPublic && f.FieldType.IsAssignableTo(typeof(Setting)))
                          .Select(f => (f.GetValue(this) as Setting)!)
                          .ToArray();
        allSettingsByNames = AllSettings.ToDictionary(keySelector: setting => setting.Name);
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
