using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// Class, that working with the records.
    /// </summary>
    public class FileCabinetService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetService"/> class.
        /// </summary>
        /// <param name="validator">.</param>
        public FileCabinetService(IRecordValidator validator)
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

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = person.FirstName ?? throw new ArgumentNullException(nameof(person)),
                LastName = person.LastName ?? throw new ArgumentNullException(nameof(person)),
                DateOfBirth = person.DateOfBirth,
                Income = income,
                Tax = tax,
                Block = block,
            };

            this.list.Add(record);
            this.AddFirstNameDictionary(person.FirstName, record);
            this.AddLastNameDictionary(person.LastName, record);
            this.AddDateOfBirthDictionary(person.DateOfBirth, record);
            return record.Id;
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

            FileCabinetRecord oldRecord = this.list[id - 1];
            this.list[id - 1] = new FileCabinetRecord
            {
                Id = id,
                FirstName = person.FirstName ?? throw new ArgumentNullException(nameof(person)),
                LastName = person.LastName ?? throw new ArgumentNullException(nameof(person)),
                DateOfBirth = person.DateOfBirth,
                Income = income,
                Tax = tax,
                Block = block,
            };

            this.EditFirstNameDictionary(person.FirstName, oldRecord, this.list[id - 1]);
            this.EditLastNameDictionary(person.LastName, oldRecord, this.list[id - 1]);
            this.EditDateOfBirthDictionary(person.DateOfBirth, oldRecord, this.list[id - 1]);
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

            return new ReadOnlyCollection<FileCabinetRecord>(this.firstNameDictionary[firstName]);
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

            return new ReadOnlyCollection<FileCabinetRecord>(this.lastNameDictionary[lastName]);
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

            return new ReadOnlyCollection<FileCabinetRecord>(this.dateOfBirthDictionary[dateOfBirth]);
        }

        /// <summary>
        /// Make snapshot of the current list of records.
        /// </summary>
        /// <returns>Array of person with same date of birth.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            FileCabinetServiceSnapshot serviceSnapshot = new FileCabinetServiceSnapshot(this.list.ToArray());
            return serviceSnapshot;
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
        public int GetStat()
        {
            return this.list.Count;
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

            string oldFirstName = oldRecord.FirstName.ToUpperInvariant();
            if (this.firstNameDictionary[oldFirstName].Count > 1)
            {
                this.firstNameDictionary[oldFirstName].Remove(oldRecord);
            }
            else
            {
                this.firstNameDictionary.Remove(oldFirstName);
            }
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

            string oldLastName = oldRecord.LastName.ToUpperInvariant();
            if (this.lastNameDictionary[oldLastName].Count > 1)
            {
                this.lastNameDictionary[oldLastName].Remove(oldRecord);
            }
            else
            {
                this.lastNameDictionary.Remove(oldLastName);
            }
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

            DateTime oldDateOfBirth = oldRecord.DateOfBirth;
            if (this.dateOfBirthDictionary[oldDateOfBirth].Count > 1)
            {
                this.dateOfBirthDictionary[oldDateOfBirth].Remove(oldRecord);
            }
            else
            {
                this.dateOfBirthDictionary.Remove(oldDateOfBirth);
            }
        }
    }
}