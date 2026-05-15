using System;
using System.Text;

namespace Util.Extensions;

/// <summary>
/// StringBuilder extensions.
/// </summary>
public static class StringBuilderExt
{
    extension(StringBuilder sb)
    {
        /// <summary>
        /// Append the given string, padding it to the specified width.
        /// </summary>
        /// <param name="str">the string to append.</param>
        /// <param name="width">desired width.</param>
        /// <param name="clip">when true, longer string is clipped.</param>
        /// <param name="alignRight">when true, the string is aligned to right.</param>
        public StringBuilder AppendPad(string str,
                                       int    width,
                                       bool   clip       = false,
                                       bool   alignRight = false)
        {
            int n = str.Length;
            int pads = n < width ? width - n : 0;

            // append
            sb.EnsureCapacity(sb.Length + Math.Max(n, width));
            if (pads > 0 && alignRight)
                sb.Append(' ', pads);
            if (n <= width || !clip)
                sb.Append(str);
            else
                sb.Append(str.AsSpan(0, width));
            if (pads > 0 && !alignRight)
                sb.Append(' ', pads);

            return sb;
        }


        /// <summary>
        /// Append the given value, padding it to the specified width.
        /// </summary>
        /// <param name="str">the value to append.</param>
        /// <param name="width">desired width.</param>
        /// <param name="clip">when true, longer string is clipped.</param>
        /// <param name="alignRight">when true, the string is aligned to right.</param>
        public StringBuilder AppendPad(int  value,
                                       int  width,
                                       bool clip       = false,
                                       bool alignRight = false)
            => sb.AppendPad(value.ToString(), width, clip, alignRight);


        /// <summary>
        /// Ensures that the last character is the "end-of-line" (when the buffer is not empty).
        /// Adds the "end-of-line" character at the end if no.
        /// </summary>
        public StringBuilder EnsureEoln()
        {
            int n = sb.Length;
            if (n == 0) return sb;
            char lastChar = sb[n-1];
            if (lastChar != '\n') sb.AppendLine();
            return sb;
        }
    }
}
