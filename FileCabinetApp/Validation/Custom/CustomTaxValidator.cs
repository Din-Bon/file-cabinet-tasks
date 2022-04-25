namespace FileCabinetApp
{
    /// <summary>
    /// Validate tax by custom ruleset.
    /// </summary>
    internal class CustomTaxValidator
    {
        /// <summary>
        /// Validate tax.
        /// </summary>
        /// <param name="tax">Decimal, characterize tax for person.</param>
        /// <exception cref="ArgumentException">Tax out of range.</exception>
        public static void ValidateParameters(decimal tax)
        {
            if (tax < 10 || tax > 100)
            {
                throw new ArgumentException("wrong tax", nameof(tax));
            }
        }
    }
}
