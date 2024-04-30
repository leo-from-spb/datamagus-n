using Core.Interaction.Commands;
using Gui.Application.Main;

namespace Gui.Application.Interaction.Commands;


internal class SimpleGuiCommands : MainCommands
{



    internal void Sunrise(CommandRegistry commandRegistry)
    {
        // instantiate commands
        commandRegistry.NewBasicCommand(ShowAbout, "Show About", "About…", DoShowAbout);


    }






    private void DoShowAbout()
    {
        var mainWindow = MainWindow.Instance;
        var aboutWindow = new AboutWindow();
        aboutWindow.ShowDialog(mainWindow);
    }

}
