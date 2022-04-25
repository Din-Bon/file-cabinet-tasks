namespace FileCabinetApp
{
    /// <summary>
    /// Validate date of birth by default ruleset.
    /// </summary>
    internal class DefaultDateOfBirthValidator
    {
        /// <summary>
        /// Validates date of birth.
        /// </summary>
        /// <param name="person">Personal data.</param>
        /// <exception cref="ArgumentException">Date of birth out of range.</exception>
        public static void ValidateParameters(Person person)
        {
            DateTime minDateOfBirth = new DateTime(1950, 01, 01);
            DateTime maxDateOfBirth = DateTime.Today;

            if (person.DateOfBirth < minDateOfBirth || person.DateOfBirth > maxDateOfBirth)
            {
                throw new ArgumentException("wrong date of birth", nameof(person));
            }
        }
    }
}
