using System;
using System.Collections.Generic;
using Util.Extensions;

namespace Core.Gears.Settings;



/// <summary>
/// Abstract setting.
/// </summary>
public abstract class Setting
{
    private readonly Settings parent;

    public readonly string Name;

    protected Setting(Settings parent, string name)
    {
        this.parent = parent;
        this.Name   = name;
    }

    protected void increment() => parent.increment();

    public abstract void Reset();

    public abstract string? Export();

    public abstract void Import(string? str, out string? error);
}

/// <summary>
/// Generic setting.
/// </summary>
/// <typeparam name="V">type of the setting value.</typeparam>
public abstract class Setting<V> : Setting
{
    public readonly V DefaultValue;

    public V Value
    {
        get => value;
        set => SetValue(value);
    }
    protected V value;

    protected Setting(Settings parent, string name, V defaultValue)
        : base(parent, name)
    {
        this.DefaultValue = defaultValue;
        this.value        = defaultValue;
    }

    protected void SetValue(V value)
    {
        if (Is(value)) return;
        this.value = value;
        increment();
    }

    public virtual bool Is(V value) => EqualityComparer<V>.Default.Equals(value, DefaultValue);

    public bool IsSet => !Is(DefaultValue);

    public static implicit operator V (Setting<V> setting) => setting.value;

    public override void Reset()
    {
        if (!IsSet) return;
        this.value = DefaultValue;
        increment();
    }

    public override string? Export() => IsSet ? exportValue() : null;

    protected abstract string? exportValue();

    public override void Import(string? str, out string? error)
    {
        string? s = str.TrimNullize();
        if (s is not null)
        {
            try
            {
                importValue(s, out var newValue, out error);
                if (error == null) SetValue(newValue);
                else Reset();
            }
            catch (Exception exception)
            {
                Reset();
                error = $"Exception: {exception.Message}";
            }
        }
        else
        {
            Reset();
            error = null;
        }
    }

    protected abstract void importValue(string str, out V result, out string? error);

}



public class BoolSetting : Setting<bool>
{
    private static readonly HashSet<string> stringsOfFalse = ["0", "-", "false", "no"];
    private static readonly HashSet<string> stringsOfTrue  = ["1", "+", "true", "yes"];

    public BoolSetting(Settings parent, string name, bool defaultValue = false) : base(parent, name, defaultValue) { }

    public override bool Is(bool value) => this.value == value;

    protected override string? exportValue() => value ? "+" : "-";

    protected override void importValue(string str, out bool newValue, out string? error)
    {
        var s = str.ToLowerInvariant();
        if (s.Length > 0)
        {
            if (stringsOfFalse.Contains(s))
            {
                newValue = false;
                error    = null;
            }
            else if (stringsOfTrue.Contains(s))
            {
                newValue = true;
                error    = null;
            }
            else
            {
                newValue = DefaultValue;
                error    = "Cannot convert the string \"{str}\" into boolean";
            }
        }
        else
        {
            newValue = DefaultValue;
            error    = null;
        }
    }

    public override string ToString() => value ? "true" : "false";
}



public class ByteSetting : Setting<byte>
{
    public ByteSetting(Settings parent, string name, byte defaultValue = 0) : base(parent, name, defaultValue) { }

    public override bool Is(byte value) => this.value == value;

    protected override string? exportValue() => value.ToString();

    protected override void importValue(string str, out byte result, out string? error)
    {
        bool ok = byte.TryParse(str, out result);
        error = ok ? null : $"Cannot convert the string \"{str}\" into a byte";
    }

    public override string ToString() => value.ToString();
}



public class IntSetting : Setting<int>
{
    public IntSetting(Settings parent, string name, int defaultValue = 0) : base(parent, name, defaultValue) { }

    public override bool Is(int value) => this.value == value;

    protected override string? exportValue() => value.ToString();

    protected override void importValue(string str, out int result, out string? error)
    {
        bool ok = int.TryParse(str, out result);
        error = ok ? null : $"Cannot convert the string \"{str}\" into an integer value";
    }

    public override string ToString() => value.ToString();
}



public class StringSetting : Setting<string?>
{
    public StringSetting(Settings parent, string name) : base(parent, name, null) { }

    public override bool Is(string? value) => this.value == value;

    protected override string? exportValue() => value;

    protected override void importValue(string str, out string? result, out string? error)
    {
        result = str;
        error  = null;
    }

    public override string ToString() => value ?? "<empty-string>";
}





