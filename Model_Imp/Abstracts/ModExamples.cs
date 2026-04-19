using Model.Concept;
using Util.Collections;
using Util.Collections.Implementation;

namespace Model.Abstracts;



public sealed class ModConDomain : ModNamedTermMatter<ConDomain>, ConDomain
{
    public ModConDomain(uint id, uint version, string? name)
        : base(id, version, name)
    { }


    public ModConDomain(ConDomain origin)
        : base(origin)
    {
        this.ContentType = origin.ContentType;
    }


    public string? ContentType { get; set; }

    public override ImmList<Ref<Matter>> AllRefs => EmptySet<Ref<Matter>>.Instance;
}



public sealed class ModConAttribute : ModNamedTermMatter<ConAttribute>, ConAttribute
{
    public MonoRef<ConDomain> Domain { get; }

    public string? ContentType { get; }


    public ModConAttribute(uint id, uint version, string? name)
        : base(id, version, name)
    {
        this.Domain = new ModMonoRef<ConDomain>();
    }

    public ModConAttribute(ConAttribute origin)
        : base(origin)
    {
        this.Domain      = new ModMonoRef<ConDomain>(origin.Domain);
        this.ContentType = origin.ContentType;
    }


    public override ImmList<Ref<Matter>> AllRefs => Imm.ListOf<Ref<Matter>>(Domain);
}

