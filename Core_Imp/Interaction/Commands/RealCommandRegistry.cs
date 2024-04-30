using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Util.Extensions;

namespace Core.Interaction.Commands;


internal class RealCommandRegistry : CommandRegistry
{
    private readonly ConcurrentDictionary<string, Command> AllCommands = new();


    public override BasicCommand NewBasicCommand(string id, string name, string? menuItemName, Action action)
    {
        var command = new BasicCommand(id, name, menuItemName, action);
        AddCommand(command);
        return command;
    }

    public override ObjectCommand<O> NewObjectCommand<O>(string id, string name, string? menuItemName, Func<O> obtainObject, Action<O> action)
    {
        var command = new ObjectCommand<O>(id, name, menuItemName, obtainObject, action);
        AddCommand(command);
        return command;
    }

    private void AddCommand(Command command)
    {
        Debug.Assert(AllCommands.ContainsKey(command.Id) is false);
        AllCommands[command.Id] = command;
    }

    public override Command? Find(string id) => AllCommands.Get(id);

    public override IReadOnlyList<Command> ListAllCommands() => AllCommands.Values.ToList();

    internal void Sunset()
    {
        
    }
}
