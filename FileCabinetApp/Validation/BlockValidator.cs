namespace FileCabinetApp
{
    /// <summary>
    /// Validate block.
    /// </summary>
    internal class BlockValidator : IRecordValidator
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
        /// <param name="person">Personal data.</param>
        /// <param name="income">Person's income.</param>
        /// <param name="tax">Person's tax.</param>
        /// <param name="block">Person's living block.</param>
        /// <exception cref="ArgumentException">Block is not capital letter in english alphabet.</exception>
        public void ValidateParameters(Person person, short income, decimal tax, char block)
        {
            if (block < this.firstLetter || block > this.lastLetter)
            {
                throw new ArgumentException("wrong block letter", nameof(block));
            }
        }
    }
}
