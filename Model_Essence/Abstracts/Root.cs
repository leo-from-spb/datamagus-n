using Model.Essence.Concept;

namespace Model.Essence.Abstracts;

/// <summary>
/// Project Root.
/// </summary>
public interface Root : MediumMatter
{
    public Family<ConModel> ConceptualModels { get; }
}