namespace FileCabinetApp
{
    /// <summary>
    /// Validate income.
    /// </summary>
    internal class IncomeValidator : IRecordValidator
    {
        private short minInc;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeValidator"/> class.
        /// </summary>
        /// <param name="min">Minimum income.</param>
        public IncomeValidator(short min)
        {
            this.minInc = min;
        }

        /// <summary>
        /// Validate income.
        /// </summary>
        /// <param name="person">Personal data.</param>
        /// <param name="income">Person's income.</param>
        /// <param name="tax">Person's tax.</param>
        /// <param name="block">Person's living block.</param>
        /// <exception cref="ArgumentException">Value less than zero.</exception>
        public void ValidateParameters(Person person, short income, decimal tax, char block)
        {
            if (income < this.minInc)
            {
                throw new ArgumentException("wrong income", nameof(income));
            }
        }
    }
}
