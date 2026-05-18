using System;
using Avalonia;
using Avalonia.Controls;
using Core.Services;
using Core.Stationery;
using static Core.Stationery.DataMagusState.ApplicationMode;

namespace Gui.Application;

/// <summary>
/// This "Program" is for compatibility with Avalonia Tools only.
/// Run the Program in the <b>DataMagus_App</b> module.
/// </summary>
public static class Program
{
    /// <summary>
    /// For starting DataMagus with GUI, use the method <c>Main</c> in the DataMagus_App module instead.
    /// This method is to Avalonia design.
    /// </summary>
    /// <param name="args"></param>
    [STAThread]
    public static void Main(string[] args)
    {
        RunAvaloniaApp(args);
    }

    public static void RunAvaloniaApp(string[] args)
    {
        BuildAvaloniaApp()
           .StartWithClassicDesktopLifetime(args, ShutdownMode.OnMainWindowClose);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        if (DataMagusState.AppMode == dmamNone)
            PrepareStartInAvaloniaMode();
        return AppBuilder.Configure<App>()
                         .UsePlatformDetect()
                         .WithInterFont()
                         .LogToTrace();
    }


    private static void PrepareStartInAvaloniaMode()
    {
        DataMagusState.AppStarted(dmamAvalonia);
        CoreServiceMaster.Sunrise();
    }
}
