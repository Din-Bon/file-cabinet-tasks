namespace FileCabinetApp
{
    /// <summary>
    /// Custom validate system.
    /// </summary>
    public class CustomValidator : IRecordValidator
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
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person), "empty personal data");
            }

            ValidateFirstName(person);
            ValidateLastName(person);
            ValidateDateOfBirth(person);
            ValidateIncome(income);
            ValidateTax(tax);
            ValidateBlock(block);
        }

        /// <summary>
        /// Validate first name.
        /// </summary>
        /// <param name="person">Personal data.</param>
        /// <exception cref="ArgumentException">Errors with name.</exception>
        private static void ValidateFirstName(Person person)
        {
            int minNameLength = 2, maxNameLength = 60;

            if (string.IsNullOrWhiteSpace(person.FirstName) || person.FirstName.Length < minNameLength || person.FirstName.Length > maxNameLength)
            {
                throw new ArgumentException("wrong first name", nameof(person));
            }
        }

        /// <summary>
        /// Validate last name.
        /// </summary>
        /// <param name="person">Personal data.</param>
        /// <exception cref="ArgumentException">Errors with name.</exception>
        private static void ValidateLastName(Person person)
        {
            int minNameLength = 2, maxNameLength = 60;

            if (string.IsNullOrWhiteSpace(person.LastName) || person.LastName.Length < minNameLength || person.LastName.Length > maxNameLength)
            {
                throw new ArgumentException("wrong last name", nameof(person));
            }
        }

        /// <summary>
        /// Validate date of birth.
        /// </summary>
        /// <param name="person">Personal data.</param>
        /// <exception cref="ArgumentException">Date of birth out of range.</exception>
        private static void ValidateDateOfBirth(Person person)
        {
            DateTime minDateOfBirth = new DateTime(1945, 01, 01);
            DateTime maxDateOfBirth = DateTime.Today;

            if (person.DateOfBirth < minDateOfBirth || person.DateOfBirth > maxDateOfBirth)
            {
                throw new ArgumentException("wrong date of birth", nameof(person));
            }
        }

        /// <summary>
        /// Validate income.
        /// </summary>
        /// <param name="income">Short value, characterize persons income.</param>
        /// <exception cref="ArgumentException">Value less than zero.</exception>
        private static void ValidateIncome(short income)
        {
            if (income < 0)
            {
                throw new ArgumentException("wrong income", nameof(income));
            }
        }

        /// <summary>
        /// Validate tax.
        /// </summary>
        /// <param name="tax">Decimal, characterize tax for person.</param>
        /// <exception cref="ArgumentException">Tax out of range.</exception>
        private static void ValidateTax(decimal tax)
        {
            if (tax < 10 || tax > 100)
            {
                throw new ArgumentException("wrong tax", nameof(tax));
            }
        }

        /// <summary>
        /// Validate block.
        /// </summary>
        /// <param name="block">Char, characterize persons living block.</param>
        /// <exception cref="ArgumentException">Block is not capital letter in english alphabet.</exception>
        private static void ValidateBlock(char block)
        {
            int firstAlphabetLetter = 65, lastAlphabetLetter = 90;

            if (block < firstAlphabetLetter || block > lastAlphabetLetter)
            {
                throw new ArgumentException("wrong block letter", nameof(block));
            }
        }
    }
}
