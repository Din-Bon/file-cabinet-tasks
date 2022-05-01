using Newtonsoft.Json;

namespace FileCabinetApp
{
    /// <summary>
    /// Contain restrictions for tax.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class TaxRange
    {
        /// <summary>
        /// Gets or sets min tax.
        /// </summary>
        /// <value>Tax restriction.</value>
        [JsonProperty("min")]
        public decimal MinTax { get; set; }

        /// <summary>
        /// Gets or sets max tax.
        /// </summary>
        /// <value>Tax restriction.</value>
        [JsonProperty("max")]
        public decimal MaxTax { get; set; }
    }
}
