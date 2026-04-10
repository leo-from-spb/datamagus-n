using System;
using System.Collections.Generic;
using System.Linq;

namespace Util.Collections.Implementation;

[TestFixture]
public class ImmutablePatchedDictTest
{
    private readonly ImmDict<ulong,string> OriginalDict =
        new Dictionary<ulong,string> { {1uL, "One"}, {2uL, "Two"}, {3uL, "Three"}, {6uL, "Six"}, {7uL, "Seven"}, {8uL, "Eight"}, {9uL, "Nine"} }
           .ToImmDict();

    private readonly ImmDict<ulong,string> PatchDict1 =
        new Dictionary<ulong,string> { {2uL, "ExtraTwo"}, {5uL, "ExtraFive"}, {8uL, "ExtraEight"} }
           .ToImmDict();

    private readonly ImmSet<ulong> RemovedKeys1 = Imm.SetOf(3uL, 4uL, 6uL);

    private readonly ImmDict<ulong,string> PatchDict2 =
        new Dictionary<ulong,string> { {5uL, "SuperFive"}, {10uL, "SuperTen"}, {11uL, "SuperEleven"} }
           .ToImmDict();

    private readonly ImmSet<ulong> RemovedKeys2 = Imm.SetOf(1uL, 99uL);


    [Test]
    public void P1_BasicProperties()
    {
        var patchedDict = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        patchedDict.Verify(
            pd => pd.Imp.CascadingLevel.ShouldBe(_2_),
            pd => pd.Any.ShouldBeTrue(),
            pd => pd.IsEmpty.ShouldBeFalse(),
            pd => pd.Count.ShouldBe(6)
        );
    }

    [Test]
    public void P1_Find()
    {
        var patchedDict = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        patchedDict.Verify(
            pd => pd.Find(1uL).ShouldBe(new Found<string>(true, "One")),
            pd => pd.Find(2uL).ShouldBe(new Found<string>(true, "ExtraTwo")),
            pd => pd.Find(3uL).ShouldBe(Found<string>.NotFound),
            pd => pd.Find(4uL).ShouldBe(Found<string>.NotFound),
            pd => pd.Find(5uL).ShouldBe(new Found<string>(true, "ExtraFive")),
            pd => pd.Find(6uL).ShouldBe(Found<string>.NotFound),
            pd => pd.Find(7uL).ShouldBe(new Found<string>(true, "Seven")),
            pd => pd.Find(8uL).ShouldBe(new Found<string>(true, "ExtraEight")),
            pd => pd.Find(9uL).ShouldBe(new Found<string>(true, "Nine"))
        );
    }

    [Test]
    public void P1_ContainsKey()
    {
        var patchedDict = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        patchedDict.Verify(
            pd => pd.ContainsKey(1uL).ShouldBeTrue(),
            pd => pd.ContainsKey(2uL).ShouldBeTrue(),
            pd => pd.ContainsKey(3uL).ShouldBeFalse(),
            pd => pd.ContainsKey(4uL).ShouldBeFalse(),
            pd => pd.ContainsKey(5uL).ShouldBeTrue(),
            pd => pd.ContainsKey(6uL).ShouldBeFalse(),
            pd => pd.ContainsKey(7uL).ShouldBeTrue(),
            pd => pd.ContainsKey(8uL).ShouldBeTrue(),
            pd => pd.ContainsKey(9uL).ShouldBeTrue()
        );
    }

    [Test]
    public void P1_KeySet_BasicProperties()
    {
        var patchedDict = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        var keys = patchedDict.Keys;
        keys.Verify(
            ks => ks.Imp.CascadingLevel.ShouldBe(_3_),
            ks => ks.Any.ShouldBeTrue(),
            ks => ks.IsEmpty.ShouldBeFalse(),
            ks => ks.Count.ShouldBe(6)
        );
    }

    [Test]
    public void P1_KeySet_Contains()
    {
        var patchedDict = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        var keys = patchedDict.Keys;
        keys.Verify(
            ks => ks.Contains(1uL).ShouldBeTrue(),
            ks => ks.Contains(2uL).ShouldBeTrue(),
            ks => ks.Contains(3uL).ShouldBeFalse(),
            ks => ks.Contains(4uL).ShouldBeFalse(),
            ks => ks.Contains(5uL).ShouldBeTrue(),
            ks => ks.Contains(6uL).ShouldBeFalse(),
            ks => ks.Contains(7uL).ShouldBeTrue(),
            ks => ks.Contains(8uL).ShouldBeTrue(),
            ks => ks.Contains(9uL).ShouldBeTrue()
        );
    }

    [Test]
    public void P1_KeySet_Enumerator()
    {
        var patchedDict = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        var keySet      = patchedDict.Keys;
        var keys        = keySet.ToArray();
        keys.ShouldBe([/* first the keys from the original dict */ 1uL, 2uL, 7uL, 8uL, 9uL,
                       /* then the key from the patch */  5uL]);
    }

    [Test]
    public void P1_Values_Enumerator()
    {
        var patchedDict = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        var values      = patchedDict.Values;
        var vals        = values.ToArray();
        vals.ShouldBe([/* first the original values patched */ "One", "ExtraTwo", "Seven", "ExtraEight", "Nine",
                       /* then the value from the patch */  "ExtraFive"]);
    }

    [Test]
    public void P1_Entries_Enumerator()
    {
        var patchedDict = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        var entries     = patchedDict.Entries;
        var es          = entries.ToArray();
        es.ShouldBe([/* first the original entries patched */
                        KV(1, "One"), KV(2, "ExtraTwo"), KV(7, "Seven"), KV(8, "ExtraEight"), KV(9, "Nine"),
                     /* then the entry from the patch */
                        KV(5, "ExtraFive")]);
    }


    [Test]
    public void P2_BasicProperties()
    {
        var p1Dict = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        var p2Dict = new ImmutablePatchedDict<ulong,string>(p1Dict, PatchDict2, RemovedKeys2);
        p2Dict.Verify(
            pd => pd.Imp.CascadingLevel.ShouldBe(_3_),
            pd => pd.Any.ShouldBeTrue(),
            pd => pd.IsEmpty.ShouldBeFalse(),
            pd => pd.Count.ShouldBe(7)
        );
    }

    [Test]
    public void P2_Find()
    {
        var p1Dict = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        var p2Dict = new ImmutablePatchedDict<ulong,string>(p1Dict, PatchDict2, RemovedKeys2);
        p2Dict.Verify(
            pd => pd.Find(1uL).ShouldBe(Found<string>.NotFound),
            pd => pd.Find(2uL).ShouldBe(new Found<string>(true, "ExtraTwo")),
            pd => pd.Find(3uL).ShouldBe(Found<string>.NotFound),
            pd => pd.Find(4uL).ShouldBe(Found<string>.NotFound),
            pd => pd.Find(5uL).ShouldBe(new Found<string>(true, "SuperFive")),
            pd => pd.Find(6uL).ShouldBe(Found<string>.NotFound),
            pd => pd.Find(7uL).ShouldBe(new Found<string>(true, "Seven")),
            pd => pd.Find(8uL).ShouldBe(new Found<string>(true, "ExtraEight")),
            pd => pd.Find(9uL).ShouldBe(new Found<string>(true, "Nine")),
            pd => pd.Find(10uL).ShouldBe(new Found<string>(true, "SuperTen")),
            pd => pd.Find(11uL).ShouldBe(new Found<string>(true, "SuperEleven"))
        );
    }

    [Test]
    public void P2_ContainsKey()
    {
        var p1Dict = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        var p2Dict = new ImmutablePatchedDict<ulong,string>(p1Dict, PatchDict2, RemovedKeys2);
        p2Dict.Verify(
            pd => pd.ContainsKey(1uL).ShouldBeFalse(),
            pd => pd.ContainsKey(2uL).ShouldBeTrue(),
            pd => pd.ContainsKey(3uL).ShouldBeFalse(),
            pd => pd.ContainsKey(4uL).ShouldBeFalse(),
            pd => pd.ContainsKey(5uL).ShouldBeTrue(),
            pd => pd.ContainsKey(6uL).ShouldBeFalse(),
            pd => pd.ContainsKey(7uL).ShouldBeTrue(),
            pd => pd.ContainsKey(8uL).ShouldBeTrue(),
            pd => pd.ContainsKey(9uL).ShouldBeTrue(),
            pd => pd.ContainsKey(10uL).ShouldBeTrue(),
            pd => pd.ContainsKey(11uL).ShouldBeTrue()
        );
    }

    [Test]
    public void P2_KeySet_Enumerator()
    {
        var p1Dict = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        var p2Dict = new ImmutablePatchedDict<ulong,string>(p1Dict, PatchDict2, RemovedKeys2);
        var keySet = p2Dict.Keys;
        var keys   = keySet.ToArray();
        keys.ShouldBe([/* first the keys from the original dict */ 2uL, 7uL, 8uL, 9uL,
                       /* then the key from the 1st patch */  5uL,
                       /* then the key from the 2nd patch */  10uL, 11uL]);
    }

    [Test]
    public void P2_Values_Enumerator()
    {
        var p1Dict = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        var p2Dict = new ImmutablePatchedDict<ulong,string>(p1Dict, PatchDict2, RemovedKeys2);
        var values = p2Dict.Values;
        var vals   = values.ToArray();
        vals.ShouldBe([/* first the original values patched */ "ExtraTwo", "Seven", "ExtraEight", "Nine",
                       /* then the value from the 1st patch */ "SuperFive",
                       /* then the value from the 2nd patch */ "SuperTen", "SuperEleven"]);
    }

    [Test]
    public void P2_Entries_Enumerator()
    {
        var p1Dict  = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        var p2Dict  = new ImmutablePatchedDict<ulong,string>(p1Dict, PatchDict2, RemovedKeys2);
        var entries = p2Dict.Entries;
        var es      = entries.ToArray();
        es.ShouldBe([/* first the original entries patched */
                        KV(2, "ExtraTwo"), KV(7, "Seven"), KV(8, "ExtraEight"), KV(9, "Nine"),
                     /* then the entry from the patch */
                        KV(5, "SuperFive"),
                     /* then the entry from the patch */
                        KV(10, "SuperTen"), KV(11, "SuperEleven")]);
    }



    // KeySet set operations \\

    [Test]
    public void P1_KeySet_IsSubsetOf()
    {
        var patchedDict = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        var keys = patchedDict.Keys;
        // keys = {1, 2, 5, 7, 8, 9}
        keys.Verify(
            ks => ks.IsSubsetOf(new ulong[] { 1, 2, 5, 7, 8, 9 }).ShouldBeTrue(),          // equal set
            ks => ks.IsSubsetOf(new ulong[] { 1, 2, 3, 5, 6, 7, 8, 9 }).ShouldBeTrue(),    // superset
            ks => ks.IsSubsetOf(new ulong[] { 1, 2, 5, 7, 8 }).ShouldBeFalse(),            // missing 9
            ks => ks.IsSubsetOf(new ulong[] { 1, 2, 3, 4 }).ShouldBeFalse(),               // mostly different
            ks => ks.IsSubsetOf(Array.Empty<ulong>()).ShouldBeFalse()                      // empty
        );
    }

    [Test]
    public void P1_KeySet_IsProperSubsetOf()
    {
        var patchedDict = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        var keys = patchedDict.Keys;
        keys.Verify(
            ks => ks.IsProperSubsetOf(new ulong[] { 1, 2, 5, 7, 8, 9 }).ShouldBeFalse(),      // equal — not proper
            ks => ks.IsProperSubsetOf(new ulong[] { 1, 2, 5, 7, 8, 9, 10 }).ShouldBeTrue(),   // one extra element
            ks => ks.IsProperSubsetOf(new ulong[] { 1, 2, 5, 7, 8 }).ShouldBeFalse(),         // missing 9
            ks => ks.IsProperSubsetOf(Array.Empty<ulong>()).ShouldBeFalse()                   // empty
        );
    }

    [Test]
    public void P1_KeySet_IsSupersetOf()
    {
        var patchedDict = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        var keys = patchedDict.Keys;
        keys.Verify(
            ks => ks.IsSupersetOf(new ulong[] { 1, 2, 5, 7, 8, 9 }).ShouldBeTrue(),      // equal set
            ks => ks.IsSupersetOf(new ulong[] { 1, 5, 9 }).ShouldBeTrue(),               // subset
            ks => ks.IsSupersetOf(Array.Empty<ulong>()).ShouldBeTrue(),                  // empty
            ks => ks.IsSupersetOf(new ulong[] { 1, 2, 3 }).ShouldBeFalse(),              // 3 was removed
            ks => ks.IsSupersetOf(new ulong[] { 1, 2, 5, 7, 8, 9, 10 }).ShouldBeFalse()  // extra element
        );
    }

    [Test]
    public void P1_KeySet_IsProperSupersetOf()
    {
        var patchedDict = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        var keys = patchedDict.Keys;
        keys.Verify(
            ks => ks.IsProperSupersetOf(new ulong[] { 1, 2, 5, 7, 8, 9 }).ShouldBeFalse(),  // equal — not proper
            ks => ks.IsProperSupersetOf(new ulong[] { 1, 5, 9 }).ShouldBeTrue(),            // strict subset
            ks => ks.IsProperSupersetOf(Array.Empty<ulong>()).ShouldBeTrue(),               // empty
            ks => ks.IsProperSupersetOf(new ulong[] { 1, 2, 3 }).ShouldBeFalse()            // 3 not in keys
        );
    }

    [Test]
    public void P1_KeySet_Overlaps()
    {
        var patchedDict = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        var keys = patchedDict.Keys;
        keys.Verify(
            ks => ks.Overlaps(new ulong[] { 1 }).ShouldBeTrue(),                 // single match
            ks => ks.Overlaps(new ulong[] { 5, 99 }).ShouldBeTrue(),             // 5 is in keys
            ks => ks.Overlaps(new ulong[] { 3, 4, 6 }).ShouldBeFalse(),          // all removed or absent
            ks => ks.Overlaps(new ulong[] { 100, 200 }).ShouldBeFalse(),         // completely disjoint
            ks => ks.Overlaps(Array.Empty<ulong>()).ShouldBeFalse()              // empty
        );
    }

    [Test]
    public void P1_KeySet_SetEquals()
    {
        var patchedDict = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        var keys = patchedDict.Keys;
        keys.Verify(
            ks => ks.SetEquals(new ulong[] { 1, 2, 5, 7, 8, 9 }).ShouldBeTrue(),        // exact match
            ks => ks.SetEquals(new ulong[] { 9, 8, 7, 5, 2, 1 }).ShouldBeTrue(),        // different order
            ks => ks.SetEquals(new ulong[] { 1, 2, 5, 7, 8 }).ShouldBeFalse(),          // missing 9
            ks => ks.SetEquals(new ulong[] { 1, 2, 5, 7, 8, 9, 10 }).ShouldBeFalse(),   // extra element
            ks => ks.SetEquals(Array.Empty<ulong>()).ShouldBeFalse()                    // empty
        );
    }

    [Test]
    public void P2_KeySet_SetOperations()
    {
        var p1Dict = new ImmutablePatchedDict<ulong,string>(OriginalDict, PatchDict1, RemovedKeys1);
        var p2Dict = new ImmutablePatchedDict<ulong,string>(p1Dict, PatchDict2, RemovedKeys2);
        var keys   = p2Dict.Keys;
        // keys = {2, 5, 7, 8, 9, 10, 11}
        keys.Verify(
            ks => ks.SetEquals(new ulong[] { 2, 5, 7, 8, 9, 10, 11 }).ShouldBeTrue(),
            ks => ks.IsSubsetOf(new ulong[] { 2, 5, 7, 8, 9, 10, 11, 12 }).ShouldBeTrue(),
            ks => ks.IsProperSubsetOf(new ulong[] { 2, 5, 7, 8, 9, 10, 11 }).ShouldBeFalse(),
            ks => ks.IsSupersetOf(new ulong[] { 5, 10, 11 }).ShouldBeTrue(),
            ks => ks.IsProperSupersetOf(new ulong[] { 5, 10, 11 }).ShouldBeTrue(),
            ks => ks.Overlaps(new ulong[] { 1, 10 }).ShouldBeTrue(),       // 1 was removed but 10 is present
            ks => ks.Overlaps(new ulong[] { 1, 3, 6 }).ShouldBeFalse(),    // all removed from P2
            ks => ks.IsSupersetOf(new ulong[] { 1, 2 }).ShouldBeFalse()    // 1 was removed in P2
        );
    }


    private static KeyValuePair<ulong,string> KV(ulong k, string v) => new KeyValuePair<ulong,string>(k, v);
}
