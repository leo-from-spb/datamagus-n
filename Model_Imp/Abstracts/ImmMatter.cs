using System.Collections.Generic;
using System.Linq;

namespace Model.Abstracts;

public abstract class ImmMatter : Matter
{
    protected ImmMatter(uint id, uint version)
    {
        Id      = id;
        Version = version;
    }

    public uint Id      { get; }
    public uint Version { get; }

    public abstract IEnumerable<Matter> AllInnerMatters { get; }

    public abstract IReadOnlyList<Ref<Matter>> AllRefs { get; }
}


public abstract class ImmMediumMatter : ImmMatter, MediumMatter
{
    protected ImmMediumMatter(uint id, uint version) : base(id, version) { }

    public abstract IReadOnlyList<Family<Matter>> Families { get; }

    public override IEnumerable<Matter> AllInnerMatters =>
        from family in Families
        from matter in family
        select matter;
}


public abstract class ImmTermMatter : ImmMatter, TermMatter
{
    protected ImmTermMatter(uint id, uint version) : base(id, version) { }

    public override IEnumerable<Matter> AllInnerMatters => Enumerable.Empty<Matter>();
}


public abstract class ImmNamedMatter : ImmMatter, NamedMatter
{
    protected ImmNamedMatter(uint id, uint version, string? name)
        : base(id, version)
    {
        Name = name;
    }

    public string? Name { get; }
}


public abstract class ImmNamedMediumMatter : ImmMediumMatter, NamedMediumMatter
{
    protected ImmNamedMediumMatter(uint id, uint version, string? name) : base(id, version)
    {
        Name = name;
    }
    
    public string? Name { get; }
}


public abstract class ImmNamedTermMatter : ImmTermMatter, NamedTermMatter
{
    protected ImmNamedTermMatter(uint id, uint version, string? name) : base(id, version)
    {
        Name = name;
    }
    
    public string? Name { get; }
}
