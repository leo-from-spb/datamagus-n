using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;

namespace Util.Fun;

public static class MoreFun
{

    /// <summary>
    /// Returns an instance of the exception <see cref="NotImplementedException"/>
    /// with information about the calling site (method name).
    /// </summary>
    /// <returns>the exception </returns>
    /// <example>
    /// <code>
    ///     public void MyMethod()
    ///     {
    ///         throw TODO();
    ///     }
    /// </code>
    /// </example>
    public static NotImplementedException TODO(string message = "TODO in the method {0}(...)",
                                               [CallerMemberName] string? methodName = null)
    {
        string m = methodName ?? "an unknown method";
        string text = string.Format(message, m);
        return new NotImplementedException(text);
    }


    public static void RequireParam(string paramName, bool condition, [CallerArgumentExpression("condition")] string? expression = null)
    {
        if (!condition)
            throw new ArgumentException($"Requirement failed: {expression}", paramName);
    }

    public static void Require(bool condition, [CallerArgumentExpression("condition")] string? expression = null)
    {
        if (!condition)
            throw new ArgumentException($"Requirement failed: {expression}");
    }


}
