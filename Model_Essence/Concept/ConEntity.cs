using Model.Abstracts;

namespace Model.Concept;


public interface ConEntity : NamedMediumMatter 
{
    
    public NamingFamily<ConAttribute> Attributes { get; }
    
}


public interface ConAttribute : NamedTermMatter
{
    
}
