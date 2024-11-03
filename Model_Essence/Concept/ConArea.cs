using Model.Abstracts;

namespace Model.Concept;

/// <summary>
/// Conceptual Area — can be either a ConModel or ConSubjArea.
/// </summary>
public interface ConArea : NamedMediumMatter
{

    public NamingFamily<ConDomain> Domains { get; }
    public NamingFamily<ConEntity> Entities { get; }

}


/// <summary>
/// Conceptual Model.
/// </summary>
public interface ConModel : ConArea, AbSegment
{
    
    public NamingFamily<ConSubjArea> SubjectAreas { get; }
    
}


/// <summary>
/// SubjectArea.
/// </summary>
public interface ConSubjArea : ConArea, AbSection
{
    [MatterProperty]
    public string Prefix { get; }


}

