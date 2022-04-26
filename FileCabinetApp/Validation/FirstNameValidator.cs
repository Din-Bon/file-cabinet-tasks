namespace FileCabinetApp
{
    /// <summary>
    /// Validate first name.
    /// </summary>
    internal class FirstNameValidator : IRecordValidator
    {
        private int minNameLength;
        private int maxNameLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstNameValidator"/> class.
        /// </summary>
        /// <param name="min">Minimum first name length.</param>
        /// <param name="max">Maximum first name length.</param>
        public FirstNameValidator(int min, int max)
        {
            this.minNameLength = min;
            this.maxNameLength = max;
        }

        /// <summary>
        /// Validate first name.
        /// </summary>
        /// <param name="person">Personal data.</param>
        /// <param name="income">Person's income.</param>
        /// <param name="tax">Person's tax.</param>
        /// <param name="block">Person's living block.</param>
        /// <exception cref="ArgumentException">Errors with name.</exception>
        public void ValidateParameters(Person person, short income, decimal tax, char block)
        {
            if (string.IsNullOrWhiteSpace(person.FirstName) || person.FirstName.Length < this.minNameLength || person.FirstName.Length > this.maxNameLength)
            {
                throw new ArgumentException("wrong first name", nameof(person));
            }
        }
    }
}
