using Model.Essence.Abstracts;
using Model.Imp.Abstracts;

namespace Model.Test.Abstracts;


public interface TestYard : MediumMatter
{
    public NamingFamily<TestRabbit> Rabbits { get; }
    public NamingFamily<TestGuineaPig> GuineaPigs { get; }
}


public interface TestRabbit : NamedTermMatter
{
    
}


public interface TestGuineaPig : NamedTermMatter
{
    
}



public class ImmTestYard : ImmMatter, TestYard
{
    public /*Imm*/NamingFamily<TestRabbit> Rabbits { get; }
    public /*Imm*/NamingFamily<TestGuineaPig> GuineaPigs { get; }

    public ImmTestYard(uint id, /*Imm*/NamingFamily<TestRabbit> rabbits, /*Imm*/NamingFamily<TestGuineaPig> guineaPigs) : base(id)
    {
        Rabbits = rabbits;
        GuineaPigs = guineaPigs;
    }

}


public class ImmTestRabbit : ImmNamedMatter, TestRabbit
{
    public ImmTestRabbit(uint id, string? name) : base(id, name)
    {
    }
}


public class ImmTestGuineaPig : ImmNamedMatter, TestGuineaPig
{
    public ImmTestGuineaPig(uint id, string? name) : base(id, name)
    {
    }
}
