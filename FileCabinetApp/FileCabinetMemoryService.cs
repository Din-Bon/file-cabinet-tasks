using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// Class, that working with the records.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();
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
            this.AddToDictionaries(record);
            return record.Id;
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
                this.AddToDictionaries(record);
            }
            else
            {
                FileCabinetRecord oldRecord = this.list[index];
                this.list[index] = record;
                this.EditInDictionaries(oldRecord, this.list[index]);
            }
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
            int index = this.list.FindIndex(record => record.Id == id);

            if (index == -1)
            {
                Console.WriteLine($"record with #{id} doesn't exists");
            }
            else
            {
                FileCabinetRecord oldRecord = this.list[index];
                this.list[index] = new FileCabinetRecord
                {
                    Id = id,
                    FirstName = person.FirstName ?? throw new ArgumentNullException(nameof(person)),
                    LastName = person.LastName ?? throw new ArgumentNullException(nameof(person)),
                    DateOfBirth = person.DateOfBirth,
                    Income = income,
                    Tax = tax,
                    Block = block,
                };

                this.EditInDictionaries(oldRecord, this.list[index]);
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

            FileCabinetRecord? record = this.list.Find(record => record.Id == id);

            if (record != null)
            {
                this.list.Remove(record);
                this.RemoveInDictionaries(record);
                Console.WriteLine($"Record #{id} is removed");
            }
            else
            {
                Console.WriteLine($"Record #{id} doesn't exists");
            }
        }

        /// <summary>
        /// Find persons by first name.
        /// </summary>
        /// <param name="firstName">Person's first name.</param>
        /// <returns>Array of person with same first name.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            firstName = firstName.ToUpperInvariant();

            if (!this.firstNameDictionary.ContainsKey(firstName))
            {
                throw new ArgumentException("wrong first name", nameof(firstName));
            }

            ReadOnlyCollection<FileCabinetRecord> collection = new ReadOnlyCollection<FileCabinetRecord>(this.firstNameDictionary[firstName]);
            IEnumerable<FileCabinetRecord> records = new MemoryEnumerable(collection);
            return records;
        }

        /// <summary>
        /// Find persons by last name.
        /// </summary>
        /// <param name="lastName">Person's last name.</param>
        /// <returns>Array of person with same last name.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            lastName = lastName.ToUpperInvariant();

            if (!this.lastNameDictionary.ContainsKey(lastName))
            {
                throw new ArgumentException("wrong last name", nameof(lastName));
            }

            ReadOnlyCollection<FileCabinetRecord> collection = new ReadOnlyCollection<FileCabinetRecord>(this.lastNameDictionary[lastName]);
            IEnumerable<FileCabinetRecord> records = new MemoryEnumerable(collection);
            return records;
        }

        /// <summary>
        /// Find persons by date of birth.
        /// </summary>
        /// <param name="strDateOfBirth">Person's date.</param>
        /// <returns>Array of person with same date of birth.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateofbirth(string strDateOfBirth)
        {
            var dateOfBirth = DateTime.ParseExact(strDateOfBirth, "yyyy-MMM-dd", System.Globalization.CultureInfo.InvariantCulture);

            if (!this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                throw new ArgumentException("wrong date of birth", nameof(strDateOfBirth));
            }

            ReadOnlyCollection<FileCabinetRecord> collection = new ReadOnlyCollection<FileCabinetRecord>(this.dateOfBirthDictionary[dateOfBirth]);
            IEnumerable<FileCabinetRecord> records = new MemoryEnumerable(collection);
            return records;
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
                    this.AddToDictionaries(record);
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
                        this.RemoveInDictionaries(record);
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

        /// <summary>
        /// Add first name as a key and the record as a value in dictionary.
        /// </summary>
        /// <param name="firstName">Person's first name.</param>
        /// <param name="record">Record of a person with that first name.</param>
        private void AddFirstNameDictionary(string firstName, FileCabinetRecord record)
        {
            firstName = firstName.ToUpperInvariant();

            if (!this.firstNameDictionary.ContainsKey(firstName))
            {
                this.firstNameDictionary.Add(firstName, new List<FileCabinetRecord>() { record });
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
        private void AddLastNameDictionary(string lastName, FileCabinetRecord record)
        {
            lastName = lastName.ToUpperInvariant();

            if (!this.lastNameDictionary.ContainsKey(lastName))
            {
                this.lastNameDictionary.Add(lastName, new List<FileCabinetRecord>() { record });
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
        private void AddDateOfBirthDictionary(DateTime dateOfBirth, FileCabinetRecord record)
        {
            if (!this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                this.dateOfBirthDictionary.Add(dateOfBirth, new List<FileCabinetRecord>() { record });
            }
            else
            {
                this.dateOfBirthDictionary[dateOfBirth].Add(record);
            }
        }

        /// <summary>
        /// Add in all dictionaries.
        /// </summary>
        /// <param name="record">Record of a person..</param>
        private void AddToDictionaries(FileCabinetRecord record)
        {
            this.AddFirstNameDictionary(record.FirstName, record);
            this.AddLastNameDictionary(record.LastName, record);
            this.AddDateOfBirthDictionary(record.DateOfBirth, record);
        }

        /// <summary>
        /// Edit value with that first name in dictionary.
        /// </summary>
        /// <param name="firstName">Person's first name.</param>
        /// <param name="oldRecord">Old record of a person.</param>
        /// <param name="newRecord">New record of a person with that first name.</param>
        private void EditFirstNameDictionary(string firstName, FileCabinetRecord oldRecord, FileCabinetRecord newRecord)
        {
            firstName = firstName.ToUpperInvariant();

            if (!this.firstNameDictionary.ContainsKey(firstName))
            {
                this.firstNameDictionary.Add(firstName, new List<FileCabinetRecord>() { newRecord });
            }
            else
            {
                this.firstNameDictionary[firstName].Add(newRecord);
            }

            this.RemoveInFirstNameDictionary(oldRecord);
        }

        /// <summary>
        /// Edit value with that last name in dictionary.
        /// </summary>
        /// <param name="lastName">Person's last name.</param>
        /// <param name="oldRecord">Old record of a person.</param>
        /// <param name="newRecord">New record of a person with that last name.</param>
        private void EditLastNameDictionary(string lastName, FileCabinetRecord oldRecord, FileCabinetRecord newRecord)
        {
            lastName = lastName.ToUpperInvariant();

            if (!this.lastNameDictionary.ContainsKey(lastName))
            {
                this.lastNameDictionary.Add(lastName, new List<FileCabinetRecord>() { newRecord });
            }
            else
            {
                this.lastNameDictionary[lastName].Add(newRecord);
            }

            this.RemoveInLastNameDictionary(oldRecord);
        }

        /// <summary>
        /// Edit value with that date of birth in dictionary.
        /// </summary>
        /// <param name="dateOfBirth">Person's date of birth.</param>
        /// <param name="oldRecord">Old record of a person.</param>
        /// <param name="newRecord">New record of a person with that date of birth.</param>
        private void EditDateOfBirthDictionary(DateTime dateOfBirth, FileCabinetRecord oldRecord, FileCabinetRecord newRecord)
        {
            if (!this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                this.dateOfBirthDictionary.Add(dateOfBirth, new List<FileCabinetRecord>() { newRecord });
            }
            else
            {
                this.dateOfBirthDictionary[dateOfBirth].Add(newRecord);
            }

            this.RemoveInDateOfBirthDictionary(oldRecord);
        }

        /// <summary>
        /// Edit value everywhere.
        /// </summary>
        /// <param name="oldRecord">Old record of a person.</param>
        /// <param name="newRecord">New record of a person.</param>
        private void EditInDictionaries(FileCabinetRecord oldRecord, FileCabinetRecord newRecord)
        {
            this.EditFirstNameDictionary(newRecord.FirstName, oldRecord, newRecord);
            this.EditLastNameDictionary(newRecord.LastName, oldRecord, newRecord);
            this.EditDateOfBirthDictionary(newRecord.DateOfBirth, oldRecord, newRecord);
        }

        /// <summary>
        /// Remove record from the firstNameDictionary.
        /// </summary>
        /// <param name="record">Record.</param>
        private void RemoveInFirstNameDictionary(FileCabinetRecord record)
        {
            string firstName = record.FirstName.ToUpperInvariant();

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
        private void RemoveInLastNameDictionary(FileCabinetRecord record)
        {
            string lastName = record.LastName.ToUpperInvariant();

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
        private void RemoveInDateOfBirthDictionary(FileCabinetRecord record)
        {
            DateTime dateOfBirth = record.DateOfBirth;

            if (this.dateOfBirthDictionary[dateOfBirth].Count > 1)
            {
                this.dateOfBirthDictionary[dateOfBirth].Remove(record);
            }
            else
            {
                this.dateOfBirthDictionary.Remove(dateOfBirth);
            }
        }

        /// <summary>
        /// Remove record from everywhere.
        /// </summary>
        /// <param name="record">Record.</param>
        private void RemoveInDictionaries(FileCabinetRecord record)
        {
            this.RemoveInFirstNameDictionary(record);
            this.RemoveInLastNameDictionary(record);
            this.RemoveInDateOfBirthDictionary(record);
        }
    }
}