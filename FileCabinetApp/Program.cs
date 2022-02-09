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
        private static FileCabinetService fileCabinetService = new FileCabinetService(new DefaultValidator());

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
            ChangeValidationMode(args);

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

            while (check)
            {
                Console.Write("First name: ");
                var firstName = Console.ReadLine();
                Console.Write("Last name: ");
                var lastName = Console.ReadLine();
                Console.Write("Date of birth: ");
                string? date = Console.ReadLine();
                Console.Write("Income: ");
                short income;

                if (!short.TryParse(Console.ReadLine(), out income))
                {
                    throw new ArgumentException("income value larger than max value");
                }

                Console.Write("Tax: ");
                decimal tax;

                if (!decimal.TryParse(Console.ReadLine(), out tax))
                {
                    throw new ArgumentException("wrong tax");
                }

                Console.Write("Block: ");
                char block;

                if (!char.TryParse(Console.ReadLine(), out block))
                {
                    throw new ArgumentException("wrong block");
                }

                if (string.IsNullOrWhiteSpace(date))
                {
                    throw new ArgumentException("wrong date");
                }

                var dateOfBirth = DateTime.ParseExact(date, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                check = ValidateParameters(firstName, lastName, dateOfBirth, income, tax, block);
                Person person = new Person() { FirstName = firstName, LastName = lastName, DateOfBirth = dateOfBirth };

                if (!check)
                {
                    Console.WriteLine($"Record #{fileCabinetService.CreateRecord(person, income, tax, block)} is created.");
                }
            }
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

            int id = Convert.ToInt32(parameters, System.Globalization.CultureInfo.CurrentCulture);
            Console.Write("First name: ");
            var firstName = Console.ReadLine();
            Console.Write("Last name: ");
            var lastName = Console.ReadLine();
            Console.Write("Date of birth: ");
            string? date = Console.ReadLine();
            Console.Write($"Income: ");
            short income;

            if (!short.TryParse(Console.ReadLine(), out income))
            {
                throw new ArgumentException("income value larger than max value");
            }

            Console.Write("Tax: ");
            decimal tax;

            if (!decimal.TryParse(Console.ReadLine(), out tax))
            {
                throw new ArgumentException("wrong tax");
            }

            Console.Write("Block: ");
            char block;

            if (!char.TryParse(Console.ReadLine(), out block))
            {
                throw new ArgumentException("wrong block");
            }

            if (string.IsNullOrWhiteSpace(date))
            {
                throw new ArgumentException("wrong date");
            }

            var dateOfBirth = DateTime.ParseExact(date, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            Person person = new Person() { FirstName = firstName, LastName = lastName, DateOfBirth = dateOfBirth };

            if (!ValidateParameters(firstName, lastName, dateOfBirth, income, tax, block))
            {
                fileCabinetService.EditRecord(id, person, income, tax, block);
                Console.WriteLine($"Record #{id} is updated.");
            }
            else
            {
                Console.WriteLine($"Wrong parameters{Environment.NewLine}Record #{id} isn't updated.");
            }
        }

        /// <summary>
        /// Shows list of records.
        /// </summary>
        /// <param name="parameters">Input parameters.</param>
        private static void List(string parameters)
        {
            FileCabinetRecord[] records = fileCabinetService.GetRecords();
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
        /// Validates input values.
        /// </summary>
        /// <param name="firstName">Person's first name.</param>
        /// <param name="lastName">Person's last name.</param>
        /// <param name="dateOfBirth">Person's date of birth.</param>
        /// <param name="income">Person's income.</param>
        /// <param name="tax">Person's tax.</param>
        /// <param name="block">Person's living block.</param>
        /// <returns>Is the data false.</returns>
        private static bool ValidateParameters(string? firstName, string? lastName, DateTime dateOfBirth, short income, decimal tax, char block)
        {
            int minNameLength = 2, maxNameLength = 60;
            DateTime minDateOfBirth = new DateTime(1950, 01, 01);
            DateTime maxDateOfBirth = DateTime.Today;
            int firstAlphabetLetter = 65, lastAlphabetLetter = 90;

            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < minNameLength || firstName.Length > maxNameLength)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < minNameLength || lastName.Length > maxNameLength)
            {
                return true;
            }

            if (dateOfBirth < minDateOfBirth || dateOfBirth > maxDateOfBirth)
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
        /// Change validation mode.
        /// </summary>
        /// <param name="cmdArguments">Command line arguments.</param>
        private static void ChangeValidationMode(string[] cmdArguments)
        {
            string mode = "DEFAULT";
            string cmdCommand = "--validation-rules=";

            if (cmdArguments.Length != 0)
            {
                string argument = cmdArguments[0];

                if (argument.Contains(cmdCommand, StringComparison.InvariantCulture))
                {
                    mode = argument.Substring(cmdCommand.Length);
                }
                else if (argument == "-v")
                {
                    mode = cmdArguments[1];
                }
            }

            if (mode.ToUpperInvariant() == "CUSTOM")
            {
                fileCabinetService = new FileCabinetService(new CustomValidator());
            }
        }

        /// <summary>
        /// Print records data on the console.
        /// </summary>
        /// <param name="records">Array of the records.</param>
        private static void PrintRecords(FileCabinetRecord[] records)
        {
            for (int i = 0; i < records.Length; i++)
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