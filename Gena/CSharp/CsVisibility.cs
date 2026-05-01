namespace Gena.CSharp;

/// <summary>
/// Visibility of a C# entity.
/// </summary>
public enum CsVisibility : byte
{
    visAuto      = 0,
    visPrivate   = 1,
    visProtected = 2,
    visInternal  = 3,
    visPublic    = 4
}



public static class CsVisibilityExtensions
{
    extension(CsVisibility visibility)
    {
        public string? Word =>
            visibility switch
            {
                CsVisibility.visPrivate   => "private",
                CsVisibility.visProtected => "protected",
                CsVisibility.visInternal  => "internal",
                CsVisibility.visPublic    => "public",
                _                         => null
            };
    }
}
