using System.Collections.ObjectModel;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Class, that working with the records.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        /// <param name="validator">.</param>
        public FileCabinetMemoryService(IRecordValidator validator)
        {
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
            this.validator.ValidateParameters(person, income, tax, block);
            int id = 0;

            if (this.list.Count > 0)
            {
                id = this.list[this.list.Count - 1].Id;
            }

            var record = new FileCabinetRecord
            {
                Id = id + 1,
                FirstName = person.FirstName ?? throw new ArgumentNullException(nameof(person)),
                LastName = person.LastName ?? throw new ArgumentNullException(nameof(person)),
                DateOfBirth = person.DateOfBirth,
                Income = income,
                Tax = tax,
                Block = block,
            };

            this.list.Add(record);
            return record.Id;
        }

        /// <summary>
        /// Select records by input parameters.
        /// </summary>
        /// <param name="fields">Select these records fields.</param>
        /// <param name="parameters">Records parameters.</param>
        /// <returns>Selected records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> SelectRecord(bool[] fields, string[] parameters)
        {
            if (fields.Length == 0)
            {
                throw new ArgumentNullException(nameof(fields), "wrong fields: array doesn't contain value");
            }

            if (parameters.Length == 0)
            {
                throw new ArgumentNullException(nameof(parameters), "wrong parameters: array doesn't contain value");
            }

            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            bool noParameters = true;

            for (int i = 0; i < parameters.Length; i++)
            {
                string? parameter = parameters[i];

                if (!string.IsNullOrEmpty(parameter))
                {
                    records.AddRange(this.FindByParameter(parameter, i));
                    noParameters = false;
                }
            }

            records = records.GroupBy(record => record).Select(record => record.Key).ToList();

            if (noParameters)
            {
                records = this.list;
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>
        /// Insert record to the file cabinet service list.
        /// </summary>
        /// <param name="id">Person's id.</param>
        /// <param name="person">Personal data.</param>
        /// <param name="income">Person's new income.</param>
        /// <param name="tax">Person's new tax.</param>
        /// <param name="block">Person's new living block.</param>
        public void InsertRecord(int id, Person person, short income, decimal tax, char block)
        {
            this.validator.ValidateParameters(person, income, tax, block);
            int index = this.list.FindIndex(record => record.Id == id);
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

            if (index == -1)
            {
                this.list.Add(record);
            }
            else
            {
                this.list[index] = record;
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

            List<FileCabinetRecord> records = new List<FileCabinetRecord>();

            for (int i = 0; i < oldRecordParameters.Length; i++)
            {
                string? parameter = oldRecordParameters[i];
                if (!string.IsNullOrEmpty(parameter))
                {
                    records.AddRange(this.FindByParameter(parameter, i));
                }
            }

            records = records.GroupBy(record => record).Select(record => record.Key).ToList();
            Console.Write("Update record:");

            for (int i = 0; i < newRecordParameters.Length; i++)
            {
                string? parameter = newRecordParameters[i];
                if (!string.IsNullOrEmpty(parameter))
                {
                    foreach (FileCabinetRecord record in records)
                    {
                        this.ChangeRecordParameters(record, parameter, i);
                        Console.Write($" #{record.Id}");
                    }
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

            List<FileCabinetRecord>? records = fieldName switch
            {
                "id" => this.list.FindAll(record => record.Id == int.Parse(value, CultureInfo.InvariantCulture)),
                "firstname" => this.list.FindAll(record => record.FirstName == value),
                "lastname" => this.list.FindAll(record => record.LastName == value),
                "dateofbirth" => this.list.FindAll(record => record.DateOfBirth == DateTime.ParseExact(value, "M/dd/yyyy", CultureInfo.InvariantCulture)),
                "income" => this.list.FindAll(record => record.Income == short.Parse(value, CultureInfo.InvariantCulture)),
                "tax" => this.list.FindAll(record => record.Tax == decimal.Parse(value, CultureInfo.InvariantCulture)),
                "block" => this.list.FindAll(record => record.Block == value[0]),
                _ => null
            }

            ?? throw new ArgumentNullException(nameof(value), "wrong parameter: can't find record with this parameter in list");

            int i = 0;
            Console.Write("Deleted records:");
            while (i < records.Count)
            {
                this.list.Remove(records[i]);
                Console.Write($" #{records[i].Id}");
                i++;
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Make snapshot of the current list of records.
        /// </summary>
        /// <returns>Snapshot of records.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            FileCabinetServiceSnapshot serviceSnapshot = new FileCabinetServiceSnapshot(this.list.ToArray());
            return serviceSnapshot;
        }

        /// <summary>
        /// Restore records.
        /// </summary>
        /// <param name="snapshot">Records snapshot.</param>
        /// <returns>Counts of imported records.</returns>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            // Need to optimize
            IList<FileCabinetRecord> importRecords = snapshot.Records;
            int listSize = this.list.Count;
            int count = 0;
            int index = listSize;
            List<int> importIds = new List<int>();
            this.list.AddRange(importRecords);

            for (; index < this.list.Count; index++)
            {
                FileCabinetRecord record = this.list[index];
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
                    this.list.Remove(record);
                    index--;
                    continue;
                }
            }

            if (listSize > 0 && importRecords.Count > 0)
            {
                index = 0;

                for (int deleted = 0; index < (listSize - deleted); index++)
                {
                    FileCabinetRecord record = this.list[index];

                    if (importIds.Contains(record.Id))
                    {
                        this.list.Remove(record);
                        index--;
                        deleted++;
                    }
                }

                this.list.Sort(delegate(FileCabinetRecord firstRecord, FileCabinetRecord secondRecord) { return firstRecord.Id.CompareTo(secondRecord.Id); });
            }

            return count;
        }

        /// <summary>
        /// Get array of records.
        /// </summary>
        /// <returns>Collection of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return new ReadOnlyCollection<FileCabinetRecord>(this.list);
        }

        /// <summary>
        /// Get records count.
        /// </summary>
        /// <returns>Records count.</returns>
        public Tuple<int, int> GetStat()
        {
            return new Tuple<int, int>(this.list.Count, 0);
        }

        private void ChangeRecordParameters(FileCabinetRecord record, string value, int index)
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

        private List<FileCabinetRecord> FindByParameter(string value, int index)
        {
            List<FileCabinetRecord>? records = index switch
            {
                0 => this.list.FindAll(record => record.Id == int.Parse(value, CultureInfo.InvariantCulture)),
                1 => this.list.FindAll(record => record.FirstName == value),
                2 => this.list.FindAll(record => record.LastName == value),
                3 => this.list.FindAll(record => record.DateOfBirth == DateTime.ParseExact(value, "M/dd/yyyy", CultureInfo.InvariantCulture)),
                4 => this.list.FindAll(record => record.Income == short.Parse(value, CultureInfo.InvariantCulture)),
                5 => this.list.FindAll(record => record.Tax == short.Parse(value, CultureInfo.InvariantCulture)),
                6 => this.list.FindAll(record => record.Block == value[0]),
                _ => null
            }

            ?? throw new ArgumentNullException(nameof(value), "wrong parameter: can't find record with this parameter in list");
            return records;
        }
    }
}