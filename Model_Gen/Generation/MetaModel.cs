using System.Diagnostics;

namespace Model.Gen.Generation;

internal class MetaModel
{
    internal readonly List<MetaMatter>             Matters = [];
    internal readonly Dictionary<Type, MetaMatter> Intfs   = [];


    internal void Add(MetaMatter matter)
    {
        Matters.Add(matter);
        Intfs.Add(matter.Intf, matter);
    }
}

internal enum SegmentKind : byte
{
    soCommon,
    soConcept,
    soDB,
    soDiagram
}

internal class MetaMatter
{
    internal readonly SegmentKind SegmKind;
    internal readonly byte        Level;
    internal readonly string      IntfName;

    internal readonly Type Intf;

    internal bool IsConcrete;
    internal bool IsAbstract => !IsConcrete;

    internal List<MetaFamily> Families = new();

    internal MetaMatter(Type intf, bool isConcrete, byte level)
    {
        Level      = level;
        IntfName   = intf.Name;
        Intf       = intf;
        IsConcrete = isConcrete;
        
        SegmKind =
            IntfName.StartsWith("Ab") ? SegmentKind.soCommon :
            IntfName.StartsWith("Con") ? SegmentKind.soConcept :
            IntfName.StartsWith("Db") ? SegmentKind.soDB :
            IntfName.StartsWith("Dia") ? SegmentKind.soDiagram :
            SegmentKind.soCommon;
    }

    internal void AddFamily(MetaFamily family)
    {
        Families.Add(family);
    }


    public override string ToString()
    {
        char bullet = IsConcrete ? '◆' : '◇';
        return $"{bullet} [{(byte)SegmKind}.{Level}] {Intf.Name}";
    }
}

internal class MetaFamily
{
    internal readonly MetaMatter Parent;
    internal readonly MetaMatter Child;

    internal bool isAbstract => Child.IsAbstract;

    public MetaFamily(MetaMatter parent, MetaMatter child)
    {
        Debug.Assert(parent != child);
        Parent = parent;
        Child  = child;
    }
}