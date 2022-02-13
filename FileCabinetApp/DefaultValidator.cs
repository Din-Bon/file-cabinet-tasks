namespace FileCabinetApp
{
    /// <summary>
    /// Default validate system.
    /// </summary>
    public class DefaultValidator : IRecordValidator
    {
        /// <summary>
        /// Validates input values.
        /// </summary>
        /// <param name="person">Personal data.</param>
        /// <param name="income">Person's income.</param>
        /// <param name="tax">Person's tax.</param>
        /// <param name="block">Person's living block.</param>
        public void ValidateParameters(Person person, short income, decimal tax, char block)
        {
            int minNameLength = 2, maxNameLength = 60;
            DateTime minDateOfBirth = new DateTime(1950, 01, 01);
            DateTime maxDateOfBirth = DateTime.Today;
            int firstAlphabetLetter = 65, lastAlphabetLetter = 90;

            if (string.IsNullOrWhiteSpace(person.FirstName) || person.FirstName.Length < minNameLength || person.FirstName.Length > maxNameLength)
            {
                throw new ArgumentException("wrong first name", nameof(person));
            }

            if (string.IsNullOrWhiteSpace(person.LastName) || person.LastName.Length < minNameLength || person.LastName.Length > maxNameLength)
            {
                throw new ArgumentException("wrong last name", nameof(person));
            }

            if (person.DateOfBirth < minDateOfBirth || person.DateOfBirth > maxDateOfBirth)
            {
                throw new ArgumentException("wrong date of birth", nameof(person));
            }

            if (income < 0)
            {
                throw new ArgumentException("wrong income", nameof(income));
            }

            if (tax < 0 || tax > 100)
            {
                throw new ArgumentException("wrong tax", nameof(tax));
            }

            if (block < firstAlphabetLetter || block > lastAlphabetLetter)
            {
                throw new ArgumentException("wrong block letter", nameof(block));
            }
        }
    }
}
