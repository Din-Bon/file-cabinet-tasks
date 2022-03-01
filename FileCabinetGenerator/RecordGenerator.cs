using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Class that generate records.
    /// </summary>
    public class RecordGenerator
    {
        private int id = 0;
        private int amount = 0;
        private readonly char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
        private static Record[]? records;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordGenerator"/> class.
        /// </summary>
        /// <param name="id">Start id.</param>
        /// <param name="amount">Amount of records.</param>
        public RecordGenerator(int id, int amount)
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
            records = new Record[amount];

            for (int i = 0; i < amount; i++)
            {
                string firstName = GenerateFirstName(random);
                string lastName = GenerateLastName(random);
                DateTime dateOfBirth = GenerateDateOfBirth(random);
                short income = GenerateIncome(random);
                decimal tax = GenerateTax(random);
                char block = GenerateBlock(random);

                Record record = new Record()
                {
                    Id = this.id + i,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    Income = income,
                    Tax = tax,
                    Block = block,
                };

                records[i] = record;
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

            for (int i = 0; i < 60; i++)
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

            for (int i = 0; i < 60; i++)
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

        /// <summary>
        /// Structure describing a person.
        /// </summary>
        public struct Record
        {
            /// <summary>
            /// Gets or sets persons id.
            /// </summary>
            /// <value>Persons id.</value>
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets persons first name.
            /// </summary>
            /// <value>Persons first name.</value>
            public string FirstName { get; set; }

            /// <summary>
            /// Gets or sets persons last name.
            /// </summary>
            /// <value>Persons last name.</value>
            public string LastName { get; set; }

            /// <summary>
            /// Gets or sets persons date of birth.
            /// </summary>
            /// <value>Persons date of birth.</value>
            public DateTime DateOfBirth { get; set; }

            /// <summary>
            /// Gets or sets persons income.
            /// </summary>
            /// <value>Persons income.</value>
            public short Income { get; set; }

            /// <summary>
            /// Gets or sets persons tax.
            /// </summary>
            /// <value>Persons tax.</value>
            public decimal Tax { get; set; }

            /// <summary>
            /// Gets or sets persons living block.
            /// </summary>
            /// <value>Persons living block.</value>
            public char Block { get; set; }

            /// <summary>
            /// Build string from records parameters.
            /// </summary>
            /// <returns>Record string representation.</returns>
            public override string ToString()
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"#{this.Id}, ");
                builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"{this.FirstName}, ");
                builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"{this.LastName}, ");
                builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"{this.DateOfBirth.ToString("yyyy-MMM-dd", System.Globalization.CultureInfo.InvariantCulture)}, ");
                builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"{this.Income}, ");
                builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"{this.Tax}, ");
                builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"{this.Block}");
                return builder.ToString();
            }
        }
    }
}
