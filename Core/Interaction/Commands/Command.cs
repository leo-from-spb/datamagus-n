using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Core.Interaction.Commands;



/// <summary>
/// Abstract command.
/// </summary>
public abstract class Command : ICommand
{
    public readonly string Id;

    public readonly string Name;

    public readonly string? MenuItemName;

    protected Command(string id, string name, string? menuItemName)
    {
        Id           = id;
        Name         = name;
        MenuItemName = menuItemName;
    }

    public abstract bool CanExecute(object? parameter);

    public abstract void Execute();

    public void Execute(object? parameter) => Execute();

    public abstract event EventHandler? CanExecuteChanged;

    public override string ToString() => Id;
}



public class BasicCommand : Command
{
    private readonly Action BasicAction;

    public BasicCommand(string id, string name, string? menuItemName, Action basicAction)
        : base(id, name, menuItemName)
    {
        BasicAction = basicAction;
    }

    internal void Setup()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public override bool CanExecute(object? parameter) => true;

    public override void Execute() => BasicAction();

    #pragma warning disable CS0067
    public override event EventHandler? CanExecuteChanged;
    #pragma warning restore CS0067
}



public class ObjectCommand<O> : Command
{
    private readonly Func<O>   ObtainObject;
    private readonly Action<O> Action;

    private bool ActiveStatus = false;

    public override event EventHandler? CanExecuteChanged;

    public event PropertyChangedEventHandler? PropertyChanged;


    public ObjectCommand(string id, string name, string? menuItemName, Func<O> obtainObject, Action<O> action)
        : base(id, name, menuItemName)
    {
        ObtainObject = obtainObject;
        Action       = action;
    }

    public void Refresh()
    {
        bool oldStatus = ActiveStatus;
        bool newStatus = ObtainObject() is not null;
        if (oldStatus != newStatus)
        {
            ActiveStatus = newStatus;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanExecute)));
        }
    }

    public override bool CanExecute(object? parameter) => ActiveStatus;

    public override void Execute()
    {
        O obj = ObtainObject();
        if (obj is not null)
            Action(obj);
    }
}





