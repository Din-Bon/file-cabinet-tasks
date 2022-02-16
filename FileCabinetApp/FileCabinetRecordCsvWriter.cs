using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Class that create snapshot of records in csv format.
    /// </summary>
    internal class FileCabinetRecordCsvWriter
    {
        private TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">Stream to write data.</param>
        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Write data from record to stream.
        /// </summary>
        /// <param name="record">Record with data.</param>
        public void Write(FileCabinetRecord record)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(record.Id);
            builder.Append(',');
            builder.Append(record.FirstName + ',');
            builder.Append(record.LastName + ',');
            builder.Append(record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + ',');
            builder.Append(record.Income);
            builder.Append(',');
            builder.Append(record.Tax);
            builder.Append(',');
            builder.Append(record.Block);
            this.writer.WriteLine(builder);
        }
    }
}
