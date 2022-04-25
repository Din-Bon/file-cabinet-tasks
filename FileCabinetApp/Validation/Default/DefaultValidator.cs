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
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person), "empty personal data");
            }

            DefaultFirstNameValidator.ValidateParameters(person);
            DefaultLastNameValidator.ValidateParameters(person);
            DefaultDateOfBirthValidator.ValidateParameters(person);
            DefaultIncomeValidator.ValidateParameters(income);
            DefaultTaxValidator.ValidateParameters(tax);
            DefaultBlockValidator.ValidateParameters(block);
        }
    }
}
