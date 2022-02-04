using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Class describing a main persons data.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Gets or sets persons first name.
        /// </summary>
        /// <value>Persons first name.</value>
        public string? FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets persons last name.
        /// </summary>
        /// <value>Persons last name.</value>
        public string? LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets persons date of birth.
        /// </summary>
        /// <value>Persons date of birth.</value>
        public DateTime DateOfBirth { get; set; }
    }
}
