namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Artem Filimonov";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static FileCabinetService fileCabinetService = new FileCabinetService();

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "show stats", "The 'stat' show stats." },
            new string[] { "create", "create new id", "The 'create' create new id." },
            new string[] { "list", "show list of ids", "The 'list' show list of ids." },
            new string[] { "edit", "edit existing id", "The 'edit' edit existing id." },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

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

        private static void List(string parameters)
        {
            FileCabinetRecord[] records = fileCabinetService.GetRecords();
            for (int i = 0; i < records.Length; i++)
            {
                Console.WriteLine($"#{i + 1}, {records[i].FirstName}, {records[i].LastName}, " +
                    $"{records[i].DateOfBirth.ToString("yyyy-MMM-dd", System.Globalization.CultureInfo.InvariantCulture)}, " +
                    $"{records[i].Income}, {records[i].Tax}, {records[i].Block}");
            }
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

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
            Console.Write("Income: ");
            short income;

            if (!short.TryParse(Console.ReadLine(), out income))
            {
                throw new ArgumentException("wrong income");
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

            var dateOfBirth = DateTime.ParseExact(date, "MM/dd/yyyy", System.Globalization.CultureInfo.CurrentCulture);

            if (!ExceptionCheck(firstName, lastName, dateOfBirth, income, tax, block))
            {
                fileCabinetService.EditRecord(id, firstName, lastName, dateOfBirth, income, tax, block);
                Console.WriteLine($"Record #{id} is updated.");
            }

            Console.WriteLine($"Wrong parameters\nRecord #{id} isn't updated.");
        }

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
                    throw new ArgumentException("wrong income");
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

                var dateOfBirth = DateTime.ParseExact(date, "MM/dd/yyyy", System.Globalization.CultureInfo.CurrentCulture);

                check = ExceptionCheck(firstName, lastName, dateOfBirth, income, tax, block);

                if (!check)
                {
                    Console.WriteLine($"Record #{fileCabinetService.CreateRecord(firstName, lastName, dateOfBirth, income, tax, block)} is created.");
                }
            }
        }

        private static bool ExceptionCheck(string? firstName, string? lastName, DateTime dateOfBirth, short income, decimal tax, char block)
        {
            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 2 || firstName.Length > 60)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 2 || lastName.Length > 60)
            {
                return true;
            }

            if (dateOfBirth < new DateTime(1950, 01, 01) || dateOfBirth > DateTime.Today)
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

            if (block < 65 || block > 90)
            {
                return true;
            }

            return false;
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

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

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }
    }
}