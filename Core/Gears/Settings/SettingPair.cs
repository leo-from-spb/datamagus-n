namespace Core.Gears.Settings;


/// <summary>
/// Special intermediate record that is used when a setting entry is imported or exported.
/// </summary>
/// <param name="Name">the setting name.</param>
/// <param name="Thing">the value in the string presentation.</param>
public readonly record struct SettingPair (string Name, string? Thing)
{
    public override string ToString() => $"{Name} = {Thing ?? "null"}";
}
