using Model.Essence.Abstracts;

namespace Model.Essence.Visuality;


/// <summary>
/// A page or a page template.
/// </summary>
public interface DiaSurface : AbSection
{



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


}