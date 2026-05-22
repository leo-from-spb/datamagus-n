using Avalonia.Controls;
using Avalonia.Input;
using Core.Interaction.Commands;
using Core.Services;
using Util.SystemStuff;

namespace DataMagus.Main.Main;

internal class MainMenu : Service
{
    private CommandRegistry   CommandRegistry;
    private KeyboardShortcuts KeyboardShortcuts;
    private MainWindow        Window;

    internal MainMenu(CommandRegistry commandRegistry, KeyboardShortcuts keyboardShortcuts, MainWindow window)
    {
        CommandRegistry   = commandRegistry;
        KeyboardShortcuts = keyboardShortcuts;
        Window            = window;
    }

    internal void SetupMenu()
    {
        var menuView = new NativeMenuItem("View");
        menuView.Menu = new NativeMenu();

        makeItem(MainCommands.SwitchToEasel, menuView.Menu);
        makeItem(MainCommands.SwitchToExplorer, menuView.Menu);
        makeSeparator(menuView.Menu);
        makeItem(MainCommands.ResetMainWindowPlace, menuView.Menu);

        Window.SetValue(NativeMenu.MenuProperty, new NativeMenu { menuView });
    }

    private void makeItem(string id, NativeMenu menu)
    {
        var command = CommandRegistry[id];
        if (command is null) return; // TODO LOG
        var shortcut = KeyboardShortcuts[id];
        var item = new NativeMenuItem
                   {
                       Header = command.MenuItemName ?? command.Name,
                       Command = command,
                       Gesture = shortcut,
                   };
        menu.Add(item);
        if (shortcut is not null && EnvironmentInfo.OS != OS.osMac)
            Window.KeyBindings.Add(new KeyBinding { Gesture = shortcut, Command = command });
    }

    private void makeSeparator(NativeMenu menu)
    {
        menu.Add(new NativeMenuItemSeparator());
    }

}
