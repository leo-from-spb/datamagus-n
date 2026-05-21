using Avalonia.Input;

namespace DataMagus.Main.Workbenches;

public interface Workbench
{

    public void Activate();

    public void Deactivate();

    internal void HandleKeyDown(object? sender, KeyEventArgs e);

}
