using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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
        /// <param name="record">Record with data.</param>
        public void Write(FileCabinetRecord[] record)
        {
            FileCabinetRecordArray recordArray = new FileCabinetRecordArray();
            recordArray.Records = record;
            XmlSerializer serializer = new XmlSerializer(typeof(FileCabinetRecordArray));
            serializer.Serialize(stream, recordArray);
        }
    }
}
