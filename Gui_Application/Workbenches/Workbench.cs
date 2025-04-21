using Avalonia.Input;

namespace Gui.Application.Workbenches;

public interface Workbench
{

    public void Activate();

    public void Deactivate();

    internal void HandleKeyDown(object? sender, KeyEventArgs e);

}
