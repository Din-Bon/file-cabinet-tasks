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
        private static bool IsRunning = true;
        private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());

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
            var helpHandler = new HelpCommandHandler();
            var createHandler = new CreateCommandHandler(fileCabinetService);
            var exitHandler = new ExitCommandHandler(obj => { IsRunning = false; });
            var statHandler = new StatCommandHandler(fileCabinetService);
            var listHandler = new ListCommandHandler(fileCabinetService, DefaultRecordPrint);
            var editHandler = new EditCommandHandler(fileCabinetService);
            var findHandler = new FindCommandHandler(fileCabinetService, DefaultRecordPrint);
            var exportHandler = new ExportCommandHandler(fileCabinetService);
            var importHandler = new ImportCommandHandler(fileCabinetService);
            var removeHandler = new RemoveCommandHandler(fileCabinetService);
            var purgeHandler = new PurgeCommandHandler(fileCabinetService);
            helpHandler.SetNext(createHandler);
            createHandler.SetNext(exitHandler);
            exitHandler.SetNext(statHandler);
            statHandler.SetNext(listHandler);
            listHandler.SetNext(editHandler);
            editHandler.SetNext(findHandler);
            findHandler.SetNext(exportHandler);
            exportHandler.SetNext(importHandler);
            importHandler.SetNext(removeHandler);
            removeHandler.SetNext(purgeHandler);
            return helpHandler;
        }

        /// <summary>
        /// Default record printer.
        /// </summary>
        /// <param name="records">Source.</param>
        private static void DefaultRecordPrint(IEnumerable<FileCabinetRecord> records)
        {
            foreach (var record in records)
            {
                Console.WriteLine(record.ToString());
            }
        }

        /// <summary>
        /// Change validation mode.
        /// </summary>
        /// <param name="mode">Validation mode.</param>
        private static IRecordValidator ChangeValidationMode(string mode)
        {
            var validator = new ValidatorBuilder();

            if (mode == "CUSTOM")
            {
                validator
                    .ValidateFirstName(2, 20)
                    .ValidateLastName(2, 20)
                    .ValidateDateOfBirth(new DateTime(1950, 01, 01), new DateTime(2015, 01, 01))
                    .ValidateIncome(150)
                    .ValidateTax(10, 70)
                    .ValidateBlock(65, 90);
                ValidationMode = mode;
            }
            else
            {
                validator
                    .ValidateFirstName(2, 100)
                    .ValidateLastName(2, 100)
                    .ValidateDateOfBirth(new DateTime(1950, 01, 01), new DateTime(2020, 01, 01))
                    .ValidateIncome(0)
                    .ValidateTax(0, 100)
                    .ValidateBlock(65, 90);
            }

            return validator.Create();
        }

        /// <summary>
        /// Change storage system type.
        /// </summary>
        /// <param name="mode">Storage mode.</param>
        private static void ChangeStorage(string mode, string validationMode)
        {
            if (mode == "FILE")
            {
                FileStream stream = new FileStream("cabinet-records.db", FileMode.OpenOrCreate);
                fileCabinetService = new FileCabinetFilesystemService(stream, ChangeValidationMode(validationMode));
            }
            else
            {
                fileCabinetService = new FileCabinetMemoryService(ChangeValidationMode(validationMode));
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

            ChangeStorage(storageMode, validationMode);
        }
    }
}