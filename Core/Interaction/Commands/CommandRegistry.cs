using System;
using System.Collections.Generic;
using Core.Services;

namespace Core.Interaction.Commands;


/// <summary>
/// Registry of all existing commands in the system.
/// </summary>
[Service]
public abstract class CommandRegistry
{
    public void Execute(string id)
    {
        var command = Find(id);
        if (command is null) return; // TODO LOG
        command.Execute();
    }

    public abstract BasicCommand NewBasicCommand(string  id,
                                                 string  name,
                                                 string? menuItemName,
                                                 Action  action);

    public abstract ObjectCommand<O> NewObjectCommand<O>(string    id,
                                                         string    name,
                                                         string?   menuItemName,
                                                         Func<O>   obtainObject,
                                                         Action<O> action);

    public Command? this [string id] { get => Find(id); }

    public abstract Command? Find(string id);

    public abstract IReadOnlyList<Command> ListAllCommands();
}

