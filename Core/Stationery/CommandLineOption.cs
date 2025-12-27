using System;
using System.Linq;
using Util.Collections;
using Util.Collections.Implementation;
using Util.Extensions;

namespace Core.Stationery;


/// <summary>
/// One known command-line option.
/// </summary>
/// <param name="Code">the code in the command line (without the heading '-' character.)</param>
/// <param name="Description">short human-readable description.</param>
public record class CommandLineOption(string Code, string Description)
{
    public override string ToString() => $"{Code}: {Description}";
}


/// <summary>
/// Handled given command-line options.
/// </summary>
public class CommandLineOptions
{
    /// <summary>
    /// All possible options known to the application.
    /// </summary>
    public readonly ImmOrdSet<CommandLineOption> KnownOptions;

    /// <summary>
    /// The options specified by the user.
    /// </summary>
    public readonly ImmSet<CommandLineOption> ActualOptions;


    public CommandLineOptions(ImmOrdSet<CommandLineOption> knownOptions)
        : this(knownOptions, Environment.GetCommandLineArgs())
    { }

    public CommandLineOptions(ImmOrdSet<CommandLineOption> knownOptions, string[] args)
    {
        this.KnownOptions = knownOptions;

        if (args.Length > 1)
        {
            var knownOptionCodes = knownOptions.ToDictionary(k => k.Code).ToImmDict();  // imm: in one action
            var givenOptions = args
                              .Skip(1)
                              .Where(a => a.StartsWith('-'))
                              .Select(a => a[1..])
                              .ToArray();
            this.ActualOptions = givenOptions
                                .Select(code => knownOptionCodes.Find(code))
                                .Where(result => result.Ok)
                                .Select(result => result.Item)
                                .ToImmSet();
            var unknownOptions = givenOptions
                                .Where(code => code.IsNotIn(knownOptionCodes.Keys))
                                .ToArray();
            if (unknownOptions.IsNotEmpty())
            {
                var message = "Unknown options: " + unknownOptions.JoinToString();
                Console.Error.WriteLine(message);
            }
        }
        else
        {
            ActualOptions = EmptySet<CommandLineOption>.Instance;
        }
    }


    public bool Has(CommandLineOption option)
        => ActualOptions.Contains(option);

}