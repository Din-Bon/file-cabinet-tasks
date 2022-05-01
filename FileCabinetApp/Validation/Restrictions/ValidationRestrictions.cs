using Newtonsoft.Json;

namespace FileCabinetApp
{
    /// <summary>
    /// Class that describes validation
    /// parameters.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class ValidationRestrictions
    {
        /// <summary>
        /// Gets or sets first name restrictions.
        /// </summary>
        /// <value>First name restriction.</value>
        [JsonProperty("firstName")]
        public FirstNameLength FirstNameLength { get; set; } = new FirstNameLength();

        /// <summary>
        /// Gets or sets last name restrictions.
        /// </summary>
        /// <value>Last name restriction.</value>
        [JsonProperty("lastName")]
        public LastNameLength LastNameLength { get; set; } = new LastNameLength();

        /// <summary>
        /// Gets or sets date of birth range.
        /// </summary>
        /// <value>Date of birth restrictions.</value>
        [JsonProperty("dateOfBirth")]
        public DateOfBirthRange DateOfBirthRange { get; set; } = new DateOfBirthRange();

        /// <summary>
        /// Gets or sets income range.
        /// </summary>
        /// <value>Income restrictions.</value>
        [JsonProperty("income")]
        public IncomeRange IncomeRange { get; set; } = new IncomeRange();

        /// <summary>
        /// Gets or sets tax range.
        /// </summary>
        /// <value>Tax restrictions.</value>
        [JsonProperty("tax")]
        public TaxRange TaxRange { get; set; } = new TaxRange();

        /// <summary>
        /// Gets or sets block letter range.
        /// </summary>
        /// <value>Block restrictions.</value>
        [JsonProperty("block")]
        public BlockLetterRange BlockLetterRange { get; set; } = new BlockLetterRange();
    }
}
