using Core.Interaction.Commands;
using Core.Services;
using DataMagus.Main.Interaction.Commands;
using DataMagus.Main.Main;
using NLog;

namespace DataMagus.Main.Services;

public static class GuiServiceMaster
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();


    public static void Startup()
    {
        Log.Debug("Gui Service Master started");

        var mill = BigServiceMill.GetTheMill();

        // get core services in order for providing them to the newly creating service
        var theCommandRegistry = ServiceMill.GetService<CommandRegistry>();

        // instantiate and register all services
        Log.Debug("Instantiate services...");
        var theGuiCommands       = mill.Register(new SimpleGuiCommands());
        var theKeyboardShortcuts = mill.Register(new KeyboardShortcuts());
        var theMainMenu =
            mill.Register(new MainMenu(theCommandRegistry, theKeyboardShortcuts,
                                       MainWindow.Instance));

        // setup
        Log.Debug("Set up services...");
        theGuiCommands.Sunrise(theCommandRegistry);
        theKeyboardShortcuts.Setup();
        theMainMenu.SetupMenu();

        Log.Debug("Gui services are initialized.");
    }
}
