namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        public int CreateRecord(string? firstName, string? lastName, DateTime dateOfBirth, short income, decimal tax, char block)
        {
            ExceptionCheck(firstName, lastName, dateOfBirth, income, tax, block);

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName)),
                LastName = lastName ?? throw new ArgumentNullException(nameof(lastName)),
                DateOfBirth = dateOfBirth,
                Income = income,
                Tax = tax,
                Block = block,
            };

            this.list.Add(record);
            this.AddFirstNameDictionary(firstName, record);
            this.AddLastNameDictionary(lastName, record);
            this.AddDateOfBirthDictionary(dateOfBirth, record);
            return record.Id;
        }

        public void EditRecord(int id, string? firstName, string? lastName, DateTime dateOfBirth, short income, decimal tax, char block)
        {
            ExceptionCheck(firstName, lastName, dateOfBirth, income, tax, block);

            FileCabinetRecord oldRecord = this.list[id - 1];
            this.list[id - 1] = new FileCabinetRecord
            {
                Id = id,
                FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName)),
                LastName = lastName ?? throw new ArgumentNullException(nameof(lastName)),
                DateOfBirth = dateOfBirth,
                Income = income,
                Tax = tax,
                Block = block,
            };

            this.EditFirstNameDictionary(firstName, oldRecord, this.list[id - 1]);
            this.EditLastNameDictionary(lastName, oldRecord, this.list[id - 1]);
            this.EditDateOfBirthDictionary(dateOfBirth, oldRecord, this.list[id - 1]);
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            firstName = firstName.ToUpperInvariant();
            if (!this.firstNameDictionary.ContainsKey(firstName))
            {
                throw new ArgumentException("wrong first name", nameof(firstName));
            }

            return this.firstNameDictionary[firstName].ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            lastName = lastName.ToUpperInvariant();
            if (!this.lastNameDictionary.ContainsKey(lastName))
            {
                throw new ArgumentException("wrong last name", nameof(lastName));
            }

            return this.lastNameDictionary[lastName].ToArray();
        }

        public FileCabinetRecord[] FindByDateofbirth(string date)
        {
            var dateOfBirth = DateTime.ParseExact(date, "yyyy-MMM-dd", System.Globalization.CultureInfo.InvariantCulture);

            if (!this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                throw new ArgumentException("wrong date of birth", nameof(date));
            }

            return this.dateOfBirthDictionary[dateOfBirth].ToArray();
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }

        private static void ExceptionCheck(string? firstName, string? lastName, DateTime dateOfBirth, short income, decimal tax, char block)
        {
            int minNameLength = 2, maxNameLength = 60;
            DateTime minDateOfBirth = new DateTime(1950, 01, 01);
            DateTime maxDateOfBirth = DateTime.Today;
            int firstAlphabetLetter = 65, lastAlphabetLetter = 90;

            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < minNameLength || firstName.Length > maxNameLength)
            {
                throw new ArgumentException("wrong first name", nameof(firstName));
            }

            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < minNameLength || lastName.Length > maxNameLength)
            {
                throw new ArgumentException("wrong last name", nameof(lastName));
            }

            if (dateOfBirth < minDateOfBirth || dateOfBirth > maxDateOfBirth)
            {
                throw new ArgumentException("wrong date of birth", nameof(dateOfBirth));
            }

            if (income < 0)
            {
                throw new ArgumentException("wrong income", nameof(income));
            }

            if (tax < 0 || tax > 100)
            {
                throw new ArgumentException("wrong tax", nameof(tax));
            }

            if (block < firstAlphabetLetter || block > lastAlphabetLetter)
            {
                throw new ArgumentException("wrong block letter", nameof(block));
            }
        }

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