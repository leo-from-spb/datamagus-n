using System.Collections.Generic;
using System.Linq;
using Util.Collections;

namespace Model.Abstracts;


public abstract class ModMatter<M> : ModBaseObject, Matter
    where M: class, Matter
{
    public M?   Origin  { get; }
    public uint Id      { get; }
    public uint Version { get; }

    protected ModMatter(ModelMaster master)
        : base(master)
    {
        Origin  = null;
        Id      = Master.NewId();
        Version = Master.Version;
    }

    protected ModMatter(ModBaseObject parent)
        : base(parent)
    {
        Origin  = null;
        Id      = Master.NewId();
        Version = Master.Version;
    }

    protected ModMatter(ModBaseObject parent, M origin)
        : base(parent)
    {
        Origin  = origin;
        Id      = origin.Id;
        Version = Master.Version;

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
    protected ModMediumMatter(ModelMaster master)
        : base(master)
    { }

    protected ModMediumMatter(ModBaseObject parent)
        : base(parent)
    { }

    protected ModMediumMatter(ModBaseObject parent, M origin)
        : base(parent, origin)
    { }

    public abstract ImmList<Family<Matter>> Families { get; }
}



public abstract class ModTermMatter<M> : ModMatter<M>, TermMatter
    where M: class, TermMatter
{
    protected ModTermMatter(ModBaseObject parent)
        : base(parent)
    { }

    protected ModTermMatter(ModBaseObject parent, M origin)
        : base(parent, origin)
    { }

    public override IEnumerable<Matter> AllInnerMatters => Enumerable.Empty<Matter>();
}



public abstract class ModNamedMatter<M> : ModMatter<M>, NamedMatter
    where M: class, NamedMatter
{
    public string? Name { get; set; }

    protected ModNamedMatter(ModBaseObject parent)
        : base(parent)
    { }

    protected ModNamedMatter(ModBaseObject parent, string? name)
        : base(parent)
    {
        this.Name = name;
    }

    protected ModNamedMatter(ModBaseObject parent, M origin) : base(parent, origin)
    {
        this.Name = origin.Name;
    }
}



public abstract class ModNamedMediumMatter<M> : ModMediumMatter<M>, NamedMediumMatter
    where M : class, NamedMediumMatter
{
    public string? Name { get; }

    protected ModNamedMediumMatter(ModBaseObject parent, string? name)
        : base(parent)
    {
        Name = name;
    }

}



public abstract class ModNamedTermMatter<M> : ModNamedMatter<M>, NamedTermMatter
    where M : class, NamedTermMatter
{
    protected ModNamedTermMatter(ModBaseObject parent) : base(parent)
    { }

    protected ModNamedTermMatter(ModBaseObject parent, string? name) : base(parent, name)
    { }

    protected ModNamedTermMatter(ModBaseObject parent, M origin)
        : base(parent, origin)
    { }

    public override IEnumerable<Matter> AllInnerMatters => Enumerable.Empty<Matter>();
}

