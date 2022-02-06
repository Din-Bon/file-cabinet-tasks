namespace FileCabinetApp
{
    /// <summary>
    /// Class, that working with the records(with custom validate system).
    /// </summary>
    public class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>
        /// Validates input values.
        /// </summary>
        /// <param name="person">Personal data.</param>
        /// <param name="income">Person's income.</param>
        /// <param name="tax">Person's tax.</param>
        /// <param name="block">Person's living block.</param>
        protected override void ValidateParameters(Person person, short income, decimal tax, char block)
        {
            int minNameLength = 2, maxNameLength = 60;
            DateTime minDateOfBirth = new DateTime(1945, 01, 01);
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

            if (tax < 10 || tax > 100)
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
