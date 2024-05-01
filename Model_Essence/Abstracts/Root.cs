using Model.Concept;
using Model.Visuality;

namespace Model.Abstracts;

/// <summary>
/// Project Root.
/// </summary>
public interface Root : MediumMatter
{
    public NamingFamily<ConModel> ConceptualModels { get; }

    public NamingFamily<DiaAlbum> Albums { get; }
    
}