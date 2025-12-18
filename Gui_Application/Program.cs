using System;
using Avalonia;
using Avalonia.Controls;

namespace Gui.Application;

/// <summary>
/// This "Program" is for compatibility with Avalonia Tools only.
/// Run the Program in the <b>DataMagus_App</b> module.
/// </summary>
public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
            RunAvaloniaApp(args);
    }

    private static void RunAvaloniaApp(string[] args)
    {
        BuildAvaloniaApp()
           .StartWithClassicDesktopLifetime(args, ShutdownMode.OnMainWindowClose);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
                     .UsePlatformDetect()
                     .WithInterFont()
                     .LogToTrace();
}
