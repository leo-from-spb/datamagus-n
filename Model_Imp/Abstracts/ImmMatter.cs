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
}


public abstract class ImmMediumMatter : ImmMatter, MediumMatter
{
    protected ImmMediumMatter(uint id, uint version) : base(id, version) { }
    
}


public abstract class ImmTermMatter : ImmMatter, TermMatter
{
    protected ImmTermMatter(uint id, uint version) : base(id, version) { }
    
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
