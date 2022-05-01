using Newtonsoft.Json;

namespace FileCabinetApp
{
    /// <summary>
    /// Contain restrictions for last name.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class LastNameLength
    {
        /// <summary>
        /// Gets or sets min last name length.
        /// </summary>
        /// <value>Name restriction.</value>
        [JsonProperty("min")]
        public int MinLastNameLength { get; set; }

        /// <summary>
        /// Gets or sets max last name length.
        /// </summary>
        /// <value>Name restriction.</value>
        [JsonProperty("max")]
        public int MaxLastNameLength { get; set; }
    }
}
