namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<string, List<FileCabinetRecord>>();

        public int CreateRecord(string? firstName, string? lastName, DateTime dateOfBirth, short income, decimal tax, char block)
        {
            ExceptionCheck(firstName, lastName, dateOfBirth, income, tax, block);

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Income = income,
                Tax = tax,
                Block = block,
            };

            this.list.Add(record);
            AddFirstNameDictionary(firstName, record);
            AddLastNameDictionary(lastName, record);
            return record.Id;
        }

        public void EditRecord(int id, string? firstName, string? lastName, DateTime dateOfBirth, short income, decimal tax, char block)
        {
            ExceptionCheck(firstName, lastName, dateOfBirth, income, tax, block);

            FileCabinetRecord oldRecord = this.list[id - 1];
            this.list[id - 1] = new FileCabinetRecord
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Income = income,
                Tax = tax,
                Block = block,
            };

            EditFirstNameDictionary(firstName, oldRecord, this.list[id - 1]);
            EditLastNameDictionary(lastName, oldRecord, this.list[id - 1]);
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            return this.firstNameDictionary[firstName.ToUpperInvariant()].ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            return this.lastNameDictionary[lastName.ToUpperInvariant()].ToArray();
        }

        public FileCabinetRecord[] FindByDateofbirth(string date)
        {
            List<FileCabinetRecord> suit = this.list.FindAll(id =>
            id.DateOfBirth.ToString("yyyy-MMM-dd", System.Globalization.CultureInfo.InvariantCulture) == date);
            return suit.ToArray();
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
            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException("wrong first name", nameof(firstName));
            }

            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException("wrong last name", nameof(lastName));
            }

            if (dateOfBirth < new DateTime(1950, 01, 01) || dateOfBirth > DateTime.Today)
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

            if (block < 65 || block > 90)
            {
                throw new ArgumentException("wrong block letter", nameof(block));
            }
        }

        private void AddFirstNameDictionary(string firstName, FileCabinetRecord record)
        {
            if (!this.firstNameDictionary.ContainsKey(firstName.ToUpperInvariant()))
            {
                this.firstNameDictionary.Add(firstName.ToUpperInvariant(), new List<FileCabinetRecord>() { record });
            }
            else
            {
                this.firstNameDictionary[firstName.ToUpperInvariant()].Add(record);
            }
        }

        private void AddLastNameDictionary(string lastName, FileCabinetRecord record)
        {
            if (!this.lastNameDictionary.ContainsKey(lastName.ToUpperInvariant()))
            {
                this.lastNameDictionary.Add(lastName.ToUpperInvariant(), new List<FileCabinetRecord>() { record });
            }
            else
            {
                this.lastNameDictionary[lastName.ToUpperInvariant()].Add(record);
            }
        }

        private void EditFirstNameDictionary(string firstName, FileCabinetRecord oldRecord, FileCabinetRecord newRecord)
        {
            if (!this.firstNameDictionary.ContainsKey(firstName.ToUpperInvariant()))
            {
                this.firstNameDictionary.Add(firstName.ToUpperInvariant(), new List<FileCabinetRecord>() { newRecord });
            }
            else
            {
                this.firstNameDictionary[firstName.ToUpperInvariant()].Add(newRecord);
            }

            if (this.firstNameDictionary[oldRecord.FirstName.ToUpperInvariant()].Count > 1)
            {
                this.firstNameDictionary[oldRecord.FirstName.ToUpperInvariant()].Remove(oldRecord);
            }
            else
            {
                this.firstNameDictionary.Remove(oldRecord.FirstName.ToUpperInvariant());
            }
        }

        private void EditLastNameDictionary(string lastName, FileCabinetRecord oldRecord, FileCabinetRecord newRecord)
        {
            if (!this.lastNameDictionary.ContainsKey(lastName.ToUpperInvariant()))
            {
                this.lastNameDictionary.Add(lastName.ToUpperInvariant(), new List<FileCabinetRecord>() { newRecord });
            }
            else
            {
                this.lastNameDictionary[lastName.ToUpperInvariant()].Add(newRecord);
            }

            if (this.lastNameDictionary[oldRecord.LastName.ToUpperInvariant()].Count > 1)
            {
                this.lastNameDictionary[oldRecord.LastName.ToUpperInvariant()].Remove(oldRecord);
            }
            else
            {
                this.lastNameDictionary.Remove(oldRecord.LastName.ToUpperInvariant());
            }
        }
    }
}