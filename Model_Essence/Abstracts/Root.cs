using Model.Essence.Concept;

namespace Model.Essence.Abstracts;

/// <summary>
/// Project Root.
/// </summary>
public interface Root : MediumMatter
{
    public NamingFamily<ConModel> ConceptualModels { get; }
}