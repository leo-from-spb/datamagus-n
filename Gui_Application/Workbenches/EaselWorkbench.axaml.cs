using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Gui.Application.Main;

namespace Gui.Application.Workbenches;

public partial class EaselWorkbench : UserControl, Workbench
{
    private Size PaperSize = new Size(210, 297);

    private const double CmDips = 96.0/2.54;
    private const double MmDips = 96.0/25.4;

    private double OuterFieldWidth = 25;
    private double OuterFieldWidthDips = 25 * MmDips;

    private ScaleTransform myScaleTransform = new ScaleTransform(1.0, 1.0);

    private double myCurrentScale = 1.0;


    public EaselWorkbench()
    {
        InitializeComponent();

        Canva.Width  = (PaperSize.Width + 2 * OuterFieldWidth)*MmDips;
        Canva.Height = (PaperSize.Height + 2 * OuterFieldWidth)*MmDips;

        PageWhiteSurface.Width  = PaperSize.Width * MmDips;
        PageWhiteSurface.Height = PaperSize.Height * MmDips;
        PageBorder.Width  = PaperSize.Width * MmDips;
        PageBorder.Height = PaperSize.Height * MmDips;

        Canvas.SetLeft(PageWhiteSurface, OuterFieldWidthDips);
        Canvas.SetTop(PageWhiteSurface, OuterFieldWidthDips);
        Canvas.SetLeft(PageBorder, OuterFieldWidthDips);
        Canvas.SetTop(PageBorder, OuterFieldWidthDips);

        var pgd = new StreamGeometry();
        using (StreamGeometryContext ctx = pgd.Open())
        {
            for (int i = 1; i <= 20; i++)
            {
                double x  = OuterFieldWidthDips + i * CmDips;
                double y1 = OuterFieldWidthDips;
                double y2 = y1 + PaperSize.Height * MmDips;
                ctx.BeginFigure(new Point(x, y1), false);
                ctx.LineTo(new Point(x, y2));
            }
            for (int j = 1; j <= 29; j++)
            {
                double y  = OuterFieldWidthDips + j * CmDips;
                double x1 = OuterFieldWidthDips;
                double x2 = x1 + PaperSize.Width * MmDips;
                ctx.BeginFigure(new Point(x1, y), false);
                ctx.LineTo(new Point(x2, y));
            }
        }
        PageGrid.Data = pgd;

        Canva.RenderTransform = myScaleTransform;

        this.IsVisible = true;
    }


    public void Activate()
    {
        this.IsVisible = true;
        ShowScaleInStatusBar();
    }

    public void Deactivate()
    {
        this.IsVisible = false;

        MainWindow.Instance.ScaleLabel.Content = null;
    }

    public void HandleKeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.Key, e.KeyModifiers)
        {
            case (Key.D0, KeyModifiers.Control):
                SetScale(1);
                break;
            case (Key.OemPlus, KeyModifiers.Control):
            case (Key.Add, KeyModifiers.Control):
                SetScale(myCurrentScale * Math.Sqrt(2));
                break;
            case (Key.OemMinus, KeyModifiers.Control):
            case (Key.Subtract, KeyModifiers.Control):
                SetScale(myCurrentScale / Math.Sqrt(2));
                break;
            default:
                return;
        }
        e.Handled = true;
    }

    private void SetScale(double newScale)
    {
        double adjustedScale = Math.Round(newScale, 2);
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (myCurrentScale == adjustedScale) return;

        Canva.Width  = (PaperSize.Width + 2 * OuterFieldWidth) * MmDips * adjustedScale;
        Canva.Height = (PaperSize.Height + 2 * OuterFieldWidth) * MmDips * adjustedScale;

        myScaleTransform.ScaleX = adjustedScale;
        myScaleTransform.ScaleY = adjustedScale;
        myCurrentScale          = adjustedScale;

        ShowScaleInStatusBar();
    }

    private void ShowScaleInStatusBar()
    {
        string s = myCurrentScale switch
                   {
                       1.0   => "1",
                       > 1.0 => myCurrentScale.ToString(),
                       < 1.0 => "1 / " + Math.Round(1.0 / myCurrentScale, 2).ToString(),
                       _     => "???"
                   };
        MainWindow.Instance.ScaleLabel.Content = s;
    }

    private void Canva_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        return;
    }

    private void Canva_OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        if (e.KeyModifiers == KeyModifiers.Control)
        {
            double y = -e.Delta.Y;
            if (y != 0)
            {
                double m = Math.Pow(1.1, y);
                SetScale(myCurrentScale * m);
            }
            e.Handled = true;
            return;
        }
    }
}

