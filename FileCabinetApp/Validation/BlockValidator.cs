namespace FileCabinetApp
{
    /// <summary>
    /// Validate block.
    /// </summary>
    internal class BlockValidator
    {
        private int firstLetter;
        private int lastLetter;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockValidator"/> class.
        /// </summary>
        /// <param name="first">First alphabet letter.</param>
        /// <param name="last">Last alphabet letter.</param>
        public BlockValidator(int first, int last)
        {
            this.firstLetter = first;
            this.lastLetter = last;
        }

        /// <summary>
        /// Validate block.
        /// </summary>
        /// <param name="block">Char, characterize persons living block.</param>
        /// <exception cref="ArgumentException">Block is not capital letter in english alphabet.</exception>
        public void ValidateParameters(char block)
        {
            if (block < this.firstLetter || block > this.lastLetter)
            {
                throw new ArgumentException("wrong block letter", nameof(block));
            }
        }
    }
}
