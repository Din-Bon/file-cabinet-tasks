using Newtonsoft.Json;

namespace FileCabinetApp
{
    /// <summary>
    /// Contain restrictions for income.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class IncomeRange
    {
        /// <summary>
        /// Gets or sets min income.
        /// </summary>
        /// <value>Income restriction.</value>
        [JsonProperty("min")]
        public short MinIncome { get; set; }
    }
}
