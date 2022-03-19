using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Class, that import xml.
    /// </summary>
    public class FileCabinetRecordXmlReader
    {
        private StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="stream">.</param>
        public FileCabinetRecordXmlReader(StreamReader stream)
        {
            this.reader = stream;
        }

        /// <summary>
        /// Read data from xml file.
        /// </summary>
        /// <returns>List of records.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            XmlRootAttribute root = new XmlRootAttribute();
            root.ElementName = "records";
            XmlSerializer serializer = new XmlSerializer(typeof(FileCabinetRecord[]), root);
            var records = serializer.Deserialize(XmlReader.Create(this.reader));

            if (records == null)
            {
                throw new ArgumentException("empty xml file");
            }

            IList<FileCabinetRecord> list = (FileCabinetRecord[])records;
            return list;
        }
    }
}
