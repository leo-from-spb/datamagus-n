using System.Diagnostics.CodeAnalysis;
using Core.Interaction.Commands;
using Core.Services;
using Gui.Application.Interaction.Commands;

namespace Gui.Application.Services;



public static class GuiServiceMaster
{

    [SuppressMessage("ReSharper", "UnusedVariable")]
    internal static void Sunrise()
    {
        var mill = HardServiceMill.GetTheMill();

        // get core services in order for providing them to the newly creating service
        var theCommandRegistry = ServiceMill.GetService<CommandRegistry>();

        // instantiate and register all services
        var theGuiCommands = mill.Register(new SimpleGuiCommands());

        // setup
        theGuiCommands.Sunrise(theCommandRegistry);
    }

}
