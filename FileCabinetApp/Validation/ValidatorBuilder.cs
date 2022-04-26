namespace FileCabinetApp
{
    /// <summary>
    /// Class that create validator.
    /// </summary>
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators = new ();

        /// <summary>
        /// Add first name validator.
        /// </summary>
        /// <param name="min">Min length.</param>
        /// <param name="max">Max length.</param>
        /// <returns>This object.</returns>
        public ValidatorBuilder ValidateFirstName(int min, int max)
        {
            this.validators.Add(new FirstNameValidator(min, max));
            return this;
        }

        /// <summary>
        /// Add last name validator.
        /// </summary>
        /// <param name="min">Min length.</param>
        /// <param name="max">Max length.</param>
        /// <returns>This object.</returns>
        public ValidatorBuilder ValidateLastName(int min, int max)
        {
            this.validators.Add(new LastNameValidator(min, max));
            return this;
        }

        /// <summary>
        /// Add date of birth validator.
        /// </summary>
        /// <param name="from">Max date of birth.</param>
        /// <param name="to">Min date of birth.</param>
        /// <returns>This object.</returns>
        public ValidatorBuilder ValidateDateOfBirth(DateTime from, DateTime to)
        {
            this.validators.Add(new DateOfBirthValidator(from, to));
            return this;
        }

        /// <summary>
        /// Add income validator.
        /// </summary>
        /// <param name="min">Min income.</param>
        /// <returns>This object.</returns>
        public ValidatorBuilder ValidateIncome(short min)
        {
            this.validators.Add(new IncomeValidator(min));
            return this;
        }

        /// <summary>
        /// Add tax validator.
        /// </summary>
        /// <param name="min">Min tax.</param>
        /// <param name="max">Max tax.</param>
        /// <returns>This object.</returns>
        public ValidatorBuilder ValidateTax(decimal min, decimal max)
        {
            this.validators.Add(new TaxValidator(min, max));
            return this;
        }

        /// <summary>
        /// Add block validator.
        /// </summary>
        /// <param name="first">First accessible block letter.</param>
        /// <param name="last">Last accessible block letter.</param>
        /// <returns>This object.</returns>
        public ValidatorBuilder ValidateBlock(int first, int last)
        {
            this.validators.Add(new BlockValidator(first, last));
            return this;
        }

        /// <summary>
        /// Create object of CompositeValidator type.
        /// </summary>
        /// <returns>CompositeValidator.</returns>
        public IRecordValidator Create()
        {
            return new CompositeValidator(this.validators);
        }
    }
}
