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
        private static bool isRunning = true;
        private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService(new ValidatorBuilder().CreateDefault());

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
            //insert (id, firstname, lastname, dateofbirth, income, tax, block) values ('2', 'jimm', 'jhonna', '5/18/1996', '1500', '70', 'G')
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
            while (isRunning);
        }

        /// <summary>
        /// Create command handlers.
        /// </summary>
        /// <returns>Command handler.</returns>
        private static ICommandHandler CreateCommandHandlers()
        {
            var helpHandler = new HelpCommandHandler();
            var createHandler = new CreateCommandHandler(fileCabinetService);
            var insertHandler = new InsertCommandHandler(fileCabinetService);
            var exitHandler = new ExitCommandHandler(obj => { isRunning = false; });
            var statHandler = new StatCommandHandler(fileCabinetService);
            var listHandler = new ListCommandHandler(fileCabinetService, DefaultRecordPrint);
            var editHandler = new EditCommandHandler(fileCabinetService);
            var findHandler = new FindCommandHandler(fileCabinetService, DefaultRecordPrint);
            var exportHandler = new ExportCommandHandler(fileCabinetService);
            var importHandler = new ImportCommandHandler(fileCabinetService);
            var removeHandler = new RemoveCommandHandler(fileCabinetService);
            var purgeHandler = new PurgeCommandHandler(fileCabinetService);
            helpHandler.SetNext(createHandler);
            createHandler.SetNext(insertHandler);
            insertHandler.SetNext(exitHandler);
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
        /// Default record printer(if source is iterator).
        /// </summary>
        /// <param name="records">Source.</param>
        private static void DefaultRecordPrint(IRecordIterator records)
        {
            while (records.HasMore())
            {
                Console.WriteLine(records.GetNext().ToString());
            }
        }

        /// <summary>
        /// Change validation mode.
        /// </summary>
        /// <param name="mode">Validation mode.</param>
        private static IRecordValidator ChangeValidationMode(string mode)
        {
            IRecordValidator validator;

            if (mode == "CUSTOM")
            {
                validator = new ValidatorBuilder().CreateCustom();
                ValidationMode = mode;
            }
            else
            {
                validator = new ValidatorBuilder().CreateDefault();
            }

            return validator;
        }

        /// <summary>
        /// Change storage system type.
        /// </summary>
        /// <param name="mode">Storage mode.</param>
        private static void ChangeStorage(string mode, string validationMode, bool useStopwatch, bool useLogger)
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

            UseLogger(useLogger);
            UseStopwatch(useStopwatch);
        }

        /// <summary>
        /// Add timer to service methods.
        /// </summary>
        /// <param name="useStopwatch">Use timer?.</param>
        private static void UseStopwatch(bool useStopwatch)
        {
            if (useStopwatch)
            {
                fileCabinetService = new ServiceMeter(fileCabinetService);
            }
        }

        /// <summary>
        /// Add log system to service.
        /// </summary>
        /// <param name="useLogger">Use logger?.</param>
        private static void UseLogger(bool useLogger)
        {
            if (useLogger)
            {
                UseStopwatch(true);
                fileCabinetService = new ServiceLogger(fileCabinetService);
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
            bool useStopwatch = cmdArguments.Contains("use-stopwatch");
            bool useLogger = cmdArguments.Contains("use-logger");
            string validationMode = "DEFAULT";
            string storageMode = "MEMORY";

            var valmode = Array.Find(cmdArguments, argument => argument.Contains("--validation-rules=", culture));

            if (valmode != null)
            {
                validationMode = valmode.Split('=')[1].ToUpperInvariant();
            }

            if (cmdArguments.Contains("-v"))
            {
                var modeIndex = Array.IndexOf(cmdArguments, "-v");

                if (modeIndex != -1)
                {
                    validationMode = cmdArguments[modeIndex + 1].ToUpperInvariant();
                }
            }

            var stormode = Array.Find(cmdArguments, argument => argument.Contains("--storage=", culture));

            if (stormode != null)
            {
                storageMode = stormode.Split('=')[1].ToUpperInvariant();
            }

            if (cmdArguments.Contains("-s"))
            {
                var modeIndex = Array.IndexOf(cmdArguments, "-s");

                if (modeIndex != -1)
                {
                    storageMode = cmdArguments[modeIndex + 1].ToUpperInvariant();
                }
            }

            ChangeStorage(storageMode, validationMode, useStopwatch, useLogger);
        }
    }
}