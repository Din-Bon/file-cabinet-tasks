using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Structure describing a person.
    /// </summary>
    public class Record
    {
        /// <summary>
        /// Gets or sets persons id.
        /// </summary>
        /// <value>Persons id.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets persons first name.
        /// </summary>
        /// <value>Persons first name.</value>
        public string FirstName { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets persons last name.
        /// </summary>
        /// <value>Persons last name.</value>
        public string LastName { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets persons date of birth.
        /// </summary>
        /// <value>Persons date of birth.</value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets persons income.
        /// </summary>
        /// <value>Persons income.</value>
        public short Income { get; set; }

        /// <summary>
        /// Gets or sets persons tax.
        /// </summary>
        /// <value>Persons tax.</value>
        public decimal Tax { get; set; }

        /// <summary>
        /// Gets or sets persons living block.
        /// </summary>
        /// <value>Persons living block.</value>
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
