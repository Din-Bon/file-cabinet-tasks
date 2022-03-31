﻿using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

namespace FileCabinetApp
{
    /// <summary>
    /// Class, that process request.
    /// </summary>
    internal class CommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("export", Export),
            new Tuple<string, Action<string>>("import", Import),
            new Tuple<string, Action<string>>("remove", Remove),
            new Tuple<string, Action<string>>("purge", Purge),
        };

        /// <summary>
        /// Passes the request further if
        /// the next handler is specified.
        /// </summary>
        /// <param name="request">Request.</param>
        public override void Handle(AppCommandRequest request)
        {
            var command = request.Command;
            var parameters = request.Parameters;
#pragma warning disable CA1309 // Использование порядкового сравнения строк
            var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
#pragma warning restore CA1309 // Использование порядкового сравнения строк

            if (index >= 0)
            {
                commands[index].Item2(parameters);
            }
            else
            {
                PrintMissedCommandInfo(command);
            }
        }

        /// <summary>
        /// Create record.
        /// </summary>
        /// <param name="parameters">Input parameters.</param>
        private static void Create(string parameters)
        {
            bool check = true;

            do
            {
                Console.Write("First name: ");
                var firstName = ReadInput<string>(StringConverter, ValidateFirstName);
                Console.Write("Last name: ");
                var lastName = ReadInput<string>(StringConverter, ValidateLastName);
                Console.Write("Date of birth: ");
                var dateOfBirth = ReadInput<DateTime>(DateTimeConverter, ValidateDateOfBirth);
                Console.Write("Income: ");
                var income = ReadInput<short>(ShortConverter, ValidateIncome);
                Console.Write("Tax: ");
                decimal tax = ReadInput<decimal>(DecimalConverter, ValidateTax);
                Console.Write("Block: ");
                char block = ReadInput<char>(CharConverter, ValidateBlock);
                Person person = new Person() { FirstName = firstName, LastName = lastName, DateOfBirth = dateOfBirth };
                check = ValidateParameters(person, income, tax, block);

                if (!check)
                {
                    Console.WriteLine($"Record #{Program.FileCabinetService.CreateRecord(person, income, tax, block)} is created.");
                }
            }
            while (check);
        }

        /// <summary>
        /// Modify existing records.
        /// </summary>
        /// <param name="parameters">Id of an existing record.</param>
        private static void Edit(string parameters)
        {
            if (string.IsNullOrEmpty(parameters))
            {
                throw new ArgumentNullException(nameof(parameters), "empty id");
            }

            int id = Convert.ToInt32(parameters, CultureInfo.CurrentCulture);

            if (id < 0)
            {
                throw new ArgumentException("id value less than 0", nameof(parameters));
            }

            Console.Write("First name: ");
            var firstName = ReadInput<string>(StringConverter, ValidateFirstName);
            Console.Write("Last name: ");
            var lastName = ReadInput<string>(StringConverter, ValidateLastName);
            Console.Write("Date of birth: ");
            var dateOfBirth = ReadInput<DateTime>(DateTimeConverter, ValidateDateOfBirth);
            Console.Write("Income: ");
            var income = ReadInput<short>(ShortConverter, ValidateIncome);
            Console.Write("Tax: ");
            decimal tax = ReadInput<decimal>(DecimalConverter, ValidateTax);
            Console.Write("Block: ");
            char block = ReadInput<char>(CharConverter, ValidateBlock);
            Person person = new Person() { FirstName = firstName, LastName = lastName, DateOfBirth = dateOfBirth };

            if (!ValidateParameters(person, income, tax, block))
            {
                Program.FileCabinetService.EditRecord(id, person, income, tax, block);
                Console.WriteLine($"record #{id} is updated.");
            }
            else
            {
                Console.WriteLine($"wrong parameters{Environment.NewLine}record #{id} isn't updated.");
            }
        }

        /// <summary>
        /// Shows list of records.
        /// </summary>
        /// <param name="parameters">Input parameters.</param>
        private static void List(string parameters)
        {
            ReadOnlyCollection<FileCabinetRecord> records = Program.FileCabinetService.GetRecords();
            PrintRecords(records);
        }

        /// <summary>
        /// Shows count of existing records.
        /// </summary>
        /// <param name="parameters">Input prameters.</param>
        private static void Stat(string parameters)
        {
            var recordsCount = Program.FileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount.Item1} record(s). Removed - {recordsCount.Item2}");
        }

        /// <summary>
        /// Find record.
        /// </summary>
        /// <param name="parameters">Array from property and value.</param>
        private static void Find(string parameters)
        {
            if (string.IsNullOrEmpty(parameters))
            {
                throw new ArgumentNullException(nameof(parameters), "wrong find command");
            }

            var commands = parameters.Split(' ', 2);
            string property = commands[0].ToUpperInvariant();
            string parameter = commands[1].Trim('"');

            switch (property)
            {
                case "FIRSTNAME":
                    PrintRecords(Program.FileCabinetService.FindByFirstName(parameter));
                    break;
                case "LASTNAME":
                    PrintRecords(Program.FileCabinetService.FindByLastName(parameter));
                    break;
                case "DATEOFBIRTH":
                    PrintRecords(Program.FileCabinetService.FindByDateofbirth(parameter));
                    break;
            }
        }

        /// <summary>
        /// Import records from file.
        /// </summary>
        /// <param name="parameters">Array from property and value.</param>
        private static void Import(string parameters)
        {
            if (string.IsNullOrEmpty(parameters))
            {
                throw new ArgumentNullException(nameof(parameters), "wrong find command");
            }

            var commands = parameters.Split(' ', 2);
            string fileName = commands[1];
            string path = ParseImport(fileName);
            string property = commands[0].ToUpperInvariant();
            string[] formats = { "CSV", "XML" };

            if (!string.IsNullOrEmpty(path) && formats.Contains(property))
            {
                StreamReader reader = new StreamReader(path);
                FileCabinetServiceSnapshot serviceSnapshot = Program.FileCabinetService.MakeSnapshot();

                if (property == "CSV")
                {
                    serviceSnapshot.LoadFromCsv(reader);
                }
                else if (property == "XML")
                {
                    serviceSnapshot.LoadFromXml(reader);
                }

                int count = Program.FileCabinetService.Restore(serviceSnapshot);
                reader.Close();
                Console.WriteLine($"{count} records were imported from {path}.");
            }
        }

        /// <summary>
        /// Export records in file.
        /// </summary>
        /// <param name="parameters">Array from property and value.</param>
        private static void Export(string parameters)
        {
            if (string.IsNullOrEmpty(parameters))
            {
                throw new ArgumentNullException(nameof(parameters), "wrong find command");
            }

            var commands = parameters.Split(' ', 2);
            string fileName = commands[1];
            string path = ParseExport(fileName);
            string property = commands[0].ToUpperInvariant();
            string[] formats = { "CSV", "XML" };

            if (!string.IsNullOrEmpty(path) && formats.Contains(property))
            {
                StreamWriter writer = new StreamWriter(path);
                FileCabinetServiceSnapshot serviceSnapshot = Program.FileCabinetService.MakeSnapshot();

                if (property == "CSV")
                {
                    serviceSnapshot.SaveToCsv(writer);
                }
                else if (property == "XML")
                {
                    serviceSnapshot.SaveToXml(writer);
                }

                writer.Flush();
                writer.Close();
                Console.WriteLine($"All records are exported to file {path}.");
            }
        }

        /// <summary>
        /// Parse string for export command.
        /// </summary>
        /// <param name="input">Array from property and value.</param>
        private static string ParseExport(string input)
        {
            string fileName = input;
            string path = fileName;
            char rewrite = 'Y';

            if (path.Contains('\\', StringComparison.InvariantCulture))
            {
                path = input.Substring(0, input.LastIndexOf('\\'));
                fileName = input.Substring(input.LastIndexOf('\\'), input.Length - input.LastIndexOf('\\') - 1);
                if (!Directory.Exists(path))
                {
                    Console.WriteLine($"Export failed: can't open file {path}.");
                    return string.Empty;
                }
            }

            if (File.Exists(input))
            {
                Console.Write($"File is exist - rewrite {input}? [Y/n] ");
                rewrite = (char)Console.Read();
            }

            if (rewrite == 'Y')
            {
                return input;
            }

            return string.Empty;
        }

        /// <summary>
        /// Parse string for import command.
        /// </summary>
        /// <param name="input">Array from property and value.</param>
        private static string ParseImport(string input)
        {
            string fileName = input;
            string path = fileName;

            if (path.Contains('\\', StringComparison.InvariantCulture))
            {
                path = input.Substring(0, input.LastIndexOf('\\'));
                fileName = input.Substring(input.LastIndexOf('\\'), input.Length - input.LastIndexOf('\\') - 1);
                if (!Directory.Exists(path))
                {
                    Console.WriteLine($"Export failed: can't open file {path}.");
                    return string.Empty;
                }
            }

            if (!File.Exists(input))
            {
                Console.WriteLine($"File doesn't exist");
                return string.Empty;
            }

            return input;
        }

        /// <summary>
        /// Remove record by id.
        /// </summary>
        /// <param name="parameters">String parameter(id).</param>
        private static void Remove(string parameters)
        {
            int id = 0;

            if (string.IsNullOrEmpty(parameters) || !int.TryParse(parameters, out id))
            {
                throw new ArgumentNullException(nameof(parameters), "empty id");
            }

            if (id <= 0)
            {
                throw new ArgumentException("wrond id (<1)", nameof(parameters));
            }

            Program.FileCabinetService.RemoveRecord(id);
        }

        /// <summary>
        /// Purge records.
        /// </summary>
        /// <param name="parameters">Input parameters.</param>
        private static void Purge(string parameters)
        {
            int count = Program.FileCabinetService.Purge();
            var length = Program.FileCabinetService.GetStat().Item1 + count;
            Console.WriteLine($"Data file processing is completed: {count} of {length} records were purged.");
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

            if (!DateTime.TryParseExact(strDateOfBirth, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth))
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

            if (Program.ValidationMode == "CUSTOM")
            {
                minTax = 10;
            }

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
        /// Main manipulation with the input parameters.
        /// </summary>
        /// <param name="converter">Convert input string in the right type.</param>
        /// <param name="validator">Check input data for compliance with the validation rules.</param>
        /// <returns>Test results.</returns>
        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;
                var input = Console.ReadLine();

                if (input == null)
                {
                    throw new ArgumentNullException("empty input", nameof(input));
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
        /// Print records data on the console.
        /// </summary>
        /// <param name="records">Array of the records.</param>
        private static void PrintRecords(ReadOnlyCollection<FileCabinetRecord> records)
        {
            for (int i = 0; i < records.Count; i++)
            {
                Console.WriteLine(records[i].ToString());
            }
        }

        /// <summary>
        /// Print lists of the command.
        /// </summary>
        /// <param name="parameters">Command name.</param>
        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
#pragma warning disable CA1309 // Использование порядкового сравнения строк
                var index = Array.FindIndex(Program.HelpMessages, 0, Program.HelpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
#pragma warning restore CA1309 // Использование порядкового сравнения строк

                if (index >= 0)
                {
                    Console.WriteLine(Program.HelpMessages[index][ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in Program.HelpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Exit from the application.
        /// </summary>
        /// <param name="parameters">Input parameters.</param>
        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            Program.IsRunning = false;
        }

        /// <summary>
        /// Warning message.
        /// </summary>
        /// <param name="command">Command name.</param>
        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }
    }
}