using Model.Essence.Concept;
using Model.Essence.Visuality;

namespace Model.Essence.Abstracts;

/// <summary>
/// Project Root.
/// </summary>
public interface Root : MediumMatter
{
    public NamingFamily<ConModel> ConceptualModels { get; }

    public NamingFamily<DiaAlbum> Albums { get; }
    
}