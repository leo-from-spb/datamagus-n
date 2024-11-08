using System;
using System.Collections.Generic;
using System.Diagnostics;
using Util.Extensions;
using Util.Fun;

namespace Model.Generation;

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
    soVisuality
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
    internal          string      Names    => Name.Plural();
    internal          string      LowName  => Name.Decap();
    internal          string      LowNames => LowName.Plural();

    internal readonly HashSet<Type>                    AllBaseIntfs        = [];
    internal readonly HashSet<Type>                    OwnBaseIntfs        = [];
    internal readonly List<MetaMatter>                 DeclaredBaseMatters = [];
    internal readonly List<MetaMatter>                 BaseMatters         = [];
    internal readonly HashSet<MetaFamily>              OwnFamilies         = [];
    internal readonly Dictionary<string, MetaFamily>   AllFamilies         = [];
    internal readonly List<MetaRef>                    OwnRefs             = [];
    internal readonly Dictionary<string, MetaRef>      AllRefs             = [];
    internal readonly HashSet<MetaProperty>            OwnProperties = [];
    internal readonly Dictionary<string, MetaProperty> AllProperties = [];
    
    internal readonly MetaImm Imm;

    internal bool        HasName;
    internal bool        IsMedium;
    internal MetaMatter? BaseMatter;

    private static byte matterOrderCounter = 0;


    internal class MetaImm (MetaMatter matter)
    {
        internal readonly MetaMatter Matter        = matter;
        internal          string     ClassName     = "?";
        internal          string     BaseClassName = "?";
        internal          bool       ManuallyImplemented;
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
                "Dia" => SegmentKind.soVisuality,
                _     => SegmentKind.soCommon
            };
        Name = IntfName.Remove(0, Prefix.Length);

        Imm = new MetaImm(this);
    }

    internal void AddFamily(MetaFamily family) => OwnFamilies.Add(family);

    public override string ToString()
    {
        char bullet = IsConcrete ? '◆' : '◇';
        return $"{bullet} [{(byte)SegmKind}.{Level}] {Intf.Name}";
    }
}


internal class MetaFamily
{
    internal readonly MetaMatter Matter;
    internal readonly MetaMatter Child;
    internal readonly Type       FamilyType;
    internal readonly string     FamilyTypeName;
    internal readonly string     FamilyName;
    internal readonly bool       Owned;
    
    internal bool   IsAbstract    => Child.IsAbstract;
    internal string FamilyVarName => FamilyName.Decap();
    
    internal MetaFamily(MetaMatter matter, MetaMatter child, Type familyType, string familyName)
        : this(matter, child, familyType, familyName, true) { }

    private MetaFamily(MetaMatter matter, MetaMatter child, Type familyType, string familyName, bool owned)
    {
        Debug.Assert(matter != child);
        Matter     = matter;
        Child      = child;
        FamilyType = familyType;
        FamilyName = familyName;
        Owned      = owned;

        string familyTypeTechnicalName = familyType.Name;
        FamilyTypeName = familyTypeTechnicalName.PartBefore('`', familyTypeTechnicalName);
    }

    internal MetaFamily cloneFor(MetaMatter inheritor) =>
        new MetaFamily(inheritor, Child, FamilyType, FamilyName, false);
}


internal class MetaRef
{
    internal readonly MetaMatter Matter;
    internal readonly string     RefName;
    internal          string     RefLowName => RefName.Decap();
    internal readonly bool       Poly;
    internal readonly MetaMatter TargetMatter;

    public MetaRef(MetaMatter matter, string refName, bool poly, MetaMatter targetMatter)
    {
        Matter       = matter;
        RefName      = refName;
        Poly         = poly;
        TargetMatter = targetMatter;
    }

    internal string RefWord       => Poly ? "PolyRef" : "MonoRef";
    internal string InnerTypeSpec => Poly ? "IReadOnlyList<uint>" : "uint";
}


internal class MetaProperty
{
    internal readonly MetaMatter Matter;
    internal readonly string     ProName;
    internal          string     ProVarName => ProName.Decap();
    internal readonly Type       ProType;
    internal readonly string     ProTypeName;

    internal bool ImplementedInMatter = false;

    public MetaProperty(MetaMatter matter, string proName, Type proType)
    {
        Matter      = matter;
        ProName     = proName;
        ProType     = proType;
        ProTypeName = MetaConsts.SystemTypes.Get(proType, proType.Name);
    }

    internal MetaProperty cloneFor(MetaMatter inheritor)
    {
        var newProperty = new MetaProperty(inheritor, ProName, ProType);
        newProperty.ImplementedInMatter = this.ImplementedInMatter;
        return newProperty;
    }
}