using Model.Essence.Abstracts;

namespace Model.Essence.Concept;

/// <summary>
/// Conceptual Area — can be either a ConModel or ConSubjArea.
/// </summary>
public interface ConArea : AbSegment
{
    
}


/// <summary>
/// Conceptual Model.
/// </summary>
public interface ConModel : ConArea
{
    
    public NamingFamily<ConSubjArea> SubjectAreas { get; }
    
}


/// <summary>
/// SubjectArea.
/// </summary>
public interface ConSubjArea : ConArea
{
    
    
    
}

