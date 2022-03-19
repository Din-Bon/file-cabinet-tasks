using System.Text;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Class that generate records.
    /// </summary>
    public class FileCabinetRecordGenerator
    {
        private int id = 0;
        private int amount = 0;
        private readonly char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
        private static FileCabinetRecord[]? records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordGenerator"/> class.
        /// </summary>
        /// <param name="id">Start id.</param>
        /// <param name="amount">Amount of records.</param>
        public FileCabinetRecordGenerator(int id, int amount)
        {
            this.id = id;
            this.amount = amount;
        }

        /// <summary>
        /// Generate record with random values.
        /// </summary>
        public void GenerateRecords()
        {
            Random random = new Random();
            records = new FileCabinetRecord[amount];

            for (int i = 0; i < amount; i++)
            {
                string firstName = GenerateFirstName(random);
                string lastName = GenerateLastName(random);
                DateTime dateOfBirth = GenerateDateOfBirth(random);
                short income = GenerateIncome(random);
                decimal tax = GenerateTax(random);
                char block = GenerateBlock(random);
                PersonName name = new PersonName();
                name.FirstName = firstName;
                name.LastName = lastName;

                FileCabinetRecord record = new FileCabinetRecord()
                {
                    Id = this.id + i,
                    Name = name,
                    DateOfBirth = dateOfBirth,
                    Income = income,
                    Tax = tax,
                    Block = block,
                };

                records[i] = record;
            }
        }

        /// <summary>
        /// Save records data to csv file.
        /// </summary>
        /// <param name="stream">Stream that will contain records data.</param>
        public void ExportRecordsCsv(StreamWriter stream)
        {
            stream.WriteLine("Id,First Name,Last Name,Date of Birth,Income,Tax,Block");
            FileCabinetCsvExport export = new FileCabinetCsvExport(stream);
            
            if (records != null)
            {
                for (int i = 0; i < records.Length; i++)
                {
                    export.Write(records[i]);
                }
            }
        }

        /// <summary>
        /// Save records data to xml file.
        /// </summary>
        /// <param name="stream">Stream that will contain records data.</param>
        public void ExportRecordXml(StreamWriter stream)
        {
            FileCabinetXmlExport export = new FileCabinetXmlExport(stream);

            if (records != null)
            {
                export.Write(records);
            }
        }

        /// <summary>
        /// Generate first name.
        /// </summary>
        /// <param name="random">Random.</param>
        private string GenerateFirstName(Random random)
        {
            StringBuilder builder = new StringBuilder();
            int index = 0;
            int minLength = 2, maxLength = 60;
            int stringLength = random.Next(minLength, maxLength);

            for (int i = 0; i < stringLength; i++)
            {
                index = random.Next(0, 52);
                builder.Append(letters[index]);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Generate last name.
        /// </summary>
        /// <param name="random">Random.</param>
        private string GenerateLastName(Random random)
        {
            StringBuilder builder = new StringBuilder();
            int index = 0;
            int minLength = 2, maxLength = 60;
            int stringLength = random.Next(minLength, maxLength);

            for (int i = 0; i < stringLength; i++)
            {
                index = random.Next(0, 52);
                builder.Append(letters[index]);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Generate date of birth.
        /// </summary>
        /// <param name="random">Random.</param>
        private DateTime GenerateDateOfBirth(Random random)
        {
            DateTime minDate = new DateTime(1950, 01, 01);
            DateTime maxDate = DateTime.Today;
            System.TimeSpan times = maxDate.Subtract(minDate);
            DateTime dateOfBirth = DateTime.Today.AddDays(-random.Next(0, times.Days));
            return dateOfBirth;
        }

        /// <summary>
        /// Generate income.
        /// </summary>
        /// <param name="random">Random.</param>
        private short GenerateIncome(Random random)
        {
            short income = (short)random.Next(0, short.MaxValue);
            return income;
        }

        /// <summary>
        /// Generate tax.
        /// </summary>
        /// <param name="random">Random.</param>
        private decimal GenerateTax(Random random)
        {
            decimal tax = (decimal)random.NextSingle() * 100;
            tax = decimal.Round(tax, 2);
            return tax;
        }

        /// <summary>
        /// Generate block.
        /// </summary>
        /// <param name="random">Random.</param>
        private char GenerateBlock(Random random)
        {
            int index = random.Next(0, 26);
            char block = letters[index];
            return block;
        }
    }
}
