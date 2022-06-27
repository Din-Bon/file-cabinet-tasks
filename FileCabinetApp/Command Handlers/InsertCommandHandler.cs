using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Class that handle insert command.
    /// </summary>
    internal class InsertCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service object.</param>
        public InsertCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        /// <summary>
        /// Execute insert command.
        /// </summary>
        /// <param name="request">Request.</param>
        public override void Handle(AppCommandRequest request)
        {
            var command = request.Command;
            var parameters = request.Parameters;

            if (command == "insert")
            {
                this.Insert(parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        /// <summary>
        /// Validates input values.
        /// </summary>
        /// <param name="person">Personal data.</param>
        /// <param name="income">Person's income.</param>
        /// <param name="tax">Person's tax.</param>
        /// <param name="block">Person's living block.</param>
        /// <returns>Is the data false.</returns>
        private static bool ValidateParameters(Person person, short income, decimal tax, char block)
        {
            int minNameLength = 2, maxNameLength = 60;
            DateTime minDateOfBirth = new DateTime(1950, 01, 01);
            DateTime maxDateOfBirth = DateTime.Today;
            int firstAlphabetLetter = 65, lastAlphabetLetter = 90;

            if (string.IsNullOrWhiteSpace(person.FirstName) || person.FirstName.Length < minNameLength || person.FirstName.Length > maxNameLength)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(person.LastName) || person.LastName.Length < minNameLength || person.LastName.Length > maxNameLength)
            {
                return true;
            }

            if (person.DateOfBirth < minDateOfBirth || person.DateOfBirth > maxDateOfBirth)
            {
                return true;
            }

            if (income < 0)
            {
                return true;
            }

            if (tax < 0 || tax > 100)
            {
                return true;
            }

            if (block < firstAlphabetLetter || block > lastAlphabetLetter)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Main manipulation with the input parameters.
        /// </summary>
        /// <param name="converter">Convert input string in the right type.</param>
        /// <param name="validator">Check input data for compliance with the validation rules.</param>
        /// <returns>Test results.</returns>
        private static T ReadInput<T>(string input, Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                if (input == null)
                {
                    throw new ArgumentNullException(nameof(input), "empty input");
                }

                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;
                var validationResult = validator(value);

                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        /// <summary>
        /// Checks id for compliance with the validation rules.
        /// </summary>
        /// <param name="id">Input data.</param>
        /// <returns>Test results.</returns>
        private static Tuple<bool, string> ValidateId(int id)
        {
            if (id <= 0)
            {
                return new Tuple<bool, string>(false, nameof(id));
            }

            return new Tuple<bool, string>(true, nameof(id));
        }

        /// <summary>
        /// Checks first name for compliance with the validation rules.
        /// </summary>
        /// <param name="firstName">Input data.</param>
        /// <returns>Test results.</returns>
        private static Tuple<bool, string> ValidateFirstName(string firstName)
        {
            int minNameLength = 2, maxNameLength = 60;

            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < minNameLength || firstName.Length > maxNameLength)
            {
                return new Tuple<bool, string>(false, nameof(firstName));
            }

            return new Tuple<bool, string>(true, nameof(firstName));
        }

        /// <summary>
        /// Checks last name for compliance with the validation rules.
        /// </summary>
        /// <param name="lastName">Input data.</param>
        /// <returns>Test results.</returns>
        private static Tuple<bool, string> ValidateLastName(string lastName)
        {
            int minNameLength = 2, maxNameLength = 60;

            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < minNameLength || lastName.Length > maxNameLength)
            {
                return new Tuple<bool, string>(false, nameof(lastName));
            }

            return new Tuple<bool, string>(true, nameof(lastName));
        }

        /// <summary>
        /// Checks date of birth for compliance with the validation rules.
        /// </summary>
        /// <param name="dateOfBirth">Input data.</param>
        /// <returns>Test results.</returns>
        private static Tuple<bool, string> ValidateDateOfBirth(DateTime dateOfBirth)
        {
            DateTime minDateOfBirth = new DateTime(1950, 01, 01);
            DateTime maxDateOfBirth = DateTime.Today;

            if (dateOfBirth < minDateOfBirth || dateOfBirth > maxDateOfBirth)
            {
                return new Tuple<bool, string>(false, nameof(dateOfBirth));
            }

            return new Tuple<bool, string>(true, nameof(dateOfBirth));
        }

        /// <summary>
        /// Checks income for compliance with the validation rules.
        /// </summary>
        /// <param name="income">Input data.</param>
        /// <returns>Test results.</returns>
        private static Tuple<bool, string> ValidateIncome(short income)
        {
            if (income < 0)
            {
                return new Tuple<bool, string>(false, nameof(income));
            }

            return new Tuple<bool, string>(true, nameof(income));
        }

        /// <summary>
        /// Checks tax for compliance with the validation rules.
        /// </summary>
        /// <param name="tax">Input data.</param>
        /// <returns>Test results.</returns>
        private static Tuple<bool, string> ValidateTax(decimal tax)
        {
            int minTax = 0, maxTax = 100;

            if (tax < minTax || tax > maxTax)
            {
                return new Tuple<bool, string>(false, nameof(tax));
            }

            return new Tuple<bool, string>(true, nameof(tax));
        }

        /// <summary>
        /// Checks block letter for compliance with the validation rules.
        /// </summary>
        /// <param name="block">Input data.</param>
        /// <returns>Test results.</returns>
        private static Tuple<bool, string> ValidateBlock(char block)
        {
            int firstAlphabetLetter = 65, lastAlphabetLetter = 90;

            if (block < firstAlphabetLetter || block > lastAlphabetLetter)
            {
                return new Tuple<bool, string>(false, nameof(block));
            }

            return new Tuple<bool, string>(true, nameof(block));
        }

        /// <summary>
        /// Return tuple of parameters.
        /// </summary>
        /// <param name="name">String parameter(name).</param>
        /// <returns>Tuple of parameters.</returns>
        private static Tuple<bool, string, string> StringConverter(string name)
        {
            return new Tuple<bool, string, string>(true, nameof(name), name);
        }

        /// <summary>
        /// Return tuple of parameters.
        /// </summary>
        /// <param name="strDateOfBirth">String parameter(date of birth).</param>
        /// <returns>Tuple of parameters.</returns>
        private static Tuple<bool, string, DateTime> DateTimeConverter(string strDateOfBirth)
        {
            DateTime dateOfBirth;

            if (!DateTime.TryParseExact(strDateOfBirth, "M/dd/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateOfBirth))
            {
                return new Tuple<bool, string, DateTime>(false, nameof(strDateOfBirth), DateTime.Today);
            }

            return new Tuple<bool, string, DateTime>(true, nameof(strDateOfBirth), dateOfBirth);
        }

        /// <summary>
        /// Return tuple of parameters.
        /// </summary>
        /// <param name="strIncome">String parameter(income).</param>
        /// <returns>Tuple of parameters.</returns>
        private static Tuple<bool, string, short> ShortConverter(string strIncome)
        {
            short income;

            if (!short.TryParse(strIncome, out income))
            {
                return new Tuple<bool, string, short>(false, nameof(strIncome), 0);
            }

            return new Tuple<bool, string, short>(true, nameof(strIncome), income);
        }

        /// <summary>
        /// Return tuple of parameters.
        /// </summary>
        /// <param name="strTax">String parameter(tax).</param>
        /// <returns>Tuple of parameters.</returns>
        private static Tuple<bool, string, decimal> DecimalConverter(string strTax)
        {
            decimal tax;

            if (!decimal.TryParse(strTax, out tax))
            {
                return new Tuple<bool, string, decimal>(false, nameof(strTax), 0);
            }

            return new Tuple<bool, string, decimal>(true, nameof(strTax), tax);
        }

        /// <summary>
        /// Return tuple of parameters.
        /// </summary>
        /// <param name="strBlock">String parameter(block).</param>
        /// <returns>Tuple of parameters.</returns>
        private static Tuple<bool, string, char> CharConverter(string strBlock)
        {
            char block;

            if (!char.TryParse(strBlock, out block))
            {
                return new Tuple<bool, string, char>(false, nameof(strBlock), '\0');
            }

            return new Tuple<bool, string, char>(true, nameof(strBlock), block);
        }

        /// <summary>
        /// Return tuple of parameters.
        /// </summary>
        /// <param name="strid">String parameter(id).</param>
        /// <returns>Tuple of parameters.</returns>
        private static Tuple<bool, string, int> IntConverter(string strid)
        {
            int id;

            if (!int.TryParse(strid, out id))
            {
                return new Tuple<bool, string, int>(false, nameof(strid), '\0');
            }

            return new Tuple<bool, string, int>(true, nameof(strid), id);
        }

        /// <summary>
        /// Insert record to the list.
        /// </summary>
        /// <param name="parameters">Input parameters.</param>
        private void Insert(string parameters)
        {
            string[] args = parameters.Split(" values ");
            string[] fieldNames = args[0].Trim('(', ')').Split(", ", ',');
            string[] paramValues = args[1].Trim('(', ')').Split(", ", ',');
            string[] names = { "id", "firstname", "lastname", "dateofbirth", "income", "tax", "block" };
            string[] values = new string[7];

            if (fieldNames.Length != paramValues.Length || paramValues.Length > 7)
            {
                throw new ArgumentException("wrong parameters", parameters);
            }

            for (int i = 0; i < fieldNames.Length; i++)
            {
                int index = Array.IndexOf(names, fieldNames[i]);
                values[index] = paramValues[i].Trim('\'');
            }

            int id = ReadInput<int>(values[0], IntConverter, ValidateId);
            var firstName = ReadInput<string>(values[1], StringConverter, ValidateFirstName);
            var lastName = ReadInput<string>(values[2], StringConverter, ValidateLastName);
            var dateOfBirth = ReadInput<DateTime>(values[3], DateTimeConverter, ValidateDateOfBirth);
            var income = ReadInput<short>(values[4], ShortConverter, ValidateIncome);
            decimal tax = ReadInput<decimal>(values[5], DecimalConverter, ValidateTax);
            char block = ReadInput<char>(values[6], CharConverter, ValidateBlock);
            Person person = new Person() { FirstName = firstName, LastName = lastName, DateOfBirth = dateOfBirth };
            this.fileCabinetService.InsertRecord(id, person, income, tax, block);
        }
    }
}
