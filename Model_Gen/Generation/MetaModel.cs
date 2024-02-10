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
    internal readonly byte        OrderNum;  
    internal readonly string      IntfName;
    internal readonly Type        Intf;
    internal readonly bool        IsConcrete;
    internal          bool        IsAbstract => !IsConcrete;
    internal readonly string      Prefix;
    internal readonly string      Name;

    internal readonly HashSet<Type>       AllBaseIntfs        = [];
    internal readonly HashSet<Type>       OwnBaseIntfs        = [];
    internal readonly List<MetaMatter>    DeclaredBaseMatters = [];
    internal readonly List<MetaMatter>    BaseMatters         = [];
    internal readonly HashSet<MetaFamily> Families            = [];
    
    internal readonly MetaImm Imm;

    internal bool        HasName;
    internal bool        IsMedium;
    internal MetaMatter? BaseMatter;


    private static byte matterOrderCounter = 0;
    
    
    internal class MetaImm (MetaMatter m)
    {
        internal string ClassName;
        internal string BaseClassName;
        internal bool   ManuallyImplemented;
    }
    
    internal MetaMatter(Type intf, bool isConcrete, byte level)
    {
        OrderNum   = ++matterOrderCounter;
        Level      = level;
        IntfName   = intf.Name;
        Intf       = intf;
        IsConcrete = isConcrete;

        Prefix =
            IntfName.StartsWith("Ab")  ? "Ab" :
            IntfName.StartsWith("Con") ? "Con" :
            IntfName.StartsWith("Db")  ? "Db" :
            IntfName.StartsWith("Dia") ? "Dia" :
            "";
        SegmKind =
            Prefix switch
            {
                "Ab"  => SegmentKind.soCommon,
                "Con" => SegmentKind.soConcept,
                "Db"  => SegmentKind.soDB,
                "Dia" => SegmentKind.soDiagram,
                _     => SegmentKind.soCommon
            };
        Name = IntfName.Remove(0, Prefix.Length);

        Imm = new MetaImm(this);
    }

    internal void AddFamily(MetaFamily family) => Families.Add(family);

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
    internal readonly bool       Owned;

    internal bool isAbstract => Child.IsAbstract;

    internal MetaFamily(MetaMatter parent, MetaMatter child)
        : this(parent, child, true) { }

    private MetaFamily(MetaMatter parent, MetaMatter child, bool owned)
    {
        Debug.Assert(parent != child);
        Parent = parent;
        Child  = child;
        Owned  = owned;
    }

    internal MetaFamily cloneFor(MetaMatter inheritor) =>
        new MetaFamily(inheritor, Child, false);
}