using System.Collections.ObjectModel;
using System.Globalization;
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
            return id;
        }

        /// <summary>
        /// Select records by input parameters.
        /// </summary>
        /// <param name="fields">Select these records fields.</param>
        /// <param name="parameters">Records parameters.</param>
        public void SelectRecord(bool[] fields, string[] parameters)
        {
            if (fields.Length == 0)
            {
                throw new ArgumentNullException(nameof(fields), "wrong fields: array doesn't contain value");
            }

            if (parameters.Length == 0)
            {
                throw new ArgumentNullException(nameof(parameters), "wrong parameters: array doesn't contain value");
            }

            ReadOnlyCollection<FileCabinetRecord> allRecords = this.GetRecords();
            List<FileCabinetRecord> outputRecords = new List<FileCabinetRecord>();
            bool noParameters = true;

            for (int i = 0; i < allRecords.Count; i++)
            {
                if (CompareRecordFields(allRecords[i], parameters))
                {
                    outputRecords.Add(allRecords[i]);
                    noParameters = false;
                }
            }

            if (noParameters)
            {
                outputRecords.AddRange(allRecords);
            }

            SelectPrinter(fields, outputRecords);
        }

        /// <summary>
        /// Create record from the input parameters.
        /// </summary>
        /// <param name="id">Person's id.</param>
        /// <param name="person">Personal data.</param>
        /// <param name="income">Person's new income.</param>
        /// <param name="tax">Person's new tax.</param>
        /// <param name="block">Person's new living block.</param>
        public void InsertRecord(int id, Person person, short income, decimal tax, char block)
        {
            this.validator.ValidateParameters(person, income, tax, block);
            long position = this.FindPositionById(id);
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

            if (position == -1)
            {
                this.fileStream.Position = this.fileStream.Length;
                this.fileStream.Write(recordInByte, 0, recordInByte.Length);
                this.fileStream.Flush();
            }
            else
            {
                this.fileStream.Position = position;
                this.fileStream.Write(recordInByte, 0, recordInByte.Length);
                this.fileStream.Flush();
            }
        }

        /// <summary>
        /// Update record by input parameters.
        /// </summary>
        /// <param name="oldRecordParameters">Old records data.</param>
        /// <param name="newRecordParameters">New records data.</param>
        public void UpdateRecords(string[] oldRecordParameters, string[] newRecordParameters)
        {
            if (oldRecordParameters.Length == 0)
            {
                throw new ArgumentNullException(nameof(oldRecordParameters), "wrong data: empty old records data array");
            }

            if (newRecordParameters.Length == 0)
            {
                throw new ArgumentNullException(nameof(newRecordParameters), "wrong data: empty new records data array");
            }

            int length = (int)(this.fileStream.Length / (long)RecordSize);
            byte[] recordInByte = new byte[RecordSize];
            byte isDeleted;

            for (int i = 0; i < length; i++)
            {
                long position = RecordSize * i;
                this.fileStream.Position = position;
                this.fileStream.Read(recordInByte, 0, RecordSize);
                isDeleted = recordInByte[13];
                FileCabinetRecord? record = BytesToRecord(recordInByte);
                this.fileStream.Position = position;

                if (isDeleted == 0 && CompareRecordFields(record, oldRecordParameters))
                {
                    // Update record parameters.
                    this.UpdateOneRecord(record, newRecordParameters);
                    byte[] newRecordInByte = RecordToBytes(record, 0);
                    this.fileStream.Write(newRecordInByte, 0, newRecordInByte.Length);
                    this.fileStream.Flush();
                }
            }
        }

        /// <summary>
        /// Delete record by parameter name.
        /// </summary>
        /// <param name="fieldName">Record parameter.</param>
        /// <param name="value">Parameter value.</param>
        public void DeleteRecord(string fieldName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(value, "can not delete record: value is empty");
            }

            int length = (int)(this.fileStream.Length / (long)RecordSize);
            Console.Write("Deleted records:");

            for (int i = 0; i < length; i++)
            {
                this.fileStream.Position = RecordSize * i;
                long position = this.fileStream.Position;
                int deletedId = this.ParseAndDelete(fieldName, value, position);

                if (deletedId != 0)
                {
                    Console.Write($" #{deletedId}");
                }
            }

            Console.WriteLine();
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

            for (int i = 0; i < streamList.Count; i++)
            {
                if (removedIds.Contains(streamList[i].Id) && !importIds.Contains(streamList[i].Id))
                {
                    isDeleted = 1;
                }

                long position = this.fileStream.Position;
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

        private static void SelectPrinter(bool[] fields, List<FileCabinetRecord> records)
        {
            Func<FileCabinetRecord, string>[] printRecordsPart = new Func<FileCabinetRecord, string>[]
            {
                record => record.Id.ToString(CultureInfo.InvariantCulture),
                record => record.FirstName,
                record => record.LastName,
                record => record.DateString,
                record => record.Income.ToString(CultureInfo.InvariantCulture),
                record => record.Tax.ToString(CultureInfo.InvariantCulture),
                record => record.StringBlock,
            };

            int[] lengths = PrintHeader(fields, records);
            int[] numericTypesIndexes = { 0, 4, 5 };

            foreach (var record in records)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    bool leftAlignment = fields[i];
                    bool rightAlignment = fields[i] && numericTypesIndexes.Contains(i);

                    if (rightAlignment)
                    {
                        string field = printRecordsPart[i](record).PadLeft(lengths[i]);
                        Console.Write(" | " + field);
                    }
                    else if (leftAlignment)
                    {
                        string field = printRecordsPart[i](record).PadRight(lengths[i]);
                        Console.Write(" | " + field);
                    }
                }

                Console.Write(" |" + Environment.NewLine);
            }

            PrintDividingLine(lengths, fields);
        }

        /// <summary>
        /// Print head of the table for select command.
        /// </summary>
        /// <param name="fields">Array of bool value, index mean records field.</param>
        /// <param name="records">Help to find the optimal column size.</param>
        private static int[] PrintHeader(bool[] fields, List<FileCabinetRecord> records)
        {
            int[] lengths = new int[]
            {
                records.Max(rec => rec.Id.ToString(CultureInfo.InvariantCulture).Length),
                records.Max(rec => rec.FirstName.Length),
                records.Max(rec => rec.LastName.Length),
                records[0].DateString.Length,
                records.Max(rec => rec.Income.ToString(CultureInfo.InvariantCulture).Length),
                records.Max(rec => rec.Tax.ToString(CultureInfo.InvariantCulture).Length),
                records[0].StringBlock.Length,
            };

            string[] names = { "Id", "Firstname", "Lastname", "DateOfBirth", "Income", "Tax", "Block" };

            for (int i = 0; i < lengths.Length; i++)
            {
                lengths[i] = lengths[i] > names[i].Length ? lengths[i] : names[i].Length;
            }

            PrintDividingLine(lengths, fields);

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i])
                {
                    int space = (lengths[i] - names[i].Length) / 2;
                    int leftPadding = space + names[i].Length;
                    int rightPadding = leftPadding + space;
                    rightPadding = lengths[i] - names[i].Length == space * 2 ? rightPadding : rightPadding + 1;
                    Console.Write(" | " + names[i].PadLeft(leftPadding).PadRight(rightPadding));
                }
            }

            Console.Write(" |" + Environment.NewLine);
            PrintDividingLine(lengths, fields);
            return lengths;
        }

        /// <summary>
        /// Print dividing line between rows in tabular output.
        /// </summary>
        /// <param name="lengths">Column widths.</param>
        private static void PrintDividingLine(int[] lengths, bool[] fields)
        {
            Console.Write(" +");

            for (int i = 0; i < lengths.Length; i++)
            {
                if (fields[i])
                {
                    int separatorLength = 3;
                    int length = lengths[i] + separatorLength;
                    Console.Write("+".PadLeft(length, '-'));
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Check if current record match the conditions.
        /// </summary>
        /// <param name="record">Current record from file.</param>
        /// <param name="oldRecordParameters">Check if record has such parameters.</param>
        /// <returns>Record has oldRecordParameters or not?.</returns>
        private static bool CompareRecordFields(FileCabinetRecord record, string[] oldRecordParameters)
        {
            List<bool> checkList = new List<bool>();
            for (int i = 0; i < oldRecordParameters.Length; i++)
            {
                string parameter = oldRecordParameters[i];

                if (!string.IsNullOrEmpty(parameter))
                {
                    checkList.Add(CompareOneField(record, parameter, i));
                }
            }

            if (!checkList.Contains(false))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Helps to check if a record has such a parameter.
        /// </summary>
        /// <param name="record">Current record.</param>
        /// <param name="value">Field value.</param>
        /// <param name="index">By index we can find out what field we check now.</param>
        /// <returns>Record has value or not?.</returns>
        private static bool CompareOneField(FileCabinetRecord record, string value, int index)
        {
            bool check = index switch
            {
                0 => record.Id == int.Parse(value, CultureInfo.InvariantCulture),
                1 => record.FirstName == value,
                2 => record.LastName == value,
                3 => record.DateOfBirth == DateTime.ParseExact(value, "M/dd/yyyy", CultureInfo.InvariantCulture),
                4 => record.Income == short.Parse(value, CultureInfo.InvariantCulture),
                5 => record.Tax == decimal.Parse(value, CultureInfo.InvariantCulture),
                6 => record.Block == value[0],
                _ => false,
            };

            return check;
        }

        /// <summary>
        /// Help to update record one by one.
        /// </summary>
        /// <param name="record">Record that we update.</param>
        /// <param name="newRecordParameters">New parameters.</param>
        private void UpdateOneRecord(FileCabinetRecord record, string[] newRecordParameters)
        {
            for (int i = 0; i < newRecordParameters.Length; i++)
            {
                string parameter = newRecordParameters[i];

                if (!string.IsNullOrEmpty(parameter))
                {
                    this.UpdateOneParameter(record, parameter, i);
                }
            }
        }

        /// <summary>
        /// Update parameter by its position in the array.
        /// </summary>
        /// <param name="record">Record for update.</param>
        /// <param name="value">New record value.</param>
        /// <param name="index">Index that show whar field we update.</param>
        private void UpdateOneParameter(FileCabinetRecord record, string value, int index)
        {
            switch (index)
            {
                case 1:
                    record.FirstName = value;
                    break;
                case 2:
                    record.LastName = value;
                    break;
                case 3:
                    record.DateOfBirth = DateTime.ParseExact(value, "M/dd/yyyy", CultureInfo.InvariantCulture);
                    break;
                case 4:
                    record.Income = short.Parse(value, CultureInfo.InvariantCulture);
                    break;
                case 5:
                    record.Tax = short.Parse(value, CultureInfo.InvariantCulture);
                    break;
                case 6:
                    record.Block = value[0];
                    break;
                default:
                    break;
            }

            Person personalData = new Person
            {
                FirstName = record.FirstName,
                LastName = record.LastName,
                DateOfBirth = record.DateOfBirth,
            };
            this.validator.ValidateParameters(personalData, record.Income, record.Tax, record.Block);
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

        /// <summary>
        /// Parse record in file and delete if input value
        /// match records value.
        /// </summary>
        /// <param name="field">Field name.</param>
        /// <param name="value">Field parameter.</param>
        /// <param name="position">Record position in file.</param>
        /// <returns>Deleted record id.</returns>
        private int ParseAndDelete(string field, string value, long position)
        {
            byte[] recordInByte = new byte[RecordSize];
            this.fileStream.Position = position;
            this.fileStream.Read(recordInByte, 0, RecordSize);
            byte isDeleted = recordInByte[13];
            FileCabinetRecord? record = BytesToRecord(recordInByte);
            int deletedId = 0;
            this.fileStream.Position = position;

            switch (field)
            {
                case "id":
                    if (record.Id == int.Parse(value, CultureInfo.InvariantCulture))
                    {
                        this.DeleteInFile(record, position);
                        deletedId = record.Id;
                    }

                    break;
                case "firstname":
                    if (record.FirstName == value)
                    {
                        this.DeleteInFile(record, position);
                        deletedId = record.Id;
                    }

                    break;
                case "lastname":
                    if (record.LastName == value)
                    {
                        this.DeleteInFile(record, position);
                        deletedId = record.Id;
                    }

                    break;
                case "dateofbirth":
                    if (record.DateOfBirth == DateTime.ParseExact(value, "M/dd/yyyy", CultureInfo.InvariantCulture))
                    {
                        this.DeleteInFile(record, position);
                        deletedId = record.Id;
                    }

                    break;
                case "income":
                    if (record.Income == short.Parse(value, CultureInfo.InvariantCulture))
                    {
                        this.DeleteInFile(record, position);
                        deletedId = record.Id;
                    }

                    break;
                case "tax":
                    if (record.Tax == decimal.Parse(value, CultureInfo.InvariantCulture))
                    {
                        this.DeleteInFile(record, position);
                        deletedId = record.Id;
                    }

                    break;
                case "block":
                    if (record.Block == value[0])
                    {
                        this.DeleteInFile(record, position);
                        deletedId = record.Id;
                    }

                    break;
                default:
                    Console.WriteLine($"Wrong field: record doesn't contain {field}");
                    break;
            }

            return deletedId;
        }

        /// <summary>
        /// Delete record by its position.
        /// </summary>
        /// <param name="record">Record.</param>
        /// <param name="position">Position in file.</param>
        private void DeleteInFile(FileCabinetRecord record, long position)
        {
            byte isDeleted = 1;
            byte[] deleteRecord = RecordToBytes(record, isDeleted);
            this.fileStream.Write(deleteRecord, 0, deleteRecord.Length);
            this.fileStream.Flush();
        }
    }
}
