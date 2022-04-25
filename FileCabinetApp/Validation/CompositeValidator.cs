namespace FileCabinetApp
{
    /// <summary>
    /// Container for validators.
    /// </summary>
    public class CompositeValidator : IRecordValidator
    {
        private List<IRecordValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidator"/> class.
        /// </summary>
        /// <param name="validators">Collection of validators.</param>
        protected CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            this.validators = validators.ToList();
        }

        /// <summary>
        /// Validates input values.
        /// </summary>
        /// <param name="person">Personal data.</param>
        /// <param name="income">Person's income.</param>
        /// <param name="tax">Person's tax.</param>
        /// <param name="block">Person's living block.</param>
        public void ValidateParameters(Person person, short income, decimal tax, char block)
        {
            foreach (var validator in this.validators)
            {
                validator.ValidateParameters(person, income, tax, block);
            }
        }
    }
}
