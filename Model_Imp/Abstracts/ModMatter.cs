using System.Collections.Generic;
using System.Linq;
using Util.Collections;

namespace Model.Abstracts;


public abstract class ModMatter<M> : Matter
    where M: class, Matter
{
    public M?   Origin  { get; }
    public uint Id      { get; }
    public uint Version { get; }

    protected ModMatter(uint id, uint version)
    {
        Id      = id;
        Version = version;
    }

    protected ModMatter(M origin)
    {
        Origin  = origin;
        Id      = origin.Id;
        Version = origin.Version + 1;

        // ReSharper disable once VirtualMemberCallInConstructor
        CopyDataFromOrigin(origin);
    }

    protected virtual void CopyDataFromOrigin(M origin)
    { }

    public abstract IEnumerable<Matter>  AllInnerMatters { get; }
    public abstract ImmList<Ref<Matter>> AllRefs         { get; }
}



public abstract class ModMediumMatter<M> : ModMatter<M>, MediumMatter
    where M: class, MediumMatter
{
    protected ModMediumMatter(uint id, uint version)
        : base(id, version)
    { }

    public abstract ImmList<Family<Matter>> Families { get; }
}



public abstract class ModTermMatter<M> : ModMatter<M>, TermMatter
    where M: class, TermMatter
{
    protected ModTermMatter(uint id, uint version)
        : base(id, version)
    { }

    protected ModTermMatter(M origin)
        : base(origin)
    { }

    public override IEnumerable<Matter> AllInnerMatters => Enumerable.Empty<Matter>();
}



public abstract class ModNamedMatter<M> : ModMatter<M>, NamedMatter
    where M: class, NamedMatter
{
    public string? Name { get; }

    protected ModNamedMatter(uint id, uint version, string? name)
        : base(id, version)
    {
        this.Name = name;
    }

}



public abstract class ModNamedMediumMatter<M> : ModMediumMatter<M>, NamedMediumMatter
    where M : class, NamedMediumMatter
{
    public string? Name { get; }

    protected ModNamedMediumMatter(uint id, uint version, string? name)
        : base(id, version)
    {
        Name = name;
    }

}



public abstract class ModNamedTermMatter<M> : ModTermMatter<M>, NamedTermMatter
    where M : class, NamedTermMatter
{
    public string? Name { get; }

    protected ModNamedTermMatter(uint id, uint version, string? name) : base(id, version)
    {
        Name = name;
    }

    protected ModNamedTermMatter(M origin)
        : base(origin)
    {
        Name = origin.Name;
    }
}

