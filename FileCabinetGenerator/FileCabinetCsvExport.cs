using System.Text;
using System.Globalization;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Class that export records data in csv file.
    /// </summary>
    public class FileCabinetCsvExport
    {
        private TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetCsvExport"/> class.
        /// </summary>
        /// <param name="writer">Stream to write data.</param>
        public FileCabinetCsvExport(TextWriter writer)
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
            builder.Append(record.Name.FirstName + ',');
            builder.Append(record.Name.LastName + ',');
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
