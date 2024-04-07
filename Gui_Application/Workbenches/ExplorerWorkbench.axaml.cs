using Avalonia.Controls;
using Avalonia.Input;

namespace Gui.Application.Workbenches;

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

