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
        private const int RecordSize = sizeof(short) + (6 * sizeof(int)) + MaxNameLength + MaxNameLength + sizeof(decimal) + sizeof(char) + 16;
        private readonly FileStream fileStream;
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<long>> firstNameDictionary = new Dictionary<string, List<long>>();
        private readonly Dictionary<string, List<long>> lastNameDictionary = new Dictionary<string, List<long>>();
        private readonly Dictionary<DateTime, List<long>> dateOfBirthDictionary = new Dictionary<DateTime, List<long>>();
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">Stream with data.</param>
        /// <param name="validator">.</param>
        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator)
        {
            this.fileStream = fileStream;
            this.validator = validator;
            this.FillDictionaries();
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
            long position = this.fileStream.Position;
            this.validator.ValidateParameters(person, income, tax, block);
            int id = this.FindLastId() + 1;

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

            byte[] recordInByte = RecordToBytes(record, 0);
            this.fileStream.Write(recordInByte, 0, recordInByte.Length);
            this.fileStream.Flush();
            this.AddFirstNameDictionary(record.FirstName, position);
            this.AddLastNameDictionary(record.LastName, position);
            this.AddDateOfBirthDictionary(record.DateOfBirth, position);
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
            this.validator.ValidateParameters(person, income, tax, block);
            long position = this.FindPositionById(id);

            if (position == -1)
            {
                Console.WriteLine($"record with #{id} doesn't exists");
            }
            else
            {
                this.fileStream.Position = position;
                byte[] oldRecordInByte = new byte[RecordSize];
                this.fileStream.Read(oldRecordInByte, 0, RecordSize);
                FileCabinetRecord oldRecord = BytesToRecord(oldRecordInByte);
                long itemToDelete = this.firstNameDictionary[oldRecord.FirstName.ToUpperInvariant()].
                    Where(pos => pos == position).Select(record => record).First();
                this.fileStream.Position = (id - 1) * RecordSize;

                FileCabinetRecord newRecord = new FileCabinetRecord
                {
                    Id = id,
                    FirstName = person.FirstName ?? throw new ArgumentNullException(nameof(person)),
                    LastName = person.LastName ?? throw new ArgumentNullException(nameof(person)),
                    DateOfBirth = person.DateOfBirth,
                    Income = income,
                    Tax = tax,
                    Block = block,
                };

                byte[] newRecordInByte = RecordToBytes(newRecord, 0);
                this.fileStream.Write(newRecordInByte, 0, newRecordInByte.Length);
                this.fileStream.Flush();
                this.EditFirstNameDictionary(person.FirstName, itemToDelete, position);
                this.EditLastNameDictionary(person.LastName, itemToDelete, position);
                this.EditDateOfBirthDictionary(person.DateOfBirth, itemToDelete, position);
            }
        }

        /// <summary>
        /// Remove record by id.
        /// </summary>
        /// <param name="id">Person's id.</param>
        public void RemoveRecord(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("wrong id(<1)", nameof(id));
            }

            int length = (int)(this.fileStream.Length / (long)RecordSize);
            byte[] recordInByte = new byte[RecordSize];
            byte[] reservedBytes = new byte[16];
            byte isDeleted = 0;

            for (int i = 0; i < length; i++)
            {
                this.fileStream.Position = RecordSize * i;
                this.fileStream.Read(recordInByte, 0, RecordSize);
                isDeleted = recordInByte[13];
                FileCabinetRecord? record = BytesToRecord(recordInByte);
                if (record.Id == id)
                {
                    if (isDeleted == 0)
                    {
                        isDeleted = 1;
                        this.fileStream.Position = RecordSize * i;
                        long position = this.fileStream.Position;
                        byte[] deleteRecord = RecordToBytes(record, isDeleted);
                        this.RemoveInFirstNameDictionary(record.FirstName, position);
                        this.RemoveInLastNameDictionary(record.LastName, position);
                        this.RemoveInDateOfBirthDictionary(record.DateOfBirth, position);
                        this.fileStream.Write(deleteRecord, 0, deleteRecord.Length);
                        this.fileStream.Flush();
                        Console.WriteLine($"Record #{id} is removed.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"Record #{id} doesn't exists.");
                        isDeleted = 0;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Purge records.
        /// </summary>
        /// <returns>Count of purged records.</returns>
        public int Purge()
        {
            var records = this.GetRecords();
            int count = (int)(this.fileStream.Length / (long)RecordSize) - records.Count;
            byte[] recordBuffer = new byte[RecordSize];
            this.fileStream.SetLength(0);
            this.fileStream.Flush();

            for (int i = 0; i < records.Count; i++)
            {
                recordBuffer = RecordToBytes(records[i], 0);
                this.fileStream.Write(recordBuffer);
            }

            this.fileStream.Flush();
            return count;
        }

        /// <summary>
        /// Find persons by first name.
        /// </summary>
        /// <param name="firstName">Person's first name.</param>
        /// <returns>Array of person with same first name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            firstName = firstName.ToUpperInvariant();

            if (!this.firstNameDictionary.ContainsKey(firstName))
            {
                throw new ArgumentException("wrong first name", nameof(firstName));
            }

            List<long> positionsList = this.firstNameDictionary[firstName];
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();

            foreach (var position in positionsList)
            {
                this.fileStream.Position = position;
                byte[] recordInByte = new byte[RecordSize];
                this.fileStream.Read(recordInByte, 0, RecordSize);
                FileCabinetRecord record = BytesToRecord(recordInByte);
                records.Add(record);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>
        /// Find persons by last name.
        /// </summary>
        /// <param name="lastName">Person's last name.</param>
        /// <returns>Array of person with same last name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            lastName = lastName.ToUpperInvariant();

            if (!this.lastNameDictionary.ContainsKey(lastName))
            {
                throw new ArgumentException("wrong last name", nameof(lastName));
            }

            List<long> positionsList = this.lastNameDictionary[lastName];
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();

            foreach (var position in positionsList)
            {
                this.fileStream.Position = position;
                byte[] recordInByte = new byte[RecordSize];
                this.fileStream.Read(recordInByte, 0, RecordSize);
                FileCabinetRecord record = BytesToRecord(recordInByte);
                records.Add(record);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>
        /// Find persons by date of birth.
        /// </summary>
        /// <param name="strDateOfBirth">Person's date.</param>
        /// <returns>Array of person with same date of birth.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateofbirth(string strDateOfBirth)
        {
            var dateOfBirth = DateTime.ParseExact(strDateOfBirth, "yyyy-MMM-dd", System.Globalization.CultureInfo.InvariantCulture);

            if (!this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                throw new ArgumentException("wrong date of birth", nameof(strDateOfBirth));
            }

            List<long> positionsList = this.dateOfBirthDictionary[dateOfBirth];
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();

            foreach (var position in positionsList)
            {
                this.fileStream.Position = position;
                byte[] recordInByte = new byte[RecordSize];
                this.fileStream.Read(recordInByte, 0, RecordSize);
                FileCabinetRecord record = BytesToRecord(recordInByte);
                records.Add(record);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>
        /// Make snapshot of the current list of records.
        /// </summary>
        /// <returns>Snapshot of records.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            var listOfRecord = this.GetRecords();
            int fileLength = listOfRecord.Count;
            FileCabinetRecord[] records = new FileCabinetRecord[fileLength];
            listOfRecord.CopyTo(records, 0);
            FileCabinetServiceSnapshot serviceSnapshot = new FileCabinetServiceSnapshot(records);
            return serviceSnapshot;
        }

        /// <summary>
        /// Restore records.
        /// </summary>
        /// <param name="snapshot">Records snapshot.</param>
        /// <returns>Counts of imported records.</returns>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            IList<FileCabinetRecord> importRecords = snapshot.Records;
            List<FileCabinetRecord> streamList = new List<FileCabinetRecord>();
            List<int> importIds = new List<int>();
            List<int> removedIds = new List<int>();
            int listSize = (int)(this.fileStream.Length / (long)RecordSize);
            int count = 0;
            int index = listSize;
            byte[] oldRecordInByte = new byte[RecordSize];
            this.fileStream.Position = 0;

            for (int i = 0; i < listSize; i++)
            {
                this.fileStream.Read(oldRecordInByte, 0, RecordSize);
                FileCabinetRecord oldRecord = BytesToRecord(oldRecordInByte);

                if (oldRecordInByte[13] == 1)
                {
                    removedIds.Add(oldRecord.Id);
                }

                streamList.Add(oldRecord);
            }

            streamList.AddRange(importRecords);

            for (; index < streamList.Count; index++)
            {
                FileCabinetRecord record = streamList[index];
                Person person = new Person
                {
                    FirstName = record.FirstName,
                    LastName = record.LastName,
                    DateOfBirth = record.DateOfBirth,
                };

                try
                {
                    this.validator.ValidateParameters(person, record.Income, record.Tax, record.Block);
                    importIds.Add(record.Id);
                    count++;
                }
                catch (ArgumentException exception)
                {
                    Console.WriteLine($"{exception.Message}. Record id - {record.Id}.");
                    streamList.Remove(record);
                    index--;
                    continue;
                }
            }

            if (listSize > 0 && importRecords.Count > 0)
            {
                index = 0;

                for (int deleted = 0; index < (listSize - deleted); index++)
                {
                    FileCabinetRecord record = streamList[index];

                    if (importIds.Contains(record.Id))
                    {
                        streamList.Remove(record);
                        index--;
                        deleted++;
                    }
                }

                streamList.Sort(delegate(FileCabinetRecord firstRecord, FileCabinetRecord secondRecord) { return firstRecord.Id.CompareTo(secondRecord.Id); });
            }

            this.fileStream.Position = 0;
            byte isDeleted = 0;
            this.firstNameDictionary.Clear();
            this.lastNameDictionary.Clear();
            this.dateOfBirthDictionary.Clear();

            for (int i = 0; i < streamList.Count; i++)
            {
                if (removedIds.Contains(streamList[i].Id) && !importIds.Contains(streamList[i].Id))
                {
                    isDeleted = 1;
                }

                long position = this.fileStream.Position;
                this.AddFirstNameDictionary(streamList[i].FirstName, position);
                this.AddLastNameDictionary(streamList[i].LastName, position);
                this.AddDateOfBirthDictionary(streamList[i].DateOfBirth, position);
                byte[] record = RecordToBytes(streamList[i], isDeleted);
                this.fileStream.Write(record, 0, record.Length);
                isDeleted = 0;
            }

            this.fileStream.Flush();
            return count;
        }

        /// <summary>
        /// Get array of records.
        /// </summary>
        /// <returns>Collection of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var recordBuffer = new byte[RecordSize];
            this.fileStream.Position = 0;
            this.list.Clear();

            while (this.fileStream.Position != this.fileStream.Length)
            {
                this.fileStream.Read(recordBuffer, 0, RecordSize);
                if (recordBuffer[13] == 0)
                {
                    var record = BytesToRecord(recordBuffer);
                    this.list.Add(record);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(this.list);
        }

        /// <summary>
        /// Get records count.
        /// </summary>
        /// <returns>Records count.</returns>
        public Tuple<int, int> GetStat()
        {
            int length = (int)(this.fileStream.Length / (long)RecordSize);
            int count = this.GetRecords().Count;
            int removed = length - count;
            return new Tuple<int, int>(count, removed);
        }

        /// <summary>
        /// Convert record parameters to bytes.
        /// </summary>
        /// <param name="record">Record with data.</param>
        /// <param name="isDeleted">Deleted value.</param>
        /// <returns>Byte array.</returns>
        private static byte[] RecordToBytes(FileCabinetRecord record, byte isDeleted)
        {
            if (string.IsNullOrEmpty(record.FirstName) || string.IsNullOrEmpty(record.LastName))
            {
                throw new ArgumentNullException(nameof(record));
            }

            var bytes = new byte[RecordSize];
            using (var memoryStream = new MemoryStream(bytes))
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                var reservedBytes = new byte[16];
                reservedBytes[13] = isDeleted;
                binaryWriter.Write(reservedBytes);
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

        /// <summary>
        /// Find last id in the file.
        /// </summary>
        /// <returns>Last id.</returns>
        private int FindLastId()
        {
            int length = (int)(this.fileStream.Length / (long)RecordSize);
            int currentId = 0, lastId = 0;
            byte[] recordInByte = new byte[RecordSize];
            byte[] reservedBytes = new byte[16];
            byte isDeleted = 0;

            for (int i = 0; i < length; i++)
            {
                using (var memoryStream = new MemoryStream(recordInByte))
                using (var binaryReader = new BinaryReader(memoryStream))
                {
                    this.fileStream.Position = RecordSize * i;
                    this.fileStream.Read(recordInByte, 0, RecordSize);
                    reservedBytes = binaryReader.ReadBytes(16);
                    isDeleted = reservedBytes[13];
                    currentId = binaryReader.ReadInt32();
                    if (currentId > lastId)
                    {
                        lastId = currentId;
                    }
                }
            }

            return lastId;
        }

        /// <summary>
        /// Find position by id.
        /// </summary>
        /// <param name="id">Records id.</param>
        /// <returns>Records position.</returns>
        private long FindPositionById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("wrong id(<1)", nameof(id));
            }

            int length = (int)(this.fileStream.Length / (long)RecordSize);
            long position = -1;
            int currentId = 0;
            byte[] recordInByte = new byte[RecordSize];
            byte[] reservedBytes = new byte[16];
            byte isDeleted = 0;

            for (int i = 0; i < length; i++)
            {
                using (var memoryStream = new MemoryStream(recordInByte))
                using (var binaryReader = new BinaryReader(memoryStream))
                {
                    this.fileStream.Position = RecordSize * i;
                    this.fileStream.Read(recordInByte, 0, RecordSize);
                    reservedBytes = binaryReader.ReadBytes(16);
                    isDeleted = reservedBytes[13];
                    currentId = binaryReader.ReadInt32();
                    if (currentId == id && isDeleted == 0)
                    {
                        position = RecordSize * i;
                    }
                }
            }

            return position;
        }

        private void FillDictionaries()
        {
            var recordBuffer = new byte[RecordSize];
            this.fileStream.Position = 0;

            while (this.fileStream.Position != this.fileStream.Length)
            {
                long position = this.fileStream.Position;
                this.fileStream.Read(recordBuffer, 0, RecordSize);

                if (recordBuffer[13] == 0)
                {
                    var record = BytesToRecord(recordBuffer);

                    Person person = new Person
                    {
                        FirstName = record.FirstName,
                        LastName = record.LastName,
                        DateOfBirth = record.DateOfBirth,
                    };

                    this.AddFirstNameDictionary(record.FirstName, position);
                    this.AddLastNameDictionary(record.LastName, position);
                    this.AddDateOfBirthDictionary(record.DateOfBirth, position);
                }
            }
        }

        /// <summary>
        /// Add first name as a key and the record as a value in dictionary.
        /// </summary>
        /// <param name="firstName">Person's first name.</param>
        /// <param name="record">Record of a person with that first name.</param>
        private void AddFirstNameDictionary(string firstName, long record)
        {
            firstName = firstName.ToUpperInvariant();

            if (!this.firstNameDictionary.ContainsKey(firstName))
            {
                this.firstNameDictionary.Add(firstName, new List<long>() { record });
            }
            else
            {
                this.firstNameDictionary[firstName].Add(record);
            }
        }

        /// <summary>
        /// Add last name as a key and the record as a value in dictionary.
        /// </summary>
        /// <param name="lastName">Person's last name.</param>
        /// <param name="record">Record of a person with that last name.</param>
        private void AddLastNameDictionary(string lastName, long record)
        {
            lastName = lastName.ToUpperInvariant();

            if (!this.lastNameDictionary.ContainsKey(lastName))
            {
                this.lastNameDictionary.Add(lastName, new List<long>() { record });
            }
            else
            {
                this.lastNameDictionary[lastName].Add(record);
            }
        }

        /// <summary>
        /// Add date of birth as a key and the record as a value in dictionary.
        /// </summary>
        /// <param name="dateOfBirth">Person's date of birth.</param>
        /// <param name="record">Record of a person with that date of birth.</param>
        private void AddDateOfBirthDictionary(DateTime dateOfBirth, long record)
        {
            if (!this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                this.dateOfBirthDictionary.Add(dateOfBirth, new List<long>() { record });
            }
            else
            {
                this.dateOfBirthDictionary[dateOfBirth].Add(record);
            }
        }

        /// <summary>
        /// Edit value with that first name in dictionary.
        /// </summary>
        /// <param name="firstName">Person's first name.</param>
        /// <param name="oldRecord">Old record of a person.</param>
        /// <param name="newRecord">New record of a person with that first name.</param>
        private void EditFirstNameDictionary(string firstName, long oldRecord, long newRecord)
        {
            firstName = firstName.ToUpperInvariant();

            if (!this.firstNameDictionary.ContainsKey(firstName))
            {
                this.firstNameDictionary.Add(firstName, new List<long>() { newRecord });
            }
            else
            {
                this.firstNameDictionary[firstName].Add(newRecord);
            }

            this.RemoveInFirstNameDictionary(firstName, oldRecord);
        }

        /// <summary>
        /// Edit value with that last name in dictionary.
        /// </summary>
        /// <param name="lastName">Person's last name.</param>
        /// <param name="oldRecord">Old record of a person.</param>
        /// <param name="newRecord">New record of a person with that last name.</param>
        private void EditLastNameDictionary(string lastName, long oldRecord, long newRecord)
        {
            lastName = lastName.ToUpperInvariant();

            if (!this.lastNameDictionary.ContainsKey(lastName))
            {
                this.lastNameDictionary.Add(lastName, new List<long>() { newRecord });
            }
            else
            {
                this.lastNameDictionary[lastName].Add(newRecord);
            }

            this.RemoveInLastNameDictionary(lastName, oldRecord);
        }

        /// <summary>
        /// Edit value with that date of birth in dictionary.
        /// </summary>
        /// <param name="dateOfBirth">Person's date of birth.</param>
        /// <param name="oldRecord">Old record of a person.</param>
        /// <param name="newRecord">New record of a person with that date of birth.</param>
        private void EditDateOfBirthDictionary(DateTime dateOfBirth, long oldRecord, long newRecord)
        {
            if (!this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                this.dateOfBirthDictionary.Add(dateOfBirth, new List<long>() { newRecord });
            }
            else
            {
                this.dateOfBirthDictionary[dateOfBirth].Add(newRecord);
            }

            this.RemoveInDateOfBirthDictionary(dateOfBirth, oldRecord);
        }

        /// <summary>
        /// Remove record from the firstNameDictionary.
        /// </summary>
        /// <param name="record">Record.</param>
        private void RemoveInFirstNameDictionary(string firstName, long record)
        {
            firstName = firstName.ToUpperInvariant();

            if (this.firstNameDictionary[firstName].Count > 1)
            {
                this.firstNameDictionary[firstName].Remove(record);
            }
            else
            {
                this.firstNameDictionary.Remove(firstName);
            }
        }

        /// <summary>
        /// Remove record from the lastNameDictionary.
        /// </summary>
        /// <param name="record">Record.</param>
        private void RemoveInLastNameDictionary(string lastName, long record)
        {
            lastName = lastName.ToUpperInvariant();

            if (this.lastNameDictionary[lastName].Count > 1)
            {
                this.lastNameDictionary[lastName].Remove(record);
            }
            else
            {
                this.lastNameDictionary.Remove(lastName);
            }
        }

        /// <summary>
        /// Remove record from the dateOfBirthDictionary.
        /// </summary>
        /// <param name="record">Record.</param>
        private void RemoveInDateOfBirthDictionary(DateTime dateOfBirth, long record)
        {
            if (this.dateOfBirthDictionary[dateOfBirth].Count > 1)
            {
                this.dateOfBirthDictionary[dateOfBirth].Remove(record);
            }
            else
            {
                this.dateOfBirthDictionary.Remove(dateOfBirth);
            }
        }
    }
}
