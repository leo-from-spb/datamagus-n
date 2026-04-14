namespace Util.Extensions;

public static class CharExt
{

    extension(char ch)
    {

        /// <summary>
        /// Determines whether this char exists in the given string
        /// (the given string is treated just as a set of characters)
        /// </summary>
        /// <param name="characters">string with characters.</param>
        public bool IsIn(string characters) =>
            characters.Contains(ch);

    }

}
