using System.Collections.Generic;

namespace Gena.CSharp;

public static class CsFun
{

    public static string? ToTypoParamStr(this IReadOnlyList<string> typoParams)
    {
        if (typoParams.Count == 0) return null;
        return '<' + string.Join(',', typoParams) + '>';
    }

}
