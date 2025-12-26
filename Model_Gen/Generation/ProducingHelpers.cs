namespace Model.Generation;

internal static class ProducingHelpers
{
    extension(string str)
    {
        /// <summary>
        /// Wraps the string with an HTML tag.
        /// </summary>
        public string InTag(string tag) =>
            $"<{tag}>{str}</{tag}>";

        /// <summary>
        /// Wraps the string with an HTML tag with attributes.
        /// </summary>
        public string InTag(string tag, string attributes) =>
            $"<{tag} {attributes}>{str}</{tag}>";
    }
}
