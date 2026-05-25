using System;
using System.Collections.Generic;
using Shouldly;
using static System.Math;

namespace Model.Gears.Mapping;

[TestFixture]
public class VitalMapTest
{
    private static readonly Random Rnd = new();

    [Test]
    public void Empty()
    {
        var vm = new VitalMap<string>();

        var versions = vm.ListAllVersions();

        versions.ShouldBeEmpty();
    }

    [Test]
    public void Map_1000()
    {
        const uint countOfVersions = 1000u;
        const int  countOfMod = 7;
        const int  countOfDel  = 3;

        var vm = new VitalMap<string>();

        var mod = new Dictionary<uint, string>();
        var del = new HashSet<uint>();

        var n = 10;

        for (uint v = 1u; v <= countOfVersions; v++)
        {
            mod.Clear();
            del.Clear();
            for (int j = 1; j <= Min(countOfMod,v); j++)
            {
                uint id = (uint)Rnd.Next(1,n);
                mod[id] = $"Element {id} version {v}";
            }
            for (int j = 1; j <= countOfDel; j++)
            {
                uint id = (uint)Rnd.Next(1,n);
                del.Add(id);
            }

            vm.AppendVersion(v, mod, del);
            n += 5;
        }

        for (uint v = 1u; v <= countOfVersions; v++)
        {
            vm.GetVersionDict(v);
        }
    }


}
