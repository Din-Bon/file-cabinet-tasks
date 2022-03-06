using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Class creating an array from records.
    /// </summary>
    [Serializable, XmlRoot(Namespace = "https://github.com/Din-Bon", ElementName = "FileCabinetRecords")]
    public class FileCabinetRecordArray
    {
        /// <summary>
        /// Gets or sets record array.
        /// </summary>
        /// <value>Records.</value>
        [XmlArray("records")]
        public FileCabinetRecord[]? Records { get; set; }
    }

    /// <summary>
    /// Class describing a person.
    /// </summary>
    [XmlRoot("record")]
    public class FileCabinetRecord
    {
        /// <summary>
        /// Gets or sets persons id.
        /// </summary>
        /// <value>Persons id.</value>
        [XmlAttribute("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets persons first name.
        /// </summary>
        /// <value>Persons first name.</value>
        [XmlElement("firstName")]
        public string FirstName { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets persons last name.
        /// </summary>
        /// <value>Persons last name.</value>
        [XmlElement("lastName")]
        public string LastName { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets persons date of birth.
        /// </summary>
        /// <value>Persons date of birth.</value>
        [XmlElement("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

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
        [XmlElement("block")]
        public char Block { get; set; }

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
