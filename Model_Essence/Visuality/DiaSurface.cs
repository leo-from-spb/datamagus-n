using Model.Abstracts;

namespace Model.Visuality;


/// <summary>
/// A page or a page template.
/// </summary>
public interface DiaSurface : AbSection
{

    Family<DiaShape> Shapes { get; }

}



/// <summary>
/// Page template.
/// </summary>
public interface DiaTemplate : DiaSurface
{



}



/// <summary>
/// Page.
/// </summary>
public interface DiaPage : DiaSurface
{
    /// <summary>
    /// Number of the page.
    /// Zero means no number.
    /// </summary>
    [MatterProperty]
    ushort PageNr { get; }

    /// <summary>
    /// Template for this page.
    /// </summary>
    MonoRef<DiaTemplate> Template { get; }

    /// <summary>
    /// The area or physical schema that this page represents.
    /// </summary>
    MonoRef<AbSection> RepresentingSection { get; }

}