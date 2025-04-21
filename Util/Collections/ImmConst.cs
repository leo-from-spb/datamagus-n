namespace Util.Collections;

/// <summary>
/// Common collection constants.
/// </summary>
public static class ImmConst
{
    /// <summary>
    /// The number that is returned by default by methods <code>IndexOf</code> when the given element is not found.
    /// This number is negative, so it should not be checked on this exact value but on negativity instead.
    /// </summary>
    public const int notFoundIndex = int.MinValue;
}
