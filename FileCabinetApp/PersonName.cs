using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Class describing name.
    /// </summary>
    public class PersonName
    {
        /// <summary>
        /// Gets or sets persons first name.
        /// </summary>
        /// <value>Persons first name.</value>
        [XmlAttribute("first")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets persons last name.
        /// </summary>
        /// <value>Persons last name.</value>
        [XmlAttribute("last")]
        public string LastName { get; set; } = string.Empty;
    }
}
