using System.Diagnostics.CodeAnalysis;
using Core.Interaction.Commands;
using Core.Services;
using DataMagus.Main.Interaction.Commands;
using DataMagus.Main.Main;

namespace DataMagus.Main.Services;



public static class GuiServiceMaster
{

    [SuppressMessage("ReSharper", "UnusedVariable")]
    internal static void Sunrise()
    {
        var mill = HardServiceMill.GetTheMill();

        // get core services in order for providing them to the newly creating service
        var theCommandRegistry = ServiceMill.GetService<CommandRegistry>();

        // instantiate and register all services
        var theGuiCommands       = mill.Register(new SimpleGuiCommands());
        var theKeyboardShortcuts = mill.Register(new KeyboardShortcuts());
        var theMainMenu          = mill.Register(new MainMenu(theCommandRegistry, theKeyboardShortcuts, MainWindow.Instance));

        // setup
        theGuiCommands.Sunrise(theCommandRegistry);
        theKeyboardShortcuts.Setup();
        theMainMenu.SetupMenu();
    }

}
