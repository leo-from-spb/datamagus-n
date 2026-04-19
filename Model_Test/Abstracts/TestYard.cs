using System.Collections.Generic;
using System.Linq;
using Util.Collections;

namespace Model.Abstracts;


public interface TestYard : MediumMatter
{
    public NamingFamily<TestRabbit>    Rabbits    { get; }
    public NamingFamily<TestGuineaPig> GuineaPigs { get; }
}


public interface TestRabbit : NamedTermMatter
{
    
}


public interface TestGuineaPig : NamedTermMatter
{
    
}



public class ImmTestYard : ImmMediumMatter, TestYard
{
    public /*Imm*/NamingFamily<TestRabbit> Rabbits { get; }
    public /*Imm*/NamingFamily<TestGuineaPig> GuineaPigs { get; }

    public ImmTestYard(uint id, uint version, /*Imm*/NamingFamily<TestRabbit> rabbits, /*Imm*/NamingFamily<TestGuineaPig> guineaPigs)
        : base(id, version)
    {
        Rabbits = rabbits;
        GuineaPigs = guineaPigs;
    }

    public override ImmList<Family<Matter>> Families => Imm.ListOf<Family<Matter>>(Rabbits, GuineaPigs);
    public override ImmList<Ref<Matter>>    AllRefs  => AbstractConsts.NoRefs;
}


public class ImmTestRabbit : ImmNamedTermMatter, TestRabbit
{
    public ImmTestRabbit(uint id, uint version, string? name) : base(id, version, name)
    {
    }

    public override ImmList<Ref<Matter>> AllRefs => AbstractConsts.NoRefs;
}


public class ImmTestGuineaPig : ImmNamedTermMatter, TestGuineaPig
{
    public ImmTestGuineaPig(uint id, uint version, string? name) : base(id, version, name)
    {
    }

    public override ImmList<Ref<Matter>> AllRefs => AbstractConsts.NoRefs;
}



public class ModTestYard : ModMediumMatter<TestYard>, TestYard
{
    public ModTestYard(ModelMaster master) : base(master)
    {
        Rabbits    = new ModNamingFamily<TestRabbit,ModTestRabbit>(this, () => new ModTestRabbit(this));
        GuineaPigs = new ModNamingFamily<TestGuineaPig,ModTestGuineaPig>(this, () => new ModTestGuineaPig(this));
    }

    public ModTestYard(ModBaseObject parent) : base(parent)
    {
        Rabbits    = new ModNamingFamily<TestRabbit,ModTestRabbit>(this, () => new ModTestRabbit(this));
        GuineaPigs = new ModNamingFamily<TestGuineaPig,ModTestGuineaPig>(this, () => new ModTestGuineaPig(this));
    }

    public ModTestYard(ModBaseObject parent, TestYard origin) : base(parent, origin)
    {
        Rabbits    = new ModNamingFamily<TestRabbit,ModTestRabbit>(this, origin.Rabbits, () => new ModTestRabbit(this));
        GuineaPigs = new ModNamingFamily<TestGuineaPig,ModTestGuineaPig>(this, origin.GuineaPigs, () => new ModTestGuineaPig(this));
    }

    public ModNamingFamily<TestRabbit,ModTestRabbit> Rabbits { get; }
    NamingFamily<TestRabbit> TestYard. Rabbits => Rabbits;

    public ModNamingFamily<TestGuineaPig,ModTestGuineaPig>   GuineaPigs { get; }
    NamingFamily<TestGuineaPig> TestYard.GuineaPigs => GuineaPigs;

    

    public override ImmList<Family<Matter>> Families => Imm.ListOf<Family<Matter>>(Rabbits, GuineaPigs);

    public override IEnumerable<Matter> AllInnerMatters => Enumerable.Union<Matter>(Rabbits, GuineaPigs);

    public override ImmList<Ref<Matter>> AllRefs => AbstractConsts.NoRefs;
}


public class ModTestRabbit : ModNamedTermMatter<TestRabbit>, TestRabbit
{
    public ModTestRabbit(ModBaseObject parent) : base(parent) { }

    public ModTestRabbit(ModBaseObject parent, TestRabbit origin) : base(parent, origin) { }

    public override ImmList<Ref<Matter>> AllRefs => AbstractConsts.NoRefs;
}



public class ModTestGuineaPig : ModNamedTermMatter<TestGuineaPig>, TestGuineaPig
{
    public ModTestGuineaPig(ModBaseObject parent) : base(parent) { }

    public ModTestGuineaPig(ModBaseObject parent, TestGuineaPig origin) : base(parent, origin) { }

    public override ImmList<Ref<Matter>> AllRefs => AbstractConsts.NoRefs;
}