using System.Linq;
using Model.Abstracts;
using Model.Visuality;

namespace Core.Interaction;

public class Bench
{

     public Root?        Root { get; private set; }

     public DiaAlbum?    Album { get; private set; }

     public DiaPage?     Page     { get; private set; }
     public DiaTemplate? Template { get; private set; }
     public DiaSurface?  Surface  { get; private set; }

     public bool IsPage     => Page != null;
     public bool IsTemplate => Surface is DiaTemplate;


     void SetRoot(Root root)
     {
          if (Root == root) return;

          Root = root;
     }


     void SetAlbum(DiaAlbum album)
     {
          if (Album == album) return;

          Album = album;

          if (Album.Pages.IsNotEmpty)
          {
               SetSurface(Album.Pages.First());
          }
          else
          {
               Surface = null;
               Page = null;
               Template = null;
          }
     }


     void SetSurface(DiaSurface surface)
     {
          if (Surface == surface) return;

          Surface = surface;
          if (surface is DiaPage page)
          {
               Page = page;
               Template = null; // TODO assign the page's template
          }
          else if (Surface is DiaTemplate template)
          {
               Template = template;
               Page = null;
          }
     }

}
