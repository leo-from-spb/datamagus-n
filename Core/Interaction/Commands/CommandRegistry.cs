using System;
using System.Collections.Generic;
using Core.Services;

namespace Core.Interaction.Commands;


[Service]
public abstract class CommandRegistry
{
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

