using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Core.Services;

namespace Gui.Application;

static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        CoreServiceMaster.Sunrise();
        try
        {
            RunAvaloniaApp(args);
        }
        finally
        {
            Thread.Sleep(1);
            CoreServiceMaster.Shutdown();
        }
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
