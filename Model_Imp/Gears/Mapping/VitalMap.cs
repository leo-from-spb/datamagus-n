using System;
using System.Collections.Generic;
using System.Diagnostics;
using NLog;
using Util.Collections;
using Util.Collections.Implementation;
using Util.Extensions;
using static Util.Fun.MoreFun;

namespace Model.Gears.Mapping;

/// <summary>
/// Map that stores elements by their Id and Version.
///
/// The type of Id and Version keys are already <c>uint</c>.
/// </summary>
/// <typeparam name="E">type of elements.</typeparam>
public class VitalMap<E> : CascadingMap<E>
    where E: class
{
    private record Layer
    (
        uint             Ver,
        ImmDict<uint, E> Dict
    )
    {
        public override string ToString() => $"Layer {Ver}: {Dict}";
    }



    private static readonly ImmDict<uint, E> EmptyDict = new EmptyDictionary<uint, E>();

    private Logger Log = LogManager.GetCurrentClassLogger();



    private readonly SortedList<uint, Layer> Layers = new();

    private uint FirstVer = 0u;
    private uint LastVer  = 0u;


    public IReadOnlyList<uint> ListAllVersions()
    {
        return Layers.Keys.ToImmList();
    }

    public IReadOnlyDictionary<uint,E>? GetVersionDict(uint ver)
    {
        bool ok = Layers.TryGetValue(ver, out var layer);
        return ok ? layer!.Dict : null;
    }


    public void AppendVersion(uint                          newVersion,
                              IReadOnlyDictionary<uint, E>? modified,
                              IReadOnlySet<uint>?           deleted)
    {
        RequireParam(nameof(newVersion), newVersion > LastVer);
        Require(modified.Some || deleted.Some);

        if (Layers.IsEmpty)
        {
            if (modified.Some)
            {
                var firstDict = modified.ToImmDict();
                var firstLayer = new Layer(FirstVer, firstDict);
                Layers.Add(newVersion, firstLayer);
                LastVer = FirstVer = newVersion;
            }
        }
        else
        {
            var lastDict = Layers.Some ? Layers[LastVer].Dict : EmptyDict;
            var newDict  = lastDict.Patch(modified, deleted);
            var newLayer = new Layer(LastVer, newDict);
            Layers.Add(newVersion, newLayer);
            LastVer = newVersion;
        }
    }

}
