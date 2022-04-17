namespace FileCabinetApp
{
    /// <summary>
    /// Class that handle import command.
    /// </summary>
    internal class ImportCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service object.</param>
        public ImportCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        /// <summary>
        /// Execute import command.
        /// </summary>
        /// <param name="request">Request.</param>
        public override void Handle(AppCommandRequest request)
        {
            var command = request.Command;
            var parameters = request.Parameters;

            if (command == "import")
            {
                this.Import(parameters);
            }
            else
            {
                base.Handle(request);
            }
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
        /// Import records from file.
        /// </summary>
        /// <param name="parameters">Array from property and value.</param>
        private void Import(string parameters)
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
                FileCabinetServiceSnapshot serviceSnapshot = this.fileCabinetService.MakeSnapshot();

                if (property == "CSV")
                {
                    serviceSnapshot.LoadFromCsv(reader);
                }
                else if (property == "XML")
                {
                    serviceSnapshot.LoadFromXml(reader);
                }

                int count = this.fileCabinetService.Restore(serviceSnapshot);
                reader.Close();
                Console.WriteLine($"{count} records were imported from {path}.");
            }
        }
    }
}
