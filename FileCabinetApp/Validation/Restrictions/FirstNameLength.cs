using Newtonsoft.Json;

namespace FileCabinetApp
{
    /// <summary>
    /// Contain retrictions for first name.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class FirstNameLength
    {
        /// <summary>
        /// Gets or sets min first name length.
        /// </summary>
        /// <value>Name restriction.</value>
        [JsonProperty("min")]
        public int MinFirstNameLength { get; set; }

        /// <summary>
        /// Gets or sets max first name length.
        /// </summary>
        /// <value>Name restriction.</value>
        [JsonProperty("max")]
        public int MaxFirstNameLength { get; set; }
    }
}
