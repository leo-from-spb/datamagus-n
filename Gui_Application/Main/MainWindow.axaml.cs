using System;
using System.Drawing;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Core.Gears.Settings;
using Core.Services;
using Gui.Application.Workbenches;

namespace Gui.Application.Main;

public partial class MainWindow : Window
{
    private static MainWindow? instance = null;

    internal Workbench? CurrentWorkbench;

    public MainWindow()
    {
        InitializeComponent(true, false); // don't attach the DevTools by default because it occupies the F12 key
        instance = this;

        var settingService    = ServiceMill.GetService<SettingService>();
        var workspaceSettings = settingService.WorkspaceSettings;
        var mainWindowPlace   = workspaceSettings.MainWindowPlace;
        if (mainWindowPlace.HasValue)
        {
            var r = mainWindowPlace.Value;
            Bounds = new Rect(0, 0, r.Width, r.Height);
            Position = new PixelPoint(r.X, r.Y);
        }

        SwitchToEasel();

        // if in the internal mode
        this.AttachDevTools(new KeyGesture(Key.F12, KeyModifiers.Control | KeyModifiers.Shift));
    }

    internal static MainWindow Instance
    {
        get
        {
            var i = instance;
            if (i == null) throw new Exception("MainWindow is not initialized yet");
            return i;
        }
    }

    private void Window_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (CurrentWorkbench != null) {
            CurrentWorkbench.HandleKeyDown(sender, e);
            if (e.Handled) return;
        }
        switch (e.Key, e.KeyModifiers)
        {
            case (Key.F11, 0):
                SwitchToEasel();
                break;
            case (Key.F12, 0):
                SwitchToExplorer();
                break;
            default:
                return;
        }
        e.Handled = true;
    }

    public void SwitchToEasel()
    {
        Explorer.Deactivate();
        Easel.Activate();
        CurrentWorkbench = Easel;
    }

    public void SwitchToExplorer()
    {
        Easel.Deactivate();
        Explorer.Activate();
        CurrentWorkbench = Explorer;
    }

    private void Window_OnResized(object? sender, WindowResizedEventArgs e)
    {
        ShowPositionInStatusLine();
    }

    private void WindowBase_OnPositionChanged(object? sender, PixelPointEventArgs e)
    {
        ShowPositionInStatusLine();
    }

    private void ShowPositionInStatusLine()
    {
        StatusLine.Content = $"Position = {Position}; Bounds = {Bounds.Size}; ";
    }

    private void Window_OnClosing(object? sender, WindowClosingEventArgs e)
    {
        // save the current position
        var settingService = ServiceMill.GetService<SettingService>();
        settingService.WorkspaceSettings.MainWindowPlace =
            new Rectangle(this.Position.X, this.Position.Y,
                          (int)Math.Round(this.Bounds.Width), (int)Math.Round(this.Bounds.Height));

        // deactivate all
        if (CurrentWorkbench != null)
        {
            CurrentWorkbench.Deactivate();
            CurrentWorkbench = null;
        }
    }
}
