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


}