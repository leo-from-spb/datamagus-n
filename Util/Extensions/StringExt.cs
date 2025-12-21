using System.Diagnostics.CodeAnalysis;
using Util.Fun;

namespace Util.Extensions;

public static class StringExt
{

    #region STRING (mandatory)

    extension(string str)
    {
        /// <summary>
        /// Assuming that the string is delimited by the <see cref="delimiter"/>, returns the first part (that is before the delimiter).
        /// If there are several delimiters in this string, the first one is considered.
        /// </summary>
        /// <param name="delimiter">the delimiter.</param>
        /// <param name="whenNoMarker">what to return when there is no delimiter.</param>
        /// <param name="trim">also trim the result.</param>
        /// <returns>the part before the delimiter.</returns>
        public string PartBefore(char delimiter, string whenNoMarker = "", bool trim = false)
        {
            int p = str.IndexOf(delimiter);
            return p switch
                   {
                       > 0 => str[..p].ModifyIf(trim, s => s.Trim()),
                       0   => "",
                       < 0 => whenNoMarker
                   };
        }

        /// <summary>
        /// Assuming that the string is delimited by the <see cref="delimiter"/>, returns the part
        /// that is after the delimiter.
        /// If there are several delimiters in this string, the whole string after the first delimiter is returned.
        /// </summary>
        /// <param name="delimiter">the delimiter.</param>
        /// <param name="whenNoMarker">what to return when there is no delimiter.</param>
        /// <param name="trim">also trim the result.</param>
        /// <returns>the part after the (first) delimiter.</returns>
        public string PartAfter(char delimiter, string whenNoMarker = "", bool trim = false)
        {
            int p = str.IndexOf(delimiter);
            return p switch
                   {
                       > 0 => str[(p+1)..].ModifyIf(trim, s => s.Trim()),
                       0   => "",
                       < 0 => whenNoMarker
                   };
        }

        public string Decapitalized
        {
            get
            {
                int n = str.Length;
                if (n == 0) return str;
                char c1 = str[0];
                char cL = char.ToLower(c1);
                if (cL == c1) return str;
                return cL + str[1..];
            }
        }

        public string LastWord
        {
            get
            {
                int n = str.Length;
                if (n == 0) return str;
                for (int i = n - 1; i > 0; i--)
                    if (char.IsUpper(str[i]))
                        return str[i..];
                return str;
            }
        }
    }

    #endregion


    #region STRING (optional)

    extension(string? str)
    {

        /// <summary>
        /// The first character of this string.
        /// </summary>
        /// <param name="empty">the character that is returning when this string is empty or null.</param>
        /// <returns>the first character.</returns>
        public char FirstChar(char empty = '\0') => str is not null && str.Length >= 1 ? str[0] : empty;

        /// <summary>
        /// The last character of this string.
        /// </summary>
        /// <param name="empty">the character that is returning when this string is empty or null.</param>
        /// <returns>the last character.</returns>
        public char LastChar(char empty = '\0')
        {
            int n = str?.Length ?? 0;
            return n > 0 ? str![n - 1] : empty;
        }

        /// <summary>
        /// If this string is not empty, it returns this string, otherwise null.
        /// </summary>
        public string? Nullize() => str is not null && str.Length > 0 ? str : null;

        /// <summary>
        /// Trims and nullizes.
        /// If this string is not blank, it returns this string, otherwise null.
        /// </summary>
        public string? TrimNullize() => str?.Trim().TakeIf(str.Trim().Length > 0);

        public string? With(char prefix = '\0', char suffix = '\0') =>
            str is null ? null :
            prefix != '\0' && suffix != '\0' ? $"{prefix}{str}{suffix}" :
            prefix != '\0' ? prefix + str :
            suffix != '\0' ? str + suffix :
            str;

        public string? With(string prefix = "", string suffix = "") =>
            str is not null ? $"{prefix}{str}{suffix}" : null;

    }

    #endregion


    #region STRING (special)

    extension([NotNullWhen(true)] string? str)
    {
        /// <summary>
        /// Check whether this string is not empty (and not null).
        /// </summary>
        /// <returns>true, when not null and not empty.</returns>
        public bool IsNotEmpty => str is not null && str.Length > 0;

        /// <summary>
        /// Check whether this string is not blank (and not null).
        /// </summary>
        /// <returns>true, when not null and not blank.</returns>
        public bool IsNotBlank => str is not null && !string.IsNullOrWhiteSpace(str);

        /// <summary>
        /// Checks whether this substring is contained in the given text.
        /// </summary>
        public bool IsIn([NotNullWhen(true)] string? text) => str is not null
                                                           && text is not null
                                                           && text.Contains(str);
    }

    extension([NotNullWhen(false)] string? str)
    {
        /// <summary>
        /// Check whether this string is empty or null.
        /// </summary>
        /// <returns>true, when empty or null.</returns>
        public bool IsEmpty => str is null || str.Length == 0;
    }

    #endregion

}