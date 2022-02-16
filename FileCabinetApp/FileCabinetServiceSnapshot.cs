using System.Xml;

namespace FileCabinetApp
{
    /// <summary>
    /// Class that create snapshot of records.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="records">Array of records.</param>
        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            this.records = records;
        }

        /// <summary>
        /// Save records data to csv file.
        /// </summary>
        /// <param name="writer">Stream that will contain records data.</param>
        public void SaveToCsv(StreamWriter writer)
        {
            writer.WriteLine("Id,First Name,Last Name,Date of Birth,Income,Tax,Block");
            FileCabinetRecordCsvWriter csvWriter = new FileCabinetRecordCsvWriter(writer);

            foreach (FileCabinetRecord record in this.records)
            {
                csvWriter.Write(record);
            }
        }

        /// <summary>
        /// Save records data to xml file.
        /// </summary>
        /// <param name="writer">Stream that will contain records data.</param>
        public void SaveToXml(StreamWriter writer)
        {
            XmlWriter xmlWriter = XmlWriter.Create(writer);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("records");
            FileCabinetRecordXmlWriter recordXmlWriter = new FileCabinetRecordXmlWriter(xmlWriter);

            foreach (FileCabinetRecord record in this.records)
            {
                recordXmlWriter.Write(record);
            }

            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            xmlWriter.Close();
        }
    }
}
