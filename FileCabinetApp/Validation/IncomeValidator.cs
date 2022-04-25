namespace FileCabinetApp
{
    /// <summary>
    /// Validate income.
    /// </summary>
    internal class IncomeValidator
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
        /// <param name="income">Short value, characterize persons income.</param>
        /// <exception cref="ArgumentException">Value less than zero.</exception>
        public void ValidateParameters(short income)
        {
            if (income < this.minInc)
            {
                throw new ArgumentException("wrong income", nameof(income));
            }
        }
    }
}
