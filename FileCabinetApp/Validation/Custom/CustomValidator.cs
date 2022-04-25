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

            CustomFirstNameValidator.ValidateParameters(person);
            CustomLastNameValidator.ValidateParameters(person);
            CustomDateOfBirthValidator.ValidateParameters(person);
            CustomIncomeValidator.ValidateParameters(income);
            CustomTaxValidator.ValidateParameters(tax);
            CustomBlockValidator.ValidateParameters(block);
        }
    }
}
