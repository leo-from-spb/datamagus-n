using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Gui.Application.Workbenches;

namespace Gui.Application.Main;

public partial class MainWindow : Window
{
    internal Workbench? CurrentWorkbench;


    public MainWindow()
    {
        InitializeComponent(true, false); // don't attach the DevTools by default because it occupies the F12 key
        //Bounds = new Rect(0, 0, 860, 500);
        //Position = new PixelPoint(-1000, 500);
        SwitchToEasel();

        // if in the internal mode
        this.AttachDevTools(new KeyGesture(Key.F12, KeyModifiers.Control | KeyModifiers.Shift));
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
        if (CurrentWorkbench != null)
        {
            CurrentWorkbench.Deactivate();
            CurrentWorkbench = null;
        }
    }
}
