using Avalonia.Controls;
using Core.Interaction.Commands;

namespace Gui.Application.Main;

internal class MainMenu
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
    }

    private void makeSeparator(NativeMenu menu)
    {
        menu.Add(new NativeMenuItemSeparator());
    }

}
