using Model.Essence.Abstracts;

namespace Model.Essence.Concept;


public interface ConEntity : NamedMediumMatter 
{
    
    public NamingFamily<ConAttribute> Attributes { get; }
    
}


public interface ConAttribute : NamedTermMatter
{
    
}
