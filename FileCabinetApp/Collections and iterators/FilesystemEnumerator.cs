using System.Collections;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Enumerator for filesystem service record collections.
    /// </summary>
    public class FilesystemEnumerator : IEnumerator<FileCabinetRecord>
    {
        private const int MaxNameLength = 60;
        private const int RecordSize = sizeof(short) + (6 * sizeof(int)) + MaxNameLength + MaxNameLength + sizeof(decimal) + sizeof(char) + 16;
        private readonly List<long> filesystemCollection;
        private readonly FileStream stream;
        private int position = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesystemEnumerator"/> class.
        /// </summary>
        /// <param name="collection">Collection of record positions in file.</param>
        /// <param name="stream">File.</param>
        public FilesystemEnumerator(List<long> collection, FileStream stream)
        {
            this.filesystemCollection = collection;
            this.stream = stream;
        }

        /// <summary>
        /// Gets FileCabinetRecord from collection
        /// by current position.
        /// </summary>
        /// <value>Record.</value>
        public FileCabinetRecord Current
        {
            get
            {
                if (this.position >= this.filesystemCollection.Count || this.position == -1)
                {
                    throw new InvalidOperationException();
                }

                this.stream.Position = this.filesystemCollection[this.position];
                byte[] recordInBytes = new byte[RecordSize];
                this.stream.Read(recordInBytes, 0, RecordSize);
                FileCabinetRecord record = BytesToRecord(recordInBytes);
                return record;
            }
        }

        /// <summary>
        /// Gets by default object from
        /// IEnumerator FileCabinetRecord.
        /// </summary>
        /// <value>Record.</value>
        object IEnumerator.Current => this.Current;

        /// <summary>
        /// Increment positions.
        /// </summary>
        /// <returns>Success of operation.</returns>
        public bool MoveNext()
        {
            if (this.position < this.filesystemCollection.Count - 1 && this.filesystemCollection.Count > 0)
            {
                this.position++;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set position at start.
        /// </summary>
        public void Reset() => this.position = -1;

        /// <summary>
        /// Dispose enumerator.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Convert bytes to record parameters.
        /// </summary>
        /// <param name="bytes">Bytes with data.</param>
        /// <returns>Record.</returns>
        private static FileCabinetRecord BytesToRecord(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            var record = new FileCabinetRecord();
            byte[] reserved = new byte[16];

            using (var memoryStream = new MemoryStream(bytes))
            using (var binaryReader = new BinaryReader(memoryStream))
            {
                reserved = binaryReader.ReadBytes(16);
                record.Id = binaryReader.ReadInt32();
                var fistNameLength = binaryReader.ReadInt32();
                var fistNameBuffer = binaryReader.ReadBytes(MaxNameLength);
                var lastNameLength = binaryReader.ReadInt32();
                var lastNameBuffer = binaryReader.ReadBytes(MaxNameLength);
                var year = binaryReader.ReadInt32();
                var month = binaryReader.ReadInt32();
                var day = binaryReader.ReadInt32();
                var income = binaryReader.ReadInt16();
                var tax = binaryReader.ReadDecimal();
                var block = binaryReader.ReadChar();

                record.FirstName = Encoding.ASCII.GetString(fistNameBuffer, 0, fistNameLength);
                record.LastName = Encoding.ASCII.GetString(lastNameBuffer, 0, lastNameLength);
                record.DateOfBirth = new DateTime(year, month, day);
                record.Income = income;
                record.Tax = tax;
                record.Block = block;
            }

            return record;
        }
    }
}
