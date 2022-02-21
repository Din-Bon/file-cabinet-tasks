using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Class, that working with the records.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private const int MaxNameLength = 60;
        private const int RecordSize = (2 * sizeof(short)) + (6 * sizeof(int)) + MaxNameLength + MaxNameLength + sizeof(decimal) + sizeof(char);
        private readonly FileStream fileStream;
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">.</param>
        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        /// <summary>
        /// Convert record parameters to bytes.
        /// </summary>
        /// <param name="record">Record with data.</param>
        /// <returns>Byte array.</returns>
        public static byte[] RecordToBytes(FileCabinetRecord record)
        {
            if (string.IsNullOrEmpty(record.FirstName) || string.IsNullOrEmpty(record.LastName))
            {
                throw new ArgumentNullException(nameof(record));
            }

            var bytes = new byte[RecordSize];
            using (var memoryStream = new MemoryStream(bytes))
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                binaryWriter.Seek(2, SeekOrigin.Begin);
                binaryWriter.Write(record.Id);

                var firstNameBytes = Encoding.ASCII.GetBytes(record.FirstName.ToCharArray());
                var firstNameBuffer = new byte[MaxNameLength];
                var lastNameBytes = Encoding.ASCII.GetBytes(record.LastName.ToCharArray());
                var lastNameBuffer = new byte[MaxNameLength];

                int firstNameLength = firstNameBytes.Length;
                int lastNameLength = lastNameBytes.Length;

                if (firstNameLength > MaxNameLength)
                {
                    firstNameLength = MaxNameLength;
                }

                if (lastNameLength > MaxNameLength)
                {
                    lastNameLength = MaxNameLength;
                }

                Array.Copy(firstNameBytes, 0, firstNameBuffer, 0, firstNameLength);
                Array.Copy(lastNameBytes, 0, lastNameBuffer, 0, lastNameLength);

                binaryWriter.Write(firstNameLength);
                binaryWriter.Write(firstNameBuffer);
                binaryWriter.Write(lastNameLength);
                binaryWriter.Write(lastNameBuffer);
                binaryWriter.Write(record.DateOfBirth.Year);
                binaryWriter.Write(record.DateOfBirth.Month);
                binaryWriter.Write(record.DateOfBirth.Day);
                binaryWriter.Write(record.Income);
                binaryWriter.Write(record.Tax);
                binaryWriter.Write(record.Block);
            }

            return bytes;
        }

        public static FileCabinetRecord BytesToRecord(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            var record = new FileCabinetRecord();

            using (var memoryStream = new MemoryStream(bytes))
            using (var binaryReader = new BinaryReader(memoryStream))
            {
                short empty = binaryReader.ReadInt16();
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

        /// <summary>
        /// Create record from the input parameters.
        /// </summary>
        /// <param name="person">Personal data.</param>
        /// <param name="income">Person's income.</param>
        /// <param name="tax">Person's tax.</param>
        /// <param name="block">Person's living block.</param>
        /// <returns>Records id.</returns>
        public int CreateRecord(Person person, short income, decimal tax, char block)
        {
            this.fileStream.Position = this.fileStream.Length;
            DefaultValidator validator = new DefaultValidator();
            validator.ValidateParameters(person, income, tax, block);
            int id = (int)(this.fileStream.Length / (long)RecordSize) + 1;
            FileCabinetRecord record = new FileCabinetRecord
            {
                Id = id,
                FirstName = person.FirstName ?? throw new ArgumentNullException(nameof(person)),
                LastName = person.LastName ?? throw new ArgumentNullException(nameof(person)),
                DateOfBirth = person.DateOfBirth,
                Income = income,
                Tax = tax,
                Block = block,
            };
            byte[] recordInByte = RecordToBytes(record);
            this.fileStream.Write(recordInByte, 0, recordInByte.Length);
            this.fileStream.Flush();
            return id;
        }

        /// <summary>
        /// Create record from the input parameters.
        /// </summary>
        /// <param name="id">Person's id.</param>
        /// <param name="person">Personal data.</param>
        /// <param name="income">Person's new income.</param>
        /// <param name="tax">Person's new tax.</param>
        /// <param name="block">Person's new living block.</param>
        public void EditRecord(int id, Person person, short income, decimal tax, char block)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Find persons by date of birth.
        /// </summary>
        /// <param name="strDateOfBirth">Person's date.</param>
        /// <returns>Array of person with same date of birth.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateofbirth(string strDateOfBirth)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Find persons by first name.
        /// </summary>
        /// <param name="firstName">Person's first name.</param>
        /// <returns>Array of person with same first name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Find persons by last name.
        /// </summary>
        /// <param name="lastName">Person's last name.</param>
        /// <returns>Array of person with same last name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get array of records.
        /// </summary>
        /// <returns>Collection of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var recordBuffer = new byte[RecordSize];
            this.fileStream.Position = 0;

            while (this.fileStream.Position != this.fileStream.Length)
            {
                this.fileStream.Read(recordBuffer, 0, RecordSize);
                var record = BytesToRecord(recordBuffer);

                if (!this.list.Exists(listRecord => listRecord.Id == record.Id))
                {
                    this.list.Add(record);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(this.list);
        }

        /// <summary>
        /// Get records count.
        /// </summary>
        /// <returns>Records count.</returns>
        public int GetStat()
        {
            return (int)(this.fileStream.Length / (long)RecordSize);
        }
    }
}
