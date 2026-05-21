using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Util.Text;

public static class RegexFun
{

    public static Regex RegEx([RegexPattern] string pattern) =>
        new Regex(pattern.Replace("  ", @"\s+").Replace(" ", @"\s*"), RegexOptions.IgnorePatternWhitespace);


}
