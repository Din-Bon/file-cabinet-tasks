using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Class that export records data in csv file.
    /// </summary>
    public class CsvExport
    {
        private TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvExport"/> class.
        /// </summary>
        /// <param name="writer">Stream to write data.</param>
        public CsvExport(TextWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Write data from record to stream.
        /// </summary>
        /// <param name="record">Record with data.</param>
        public void Write(Record record)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(record.Id);
            builder.Append(',');
            builder.Append(record.FirstName + ',');
            builder.Append(record.LastName + ',');
            builder.Append(record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + ',');
            builder.Append(record.Income);
            builder.Append(',');
            builder.Append(Math.Round(record.Tax, 2));
            builder.Append(',');
            builder.Append(record.Block);
            this.writer.WriteLine(builder);
        }
    }
}
