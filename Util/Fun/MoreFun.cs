using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Util.Fun;

public static class MoreFun
{

    /// <summary>
    /// Throws the exception.
    /// </summary>
    /// <exception cref="Exception"></exception>
    [DoesNotReturn]
    public static void TODO(string                     message      = "Function %s is not implemented yet :(",
                            [CallerMemberName] string? functionName = null) =>
        throw new Exception(string.Format(message,functionName));

}
