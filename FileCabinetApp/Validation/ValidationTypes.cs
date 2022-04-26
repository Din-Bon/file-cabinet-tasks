namespace FileCabinetApp
{
    /// <summary>
    /// In that class we can add some
    /// validation type.
    /// </summary>
    public static class ValidationTypes
    {
        /// <summary>
        /// Create validator with
        /// default set of rules.
        /// </summary>
        /// <param name="validator">Builder for validator.</param>
        /// <returns>Validator.</returns>
        public static IRecordValidator CreateDefault(this ValidatorBuilder validator)
        {
            int minLength = 2, maxLength = 50;
            short minIncome = 100;
            decimal minTax = 0, maxTax = 100;
            int firstAlphabet = 65, lastAlphabet = 90;
            DateTime from = new DateTime(1950, 01, 01);
            DateTime to = new DateTime(2020, 01, 01);
            validator.ValidateFirstName(minLength, maxLength)
                .ValidateLastName(minLength, maxLength)
                .ValidateDateOfBirth(from, to)
                .ValidateIncome(minIncome)
                .ValidateTax(minTax, maxTax)
                .ValidateBlock(firstAlphabet, lastAlphabet);
            return validator.Create();
        }

        /// <summary>
        /// Create validator with
        /// custom set of rules.
        /// </summary>
        /// <param name="validator">Builder for validator.</param>
        /// <returns>Validator.</returns>
        public static IRecordValidator CreateCustom(this ValidatorBuilder validator)
        {
            int minLength = 5, maxLength = 20;
            short minIncome = 200;
            decimal minTax = 15, maxTax = 75;
            int firstAlphabet = 65, lastAlphabet = 90;
            DateTime from = new DateTime(1945, 01, 01);
            DateTime to = new DateTime(2015, 01, 01);
            validator.ValidateFirstName(minLength, maxLength)
                .ValidateLastName(minLength, maxLength)
                .ValidateDateOfBirth(from, to)
                .ValidateIncome(minIncome)
                .ValidateTax(minTax, maxTax)
                .ValidateBlock(firstAlphabet, lastAlphabet);
            return validator.Create();
        }
    }
}
