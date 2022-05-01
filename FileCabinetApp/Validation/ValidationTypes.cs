using Newtonsoft.Json;

namespace FileCabinetApp
{
    /// <summary>
    /// In that class we can add some
    /// validation type.
    /// </summary>
    public static class ValidationTypes
    {
        private const string JSONPath = "..\\..\\..\\Properties\\validation-rules.json";

        /// <summary>
        /// Create validator with
        /// default set of rules.
        /// </summary>
        /// <param name="validator">Builder for validator.</param>
        /// <returns>Validator.</returns>
        public static IRecordValidator CreateDefault(this ValidatorBuilder validator)
        {
            string jsonstring = File.ReadAllText(JSONPath);
            var json = JsonConvert.DeserializeObject<ValidationTypeArray>(jsonstring)
                ?? throw new ArgumentNullException(nameof(jsonstring), "can't convert validation-rules.json ruleset to object");
            var defaultRuleset = json.Default;
            return ValidateParameters(validator, defaultRuleset);
        }

        /// <summary>
        /// Create validator with
        /// custom set of rules.
        /// </summary>
        /// <param name="validator">Builder for validator.</param>
        /// <returns>Validator.</returns>
        public static IRecordValidator CreateCustom(this ValidatorBuilder validator)
        {
            string jsonstring = File.ReadAllText(JSONPath);
            var json = JsonConvert.DeserializeObject<ValidationTypeArray>(jsonstring)
                ?? throw new ArgumentNullException(nameof(jsonstring), "can't convert validation-rules.json ruleset to object");
            var customRuleset = json.Custom;
            return ValidateParameters(validator, customRuleset);
        }

        /// <summary>
        /// Validate parameters by selected ruleset.
        /// </summary>
        /// <param name="validator">Builder for validator.</param>
        /// <param name="ruleset">Validation mode.</param>
        /// <returns>Validator.</returns>
        private static IRecordValidator ValidateParameters(this ValidatorBuilder validator, ValidationRestrictions ruleset)
        {
            int minFirstNameLength = ruleset.FirstNameLength.MinFirstNameLength,
                maxFirstNameLength = ruleset.FirstNameLength.MaxFirstNameLength;
            int minLastNameLength = ruleset.LastNameLength.MinLastNameLength,
                maxLastNameLength = ruleset.LastNameLength.MaxLastNameLength;
            short minIncome = ruleset.IncomeRange.MinIncome;
            decimal minTax = ruleset.TaxRange.MinTax,
                maxTax = ruleset.TaxRange.MaxTax;
            int firstAlphabet = ruleset.BlockLetterRange.FirstAlphabet,
                lastAlphabet = ruleset.BlockLetterRange.LastAlphabet;
            DateTime from = ruleset.DateOfBirthRange.From;
            DateTime to = ruleset.DateOfBirthRange.To;
            validator.ValidateFirstName(minFirstNameLength, maxFirstNameLength)
                .ValidateLastName(minLastNameLength, maxLastNameLength)
                .ValidateDateOfBirth(from, to)
                .ValidateIncome(minIncome)
                .ValidateTax(minTax, maxTax)
                .ValidateBlock(firstAlphabet, lastAlphabet);
            return validator.Create();
        }
    }
}
