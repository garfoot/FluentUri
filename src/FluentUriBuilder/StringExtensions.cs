namespace Garfoot.FluentUriBuilder
{
    public static class StringExtensions
    {
        /// <summary>
        ///     Calls <see cref="System.String.IsNullOrEmpty"/> as an extension method.
        /// </summary>
        /// <param name="text">The string to test.</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        /// <summary>
        ///     Calls <see cref="string.IsNullOrWhiteSpace"/> as an extension method.
        /// </summary>
        /// <param name="text">The string to test.</param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }
    }
}