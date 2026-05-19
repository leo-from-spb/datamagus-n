namespace Gena.CSharp;

/// <summary>
/// Visibility of a C# entity.
/// <p>
/// Visibility <c>visDefault</c> means no visibility keyword in the entity declaration.
/// Visibility <c>visAuto</c> means that a meta class can select visibility auto-magically
/// (depending on the entity type).
/// </p>
/// </summary>
public enum CsVisibility : byte
{
    visAuto      = 0,
    visDefault   = 1,
    visPrivate   = 2,
    visProtected = 3,
    visInternal  = 4,
    visPublic    = 5
}



public static class CsVisibilityExtensions
{
    extension(CsVisibility visibility)
    {
        /// <summary>
        /// Keyword to specify in the entity declaration.
        /// </summary>
        public string? Word =>
            visibility switch
            {
                CsVisibility.visPrivate   => "private",
                CsVisibility.visProtected => "protected",
                CsVisibility.visInternal  => "internal",
                CsVisibility.visPublic    => "public",
                _                         => null
            };

        /// <summary>
        /// Whether it is Auto.
        /// </summary>
        public bool IsAuto => visibility == CsVisibility.visAuto;
    }
}
