using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Gui.Application.Easel;

public partial class EaselFrame : UserControl
{
    private Size PaperSize = new Size(210, 297);

    private const double CmDips = 96.0/2.54;
    private const double MmDips = 96.0/25.4;

    private double OuterFieldWidth = 25;
    private double OuterFieldWidthDips = 25 * MmDips;

    public EaselFrame()
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

        this.IsVisible = true;
    }
}

