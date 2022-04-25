namespace FileCabinetApp
{
    /// <summary>
    /// Validate income by custom ruleset.
    /// </summary>
    internal class CustomIncomeValidator
    {
        /// <summary>
        /// Validate income.
        /// </summary>
        /// <param name="income">Short value, characterize persons income.</param>
        /// <exception cref="ArgumentException">Value less than zero.</exception>
        public static void ValidateParameters(short income)
        {
            if (income < 0)
            {
                throw new ArgumentException("wrong income", nameof(income));
            }
        }
    }
}
