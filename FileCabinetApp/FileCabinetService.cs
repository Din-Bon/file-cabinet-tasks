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

            if (!this.firstNameDictionary.ContainsKey(firstName.ToUpperInvariant()))
            {
                this.firstNameDictionary.Add(firstName.ToUpperInvariant(), new List<FileCabinetRecord>() { record });
            }
            else
            {
                this.firstNameDictionary[firstName.ToUpperInvariant()].Add(record);
            }

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

            if (!this.firstNameDictionary.ContainsKey(firstName.ToUpperInvariant()))
            {
                this.firstNameDictionary.Add(firstName.ToUpperInvariant(), new List<FileCabinetRecord>() { this.list[id - 1] });
            }
            else
            {
                this.firstNameDictionary[firstName.ToUpperInvariant()].Add(this.list[id - 1]);
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

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            return this.firstNameDictionary[firstName.ToUpperInvariant()].ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            List<FileCabinetRecord> suit = this.list.FindAll(id => id.LastName.ToUpperInvariant() == lastName.ToUpperInvariant());
            return suit.ToArray();
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
    }
}