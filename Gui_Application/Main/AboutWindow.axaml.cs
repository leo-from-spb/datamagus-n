using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace Gui.Application.Main;

public partial class AboutWindow : Window
{
    private bool         windowIsMowing = false;
    private PointerPoint windowOriginalPoint;
    
    public AboutWindow()
    {
        //Name = "About DataMagus";
        this.Width  = 300;
        this.Height = 200;
        InitializeComponent();
    }

    private void AboutSpace_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var ps = e.GetCurrentPoint(sender as Control).Properties;
        if (ps.IsRightButtonPressed)
        {
            windowIsMowing      = true;
            windowOriginalPoint = e.GetCurrentPoint(this);
        }
        else if (ps.IsLeftButtonPressed)
        {
            windowIsMowing = false;
            this.Close();
        }
    }


    private void AboutSpace_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!windowIsMowing) return;
        PointerPoint currentPoint = e.GetCurrentPoint(this);
        Position = new PixelPoint(Position.X + (int)(currentPoint.Position.X - windowOriginalPoint.Position.X),
                                  Position.Y + (int)(currentPoint.Position.Y - windowOriginalPoint.Position.Y));
    }

    private void AboutSpace_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        windowIsMowing = false;
    }

    private void AboutSpace_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape) Close();
    }
}

