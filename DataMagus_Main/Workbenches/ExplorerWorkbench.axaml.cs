using Avalonia.Controls;
using Avalonia.Input;

namespace DataMagus.Main.Workbenches;

public partial class ExplorerWorkbench : UserControl, Workbench
{
    public ExplorerWorkbench()
    {
        InitializeComponent();
    }

    public void Activate()
    {
        this.IsVisible = true;
    }

    public void Deactivate()
    {
        this.IsVisible = false;
    }

    public void HandleKeyDown(object? sender, KeyEventArgs e)
    {
        return;
    }

}

