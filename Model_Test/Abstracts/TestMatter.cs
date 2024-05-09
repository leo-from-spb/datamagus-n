namespace Model.Abstracts;


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

    public ImmTestYard(uint id, uint version, /*Imm*/NamingFamily<TestRabbit> rabbits, /*Imm*/NamingFamily<TestGuineaPig> guineaPigs)
        : base(id, version)
    {
        Rabbits = rabbits;
        GuineaPigs = guineaPigs;
    }

}


public class ImmTestRabbit : ImmNamedMatter, TestRabbit
{
    public ImmTestRabbit(uint id, uint version, string? name) : base(id, version, name)
    {
    }
}


public class ImmTestGuineaPig : ImmNamedMatter, TestGuineaPig
{
    public ImmTestGuineaPig(uint id, uint version, string? name) : base(id, version, name)
    {
    }
}
