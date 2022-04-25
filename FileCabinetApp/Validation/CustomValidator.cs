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

            int minLength = 5, maxLength = 30;
            DateTime from = new DateTime(1970, 01, 01);
            DateTime to = new DateTime(2015, 01, 01);
            short minIncome = 1200;
            decimal minTax = 15, maxTax = 70;
            int firstAlphabet = 65, lastAlphabet = 90;

            new FirstNameValidator(minLength, maxLength).ValidateParameters(person);
            new LastNameValidator(minLength, maxLength).ValidateParameters(person);
            new DateOfBirthValidator(from, to).ValidateParameters(person);
            new IncomeValidator(minIncome).ValidateParameters(income);
            new TaxValidator(minTax, maxTax).ValidateParameters(tax);
            new BlockValidator(firstAlphabet, lastAlphabet).ValidateParameters(block);
        }
    }
}
