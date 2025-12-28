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


    private static KeyValuePair<ulong,string> KV(ulong k, string v) => new KeyValuePair<ulong,string>(k, v);
}
