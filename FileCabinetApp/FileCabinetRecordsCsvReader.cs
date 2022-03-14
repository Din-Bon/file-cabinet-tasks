using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Class, that working with the records.
    /// </summary>
    public class FileCabinetRecordsCsvReader
    {
        private StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordsCsvReader"/> class.
        /// </summary>
        /// <param name="stream">.</param>
        public FileCabinetRecordsCsvReader(StreamReader stream)
        {
            this.reader = stream;
        }

        /// <summary>
        /// Read data from csv file.
        /// </summary>
        /// <returns>List of records.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            string? line;
            List<FileCabinetRecord> list = new List<FileCabinetRecord>();
            this.reader.ReadLine();

            while ((line = this.reader.ReadLine()) != null)
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                string[] parameters = line.Split(',');
                int id = Convert.ToInt32(parameters[0], provider);
                string firstName = parameters[1], lastName = parameters[2];
                DateTime dateOfBirth = DateTime.ParseExact(parameters[3] + " 00:00:00", $"MM/dd/yyyy hh:mm:ss", provider);
                short income = Convert.ToInt16(parameters[4], provider);
                char block = '\0';
                decimal tax = -1;
                NumberStyles style = NumberStyles.AllowDecimalPoint;

                if (parameters.Length == 8)
                {
                    string stringTax = string.Concat(parameters[5], '.', parameters[6]);
                    tax = decimal.Parse(stringTax, style, provider);
                    block = Convert.ToChar(parameters[7], provider);
                }
                else if (parameters.Length == 7)
                {
                    tax = Convert.ToDecimal(parameters[5], provider);
                    block = Convert.ToChar(parameters[6], provider);
                }

                if (tax != -1)
                {
                    FileCabinetRecord record = new FileCabinetRecord
                    {
                        Id = id,
                        FirstName = firstName,
                        LastName = lastName,
                        DateOfBirth = dateOfBirth,
                        Income = income,
                        Tax = tax,
                        Block = block,
                    };

                    list.Add(record);
                }
            }

            return list;
        }
    }
}
