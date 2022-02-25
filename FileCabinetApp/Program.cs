using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

namespace FileCabinetApp
{
    /// <summary>
    /// Start class for file cabinet application.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Artem Filimonov";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
        private static string validationMode = "DEFAULT";

        private static bool isRunning = true;

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
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "show stats", "The 'stat' show stats." },
            new string[] { "create", "create new record", "The 'create' create new record." },
            new string[] { "list", "show list of records", "The 'list' show list of records." },
            new string[] { "edit", "edit existing record", "The 'edit' edit existing record." },
            new string[] { "find", "finds a record by its property", "The 'find' finds record by property." },
            new string[] { "export", "export records in csv file", "The 'export' export records in csv file." },
        };

        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">Arguments of command line.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();
            ParseCLArguments(args);

            do
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                var inputs = line != null ? line.Split(' ', 2) : new string[] { string.Empty, string.Empty };
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

#pragma warning disable CA1309 // Использование порядкового сравнения строк
                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
#pragma warning restore CA1309 // Использование порядкового сравнения строк

                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
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
                    Console.WriteLine($"Record #{fileCabinetService.CreateRecord(person, income, tax, block)} is created.");
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

            if (id > fileCabinetService.GetStat() || id < 0)
            {
                throw new ArgumentException("id value larger than list", nameof(parameters));
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
                fileCabinetService.EditRecord(id, person, income, tax, block);
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
            ReadOnlyCollection<FileCabinetRecord> records = fileCabinetService.GetRecords();
            PrintRecords(records);
        }

        /// <summary>
        /// Shows count of existing records.
        /// </summary>
        /// <param name="parameters">Input prameters.</param>
        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
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
                    PrintRecords(fileCabinetService.FindByFirstName(parameter));
                    break;
                case "LASTNAME":
                    PrintRecords(fileCabinetService.FindByLastName(parameter));
                    break;
                case "DATEOFBIRTH":
                    PrintRecords(fileCabinetService.FindByDateofbirth(parameter));
                    break;
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
            string path = ParseExportString(fileName);
            string property = commands[0].ToUpperInvariant();
            string[] formats = { "CSV", "XML" };

            if (!string.IsNullOrEmpty(path) && formats.Contains(property))
            {
                StreamWriter writer = new StreamWriter(path);
                FileCabinetServiceSnapshot serviceSnapshot = fileCabinetService.MakeSnapshot();

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
        private static string ParseExportString(string input)
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

            if (validationMode == "CUSTOM")
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
        /// Change validation mode.
        /// </summary>
        /// <param name="mode">Validation mode.</param>
        private static void ChangeValidationMode(string mode)
        {
            if (mode == "CUSTOM")
            {
                fileCabinetService = new FileCabinetMemoryService(new CustomValidator());
                validationMode = mode;
            }
            else
            {
                fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
            }
        }

        /// <summary>
        /// Change storage system type.
        /// </summary>
        /// <param name="mode">Storage mode.</param>
        private static void ChangeStorage(string mode)
        {
            if (mode == "FILE")
            {
                FileStream stream = new FileStream("cabinet-records.db", FileMode.OpenOrCreate);
                fileCabinetService = new FileCabinetFilesystemService(stream);
            }
            else if (mode == "MEMORY")
            {
                fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
            }
        }

        /// <summary>
        /// Parse command line argumets string.
        /// </summary>
        /// <param name="cmdArguments">CL arguments.</param>
        private static void ParseCLArguments(string[] cmdArguments)
        {
            string[][] commands = new string[][]
            {
                new string[] { "--validation-rules", "-v" },
                new string[] { "--storage", "-s" },
            };

            if (cmdArguments.Length != 0)
            {
                string command = cmdArguments[0];
                string currentCommand = string.Empty;
                string mode = string.Empty;

                if (cmdArguments.Length == 1 && command.Contains('=', StringComparison.InvariantCulture))
                {
                    int breakingElementIndex = command.IndexOf('=', StringComparison.InvariantCulture);
                    currentCommand = command.Substring(0, breakingElementIndex);
                    mode = command.Substring(breakingElementIndex + 1, command.Length - breakingElementIndex - 1).ToUpperInvariant();
                }
                else if (cmdArguments.Length == 2)
                {
                    currentCommand = command;
                    mode = cmdArguments[1].ToUpperInvariant();
                }

                if (commands[0].Contains(currentCommand))
                {
                    ChangeValidationMode(mode);
                }
                else if (commands[1].Contains(currentCommand))
                {
                    ChangeStorage(mode);
                }
            }
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
        /// Warning message.
        /// </summary>
        /// <param name="command">Comman name.</param>
        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
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
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
#pragma warning restore CA1309 // Использование порядкового сравнения строк

                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
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
            isRunning = false;
        }
    }
}