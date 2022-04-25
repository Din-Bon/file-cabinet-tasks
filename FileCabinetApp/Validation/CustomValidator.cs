namespace FileCabinetApp
{
    /// <summary>
    /// Custom validate system.
    /// </summary>
    public class CustomValidator : CompositeValidator
    {
        private const int MinLength = 5;
        private const int MaxLength = 30;
        private const short MinIncome = 1200;
        private const decimal MinTax = 15;
        private const decimal MaxTax = 70;
        private const int FirstAlphabet = 65;
        private const int LastAlphabet = 90;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomValidator"/> class.
        /// </summary>
        public CustomValidator()
            : base(new IRecordValidator[]
            {
                new FirstNameValidator(MinLength, MaxLength),
                new LastNameValidator(MinLength, MaxLength),
                new DateOfBirthValidator(new DateTime(1970, 01, 01), new DateTime(2015, 01, 01)),
                new IncomeValidator(MinIncome),
                new TaxValidator(MinTax, MaxTax),
                new BlockValidator(FirstAlphabet, LastAlphabet),
            })
        {
        }
    }
}
