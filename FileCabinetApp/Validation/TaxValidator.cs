﻿namespace FileCabinetApp
{
    /// <summary>
    /// Validate tax.
    /// </summary>
    internal class TaxValidator : IRecordValidator
    {
        private decimal min;
        private decimal max;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxValidator"/> class.
        /// </summary>
        /// <param name="min">Minimum tax.</param>
        /// <param name="max">Maximum tax.</param>
        public TaxValidator(decimal min, decimal max)
        {
            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// Validate tax.
        /// </summary>
        /// <param name="person">Personal data.</param>
        /// <param name="income">Person's income.</param>
        /// <param name="tax">Person's tax.</param>
        /// <param name="block">Person's living block.</param>
        /// <exception cref="ArgumentException">Tax out of range.</exception>
        public void ValidateParameters(Person person, short income, decimal tax, char block)
        {
            if (tax < this.min || tax > this.max)
            {
                throw new ArgumentException("wrong tax", nameof(tax));
            }
        }
    }
}
