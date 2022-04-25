namespace FileCabinetApp
{
    /// <summary>
    /// Validate block by custom ruleset.
    /// </summary>
    internal class CustomBlockValidator
    {
        /// <summary>
        /// Validate block.
        /// </summary>
        /// <param name="block">Char, characterize persons living block.</param>
        /// <exception cref="ArgumentException">Block is not capital letter in english alphabet.</exception>
        public static void ValidateParameters(char block)
        {
            int firstAlphabetLetter = 65, lastAlphabetLetter = 90;

            if (block < firstAlphabetLetter || block > lastAlphabetLetter)
            {
                throw new ArgumentException("wrong block letter", nameof(block));
            }
        }
    }
}
