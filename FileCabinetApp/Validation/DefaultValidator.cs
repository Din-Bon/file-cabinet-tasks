namespace FileCabinetApp
{
    /// <summary>
    /// Default validate system.
    /// </summary>
    public class DefaultValidator : CompositeValidator
    {
        private const int MinLength = 2;
        private const int MaxLength = 20;
        private const short MinIncome = 100;
        private const decimal MinTax = 0;
        private const decimal MaxTax = 100;
        private const int FirstAlphabet = 65;
        private const int LastAlphabet = 90;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValidator"/> class.
        /// </summary>
        public DefaultValidator()
            : base(new IRecordValidator[]
            {
                new FirstNameValidator(MinLength, MaxLength),
                new LastNameValidator(MinLength, MaxLength),
                new DateOfBirthValidator(new DateTime(1950, 01, 01), new DateTime(2015, 01, 01)),
                new IncomeValidator(MinIncome),
                new TaxValidator(MinTax, MaxTax),
                new BlockValidator(FirstAlphabet, LastAlphabet),
            })
        {
        }
    }
}
