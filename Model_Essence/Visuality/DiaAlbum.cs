using Model.Abstracts;

namespace Model.Visuality;

/// <summary>
/// Album with EAR-diagrams.
/// </summary>
public interface DiaAlbum : AbSegment
{
    /// <summary>
    /// Templates.
    /// </summary>
    public NamingFamily<DiaTemplate> Templates { get; }


    /// <summary>
    /// Pages.
    /// </summary>
    public NamingFamily<DiaPage> Pages { get; }

}
