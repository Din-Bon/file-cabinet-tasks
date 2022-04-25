namespace FileCabinetApp
{
    /// <summary>
    /// Validate date of birth.
    /// </summary>
    internal class DateOfBirthValidator
    {
        private DateTime from;
        private DateTime to;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateOfBirthValidator"/> class.
        /// </summary>
        /// <param name="min">Oldest users age.</param>
        /// <param name="max">Youngest users age.</param>
        public DateOfBirthValidator(DateTime min, DateTime max)
        {
            this.from = min;
            this.to = max;
        }

        /// <summary>
        /// Validate date of birth.
        /// </summary>
        /// <param name="person">Personal data.</param>
        /// <exception cref="ArgumentException">Date of birth out of range.</exception>
        public void ValidateParameters(Person person)
        {
            if (person.DateOfBirth < this.from || person.DateOfBirth > this.to)
            {
                throw new ArgumentException("wrong date of birth", nameof(person));
            }
        }
    }
}
