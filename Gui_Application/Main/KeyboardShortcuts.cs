using System.Collections.Generic;
using Avalonia.Input;
using Core.Interaction.Commands;
using Util.Extensions;
using static Avalonia.Input.KeyModifiers;

namespace Gui.Application.Main;

public class KeyboardShortcuts
{

    private Dictionary<string, KeyGesture> CommandShortcuts = new();
    private Dictionary<KeyGesture, string> ShortcutCommands = new();

    public KeyGesture? this[string commandId] => CommandShortcuts.Get(commandId);

    public string? this[KeyGesture shortcut] => ShortcutCommands.Get(shortcut);


    internal void Setup()
    {
        Add(MainCommands.ResetMainWindowPlace, Key.F12, Alt | Shift);
        Add(MainCommands.SwitchToEasel, Key.F11);
        Add(MainCommands.SwitchToExplorer, Key.F12);
    }

    private void Add(string commandId, Key key, KeyModifiers modifiers = None)
    {
        var shortcut = new KeyGesture(key, modifiers);
        CommandShortcuts[commandId] = shortcut;
        ShortcutCommands[shortcut]  = commandId;
    }

}
