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
        private static int id = 0;
        private const int MaxNameLength = 60;
        private const int RecordSize = (2 * sizeof(short)) + (6 * sizeof(int)) + MaxNameLength + MaxNameLength + sizeof(decimal) + sizeof(char);
        private readonly FileStream fileStream;

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
        /// <param name="person">Personal data.</param>
        /// <param name="income">Person's income.</param>
        /// <param name="tax">Person's tax.</param>
        /// <param name="block">Person's living block.</param>
        /// <returns>Byte array.</returns>
        public static byte[] RecordToBytes(Person person, short income, decimal tax, char block)
        {
            if (string.IsNullOrEmpty(person.FirstName) || string.IsNullOrEmpty(person.LastName))
            {
                throw new ArgumentNullException(nameof(person));
            }

            var bytes = new byte[RecordSize];
            using (var memoryStream = new MemoryStream(bytes))
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                id += 1;
                binaryWriter.Seek(2, SeekOrigin.Begin);
                binaryWriter.Write(id);

                var firstNameBytes = Encoding.ASCII.GetBytes(person.FirstName.ToCharArray());
                var firstNameBuffer = new byte[MaxNameLength];
                var lastNameBytes = Encoding.ASCII.GetBytes(person.LastName.ToCharArray());
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
                binaryWriter.Write(person.DateOfBirth.Year);
                binaryWriter.Write(person.DateOfBirth.Month);
                binaryWriter.Write(person.DateOfBirth.Day);
                binaryWriter.Write(income);
                binaryWriter.Write(tax);
                binaryWriter.Write(block);
            }

            return bytes;
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
            DefaultValidator validator = new DefaultValidator();
            validator.ValidateParameters(person, income, tax, block);
            byte[] record = RecordToBytes(person, income, tax, block);
            this.fileStream.Write(record, 0, record.Length);
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get records count.
        /// </summary>
        /// <returns>Records count.</returns>
        public int GetStat()
        {
            throw new NotImplementedException();
        }
    }
}
