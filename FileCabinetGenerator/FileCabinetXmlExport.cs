using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Class that export records data in xml file.
    /// </summary>
    public class FileCabinetXmlExport
    {
        private StreamWriter stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetXmlExport"/> class.
        /// </summary>
        /// <param name="stream">Stream to write data.</param>
        public FileCabinetXmlExport(StreamWriter stream)
        {
            this.stream = stream;
        }

        /// <summary>
        /// Write data from record to stream.
        /// </summary>
        /// <param name="records">Record with data.</param>
        public void Write(FileCabinetRecord[] records)
        {
            XmlRootAttribute root = new XmlRootAttribute();
            root.ElementName = "records";
            XmlSerializerNamespaces space = new XmlSerializerNamespaces();
            space.Add("", "");
            XmlSerializer serializer = new XmlSerializer(typeof(FileCabinetRecord[]), root);
            serializer.Serialize(stream, records, space);
        }
    }
}
