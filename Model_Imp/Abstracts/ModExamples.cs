using System.Collections.Generic;
using System.Linq;
using Model.Concept;
using Util.Collections;
using Util.Collections.Implementation;

namespace Model.Abstracts;



public sealed class ModConDomain : ModNamedTermMatter<ConDomain>, ConDomain
{
    public ModConDomain(ModBaseObject parent, string? name)
        : base(parent, name)
    { }


    public ModConDomain(ModBaseObject parent, ConDomain origin)
        : base(parent, origin)
    {
        this.ContentType = origin.ContentType;
    }


    public string? ContentType { get; set; }

    public override ImmList<Ref<Matter>> AllRefs => EmptySet<Ref<Matter>>.Instance;
}



public sealed class ModConAttribute : ModNamedTermMatter<ConAttribute>, ConAttribute
{
    public ModMonoRef<ConDomain> Domain { get; }
    MonoRef<ConDomain> ConAttribute.Domain => Domain;

    public string? ContentType { get; }


    public ModConAttribute(ModBaseObject parent)
        : base(parent)
    {
        this.Domain = new ModMonoRef<ConDomain>();
    }

    public ModConAttribute(ModBaseObject parent, ConAttribute origin)
        : base(parent, origin)
    {
        this.Domain      = new ModMonoRef<ConDomain>(origin.Domain);
        this.ContentType = origin.ContentType;
    }


    public override ImmList<Ref<Matter>> AllRefs => Imm.ListOf<Ref<Matter>>(Domain);
}



public sealed class ModConEntity : ModNamedMediumMatter<ConEntity>, ConEntity
{
    public ModConEntity(ModBaseObject parent, string? name) : base(parent, name)
    {
        Attributes = new ModNamingFamily<ConAttribute,ModConAttribute>(this, () => new ModConAttribute(this));
    }

    public ModConEntity(ModBaseObject parent, ConEntity origin) : base(parent, origin.Name)
    {
        Attributes = new ModNamingFamily<ConAttribute,ModConAttribute>(this, origin.Attributes, () => new ModConAttribute(this));
    }


    public ModNamingFamily<ConAttribute,ModConAttribute> Attributes { get; }
    NamingFamily<ConAttribute> ConEntity.Attributes => Attributes;


    public override ImmList<Family<Matter>> Families => Imm.ListOf<Family<Matter>>(Attributes);

    public override IEnumerable<Matter> AllInnerMatters => Enumerable.Union(Attributes, Attributes);

    public override ImmList<Ref<Matter>> AllRefs => EmptySet<Ref<Matter>>.Instance;
}