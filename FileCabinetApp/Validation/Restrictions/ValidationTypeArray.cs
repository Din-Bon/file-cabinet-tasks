using Newtonsoft.Json;

namespace FileCabinetApp
{
    /// <summary>
    /// Contain existing type of validation.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class ValidationTypeArray
    {
        /// <summary>
        /// Gets or sets array of restrictions
        /// for some type of validation.
        /// </summary>
        /// <value>Array of validation rules.</value>
        [JsonProperty("default")]
        public ValidationRestrictions Default { get; set; } = new ValidationRestrictions();

        /// <summary>
        /// Gets or sets array of restrictions
        /// for some type of validation.
        /// </summary>
        /// <value>Array of validation rules.</value>
        [JsonProperty("custom")]
        public ValidationRestrictions Custom { get; set; } = new ValidationRestrictions();
    }
}
