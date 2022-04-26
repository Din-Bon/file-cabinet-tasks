namespace FileCabinetApp
{
    /// <summary>
    /// Validate last name.
    /// </summary>
    internal class LastNameValidator : IRecordValidator
    {
        private int minNameLength;
        private int maxNameLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameValidator"/> class.
        /// </summary>
        /// <param name="min">Minimum last name length.</param>
        /// <param name="max">Maximum last name length.</param>
        public LastNameValidator(int min, int max)
        {
            this.minNameLength = min;
            this.maxNameLength = max;
        }

        /// <summary>
        /// Validate last name.
        /// </summary>
        /// <param name="person">Personal data.</param>
        /// <param name="income">Person's income.</param>
        /// <param name="tax">Person's tax.</param>
        /// <param name="block">Person's living block.</param>
        /// <exception cref="ArgumentException">Errors with name.</exception>
        public void ValidateParameters(Person person, short income, decimal tax, char block)
        {
            if (string.IsNullOrWhiteSpace(person.LastName) || person.LastName.Length < this.minNameLength || person.LastName.Length > this.maxNameLength)
            {
                throw new ArgumentException("wrong last name", nameof(person));
            }
        }
    }
}
