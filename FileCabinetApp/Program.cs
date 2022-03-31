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
        public static readonly string[][] HelpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "show stats", "The 'stat' show stats." },
            new string[] { "create", "create new record", "The 'create' create new record." },
            new string[] { "list", "show list of records", "The 'list' show list of records." },
            new string[] { "edit", "edit existing record", "The 'edit' edit existing record." },
            new string[] { "find", "finds a record by its property", "The 'find' finds record by property." },
            new string[] { "export", "export records in file(csv/xml))", "The 'export' export records in file(csv/xml)." },
            new string[] { "import", "import records from file(csv/xml))", "The 'import' import records in file(csv/xml)." },
            new string[] { "remove", "remove record", "The 'remove' remove record by id." },
            new string[] { "purge", "purge records", "The 'purge' purge removed records." },
        };

        public static IFileCabinetService FileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
        public static bool IsRunning = true;
        private const string DeveloperName = "Artem Filimonov";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";

        /// <summary>
        /// Gets and sets validation mode.
        /// </summary>
        /// <value>Validation mode.</value>
        public static string ValidationMode { get; private set; } = "DEFAULT";

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
                const int parametersIndex = 1;
                var command = inputs[commandIndex];
                var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var comandHandler = CreateCommandHandlers();
                AppCommandRequest request = new AppCommandRequest(command, parameters);
                comandHandler.Handle(request);
            }
            while (IsRunning);
        }

        /// <summary>
        /// Create command handlers.
        /// </summary>
        /// <returns>Command handler.</returns>
        private static ICommandHandler CreateCommandHandlers()
        {
            var commandHandler = new CommandHandler();
            return commandHandler;
        }

        /// <summary>
        /// Change validation mode.
        /// </summary>
        /// <param name="mode">Validation mode.</param>
        private static void ChangeValidationMode(string mode)
        {
            if (mode == "CUSTOM")
            {
                FileCabinetService = new FileCabinetMemoryService(new CustomValidator());
                ValidationMode = mode;
            }
            else
            {
                FileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
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

                if (ValidationMode == "CUSTOM")
                {
                    FileCabinetService = new FileCabinetFilesystemService(stream, new CustomValidator());
                }
                else
                {
                    FileCabinetService = new FileCabinetFilesystemService(stream, new DefaultValidator());
                }
            }
            else
            {
                if (ValidationMode == "CUSTOM")
                {
                    FileCabinetService = new FileCabinetMemoryService(new CustomValidator());
                }
                else
                {
                    FileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
                }
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

            StringComparison culture = StringComparison.InvariantCulture;
            string validationMode = "DEFAULT";
            string storageMode = "MEMORY";

            for (int i = 0; i < cmdArguments.Length; i++)
            {
                if (cmdArguments[i].Contains("-v", culture))
                {
                    if (cmdArguments[i].Contains("--validation-rules=", culture))
                    {
                        validationMode = cmdArguments[i].Split('=')[1].ToUpperInvariant();
                    }
                    else if (cmdArguments[i] == "-v")
                    {
                        validationMode = cmdArguments[i + 1].ToUpperInvariant();
                    }

                    ChangeValidationMode(validationMode);
                }
                else if (cmdArguments[i].Contains("-s", culture))
                {
                    if (cmdArguments[i].Contains("--storage=", culture))
                    {
                        storageMode = cmdArguments[i].Split('=')[1].ToUpperInvariant();
                    }
                    else if (cmdArguments[i] == "-s")
                    {
                        storageMode = cmdArguments[i + 1].ToUpperInvariant();
                    }
                }
            }

            ChangeStorage(storageMode);
        }
    }
}