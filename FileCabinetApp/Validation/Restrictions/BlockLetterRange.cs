using Newtonsoft.Json;

namespace FileCabinetApp
{
    /// <summary>
    /// Contain restrictions for block.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class BlockLetterRange
    {
        /// <summary>
        /// Gets or sets first alphabet letter (in ASCII).
        /// </summary>
        /// <value>Block restriction.</value>
        [JsonProperty("first")]
        public int FirstAlphabet { get; set; }

        /// <summary>
        /// Gets or sets last alphabet letter (in ASCII).
        /// </summary>
        /// <value>Block restriction.</value>
        [JsonProperty("last")]
        public int LastAlphabet { get; set; }
    }
}
