using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Iterator for file cabinet filesystem service.
    /// </summary>
    public class FilesystemIterator : IRecordIterator
    {
        private const int MaxNameLength = 60;
        private const int RecordSize = sizeof(short) + (6 * sizeof(int)) + MaxNameLength + MaxNameLength + sizeof(decimal) + sizeof(char) + 16;
        private readonly List<long> positions;
        private readonly FileStream stream;
        private int currentPosition = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesystemIterator"/> class.
        /// </summary>
        /// <param name="positions">Collection of positions in file.</param>
        /// <param name="stream">File.</param>
        public FilesystemIterator(List<long> positions, FileStream stream)
        {
            this.positions = positions;
            this.stream = stream;
        }

        /// <summary>
        /// Gets next element from collection.
        /// </summary>
        /// <returns>Next element.</returns>
        public FileCabinetRecord GetNext()
        {
            if (this.HasMore())
            {
                this.currentPosition++;
            }

            this.stream.Position = this.positions[this.currentPosition];
            byte[] recordInBytes = new byte[RecordSize];
            this.stream.Read(recordInBytes, 0, RecordSize);
            FileCabinetRecord record = BytesToRecord(recordInBytes);
            return record;
        }

        /// <summary>
        /// Gets true if collection still has elements.
        /// </summary>
        /// <returns>true/false.</returns>
        public bool HasMore()
        {
            if (this.currentPosition + 1 < this.positions.Count)
            {
                return true;
            }

            return false;
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
