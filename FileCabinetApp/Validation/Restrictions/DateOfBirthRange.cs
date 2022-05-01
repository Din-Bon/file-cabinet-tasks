using Newtonsoft.Json;

namespace FileCabinetApp
{
    /// <summary>
    /// Contain restrictions for date of birth.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class DateOfBirthRange
    {
        /// <summary>
        /// Gets or sets minimum date of birth.
        /// </summary>
        /// <value>Date of birth restriction.</value>
        [JsonProperty("from")]
        public DateTime From { get; set; }

        /// <summary>
        /// Gets or sets maximum date of birth.
        /// </summary>
        /// <value>Date of birth restriction.</value>
        [JsonProperty("to")]
        public DateTime To { get; set; }
    }
}
