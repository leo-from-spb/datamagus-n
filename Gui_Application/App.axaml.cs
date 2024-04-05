using System;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Gui.Application.Main;

namespace Gui.Application;

public partial class App : Avalonia.Application
{
    private MainWindow? myMainWindow = null;

    private AboutWindow? myAboutWindow = null;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        Name = "DataMagus";
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            this.myMainWindow  = new MainWindow();
            desktop.MainWindow = myMainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }


    public void ShowAbout()
    {
        if (myMainWindow == null) return;
        if (myAboutWindow != null) return;

        myAboutWindow = new AboutWindow();
        myAboutWindow.ShowDialog(myMainWindow);
        myAboutWindow.Closed += OnAboutWindowClosed;

    }

    private void ShowAboutMenuItem_OnClick(object? sender, EventArgs e)
    {
        ShowAbout();
    }

    private void OnAboutWindowClosed(object? sender, EventArgs e)
    {
        var aw = myAboutWindow;
        myAboutWindow =  null;
        if (aw != null) aw.Closed -= OnAboutWindowClosed;
    }
}
