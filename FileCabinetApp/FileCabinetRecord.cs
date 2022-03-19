using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Class describing a person.
    /// </summary>
    [XmlType("record")]
    public class FileCabinetRecord
    {
        /// <summary>
        /// Gets or sets persons id.
        /// </summary>
        /// <value>Persons id.</value>
        [XmlAttribute("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets persons name.
        /// </summary>
        /// <value>Persons name.</value>
        [XmlElement("name")]
        public PersonName Name { get; set; } = new PersonName();

        /// <summary>
        /// Gets or sets persons first name.
        /// </summary>
        /// <value>Persons first name.</value>
        [XmlIgnore]
        public string FirstName
        {
            get { return this.Name.FirstName; }
            set { this.Name.FirstName = value; }
        }

        /// <summary>
        /// Gets or sets persons last name.
        /// </summary>
        /// <value>Persons last name.</value>
        [XmlIgnore]
        public string LastName
        {
            get { return this.Name.LastName; }
            set { this.Name.LastName = value; }
        }

        /// <summary>
        /// Gets or sets persons date of birth.
        /// </summary>
        /// <value>Persons date of birth.</value>
        [XmlIgnore]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets persons date of birth in string format.
        /// </summary>
        /// <value>Persons date of birth.</value>
        [XmlElement("dateOfBirth")]
        public string DateString
        {
            get { return this.DateOfBirth.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture); }
            set { this.DateOfBirth = DateTime.ParseExact(value, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// Gets or sets persons income.
        /// </summary>
        /// <value>Persons income.</value>
        [XmlElement("income")]
        public short Income { get; set; }

        /// <summary>
        /// Gets or sets persons tax.
        /// </summary>
        /// <value>Persons tax.</value>
        [XmlElement("tax")]
        public decimal Tax { get; set; }

        /// <summary>
        /// Gets or sets persons living block.
        /// </summary>
        /// <value>Persons living block.</value>
        [XmlIgnore]
        public char Block { get; set; }

        /// <summary>
        /// Gets or sets persons living block in string.
        /// </summary>
        /// <value>Persons living block.</value>
        [XmlElement("block")]
        public string StringBlock
        {
            get { return this.Block.ToString(); }
            set { this.Block = value.Single(); }
        }

        /// <summary>
        /// Build string from records parameters.
        /// </summary>
        /// <returns>Record string representation.</returns>
        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"#{this.Id}, ");
            builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"{this.FirstName}, ");
            builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"{this.LastName}, ");
            builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"{this.DateOfBirth.ToString("yyyy-MMM-dd", System.Globalization.CultureInfo.InvariantCulture)}, ");
            builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"{this.Income}, ");
            builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"{this.Tax}, ");
            builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"{this.Block}");
            return builder.ToString();
        }
    }
}