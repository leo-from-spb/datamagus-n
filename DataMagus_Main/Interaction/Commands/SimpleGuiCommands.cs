using Core.Interaction.Commands;
using DataMagus.Main.Main;

namespace DataMagus.Main.Interaction.Commands;


internal class SimpleGuiCommands : MainCommands
{



    internal void Sunrise(CommandRegistry commandRegistry)
    {
        // instantiate commands
        commandRegistry.NewBasicCommand(ShowAbout, "Show About", "About…", DoShowAbout);
        commandRegistry.NewBasicCommand(ResetMainWindowPlace, "Reset Main Window Place", null, MainWindow.Instance.ResetWindowPosition);
        commandRegistry.NewBasicCommand(SwitchToEasel, "Switch to Easel", null, DoSwitchToEasel);
        commandRegistry.NewBasicCommand(SwitchToExplorer, "Switch to Explorer", null, DoSwitchToExplorer);

    }






    private void DoShowAbout()
    {
        var mainWindow = MainWindow.Instance;
        var aboutWindow = new AboutWindow();
        aboutWindow.ShowDialog(mainWindow);
    }

    private void DoSwitchToEasel() => MainWindow.Instance.SwitchToEasel();

    private void DoSwitchToExplorer() => MainWindow.Instance.SwitchToExplorer();
}
