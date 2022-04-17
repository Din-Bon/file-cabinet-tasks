namespace FileCabinetApp
{
    /// <summary>
    /// Class that handle export command.
    /// </summary>
    internal class ExportCommandHandler : CommandHandlerBase
    {
        /// <summary>
        /// Execute export command.
        /// </summary>
        /// <param name="request">Request.</param>
        public override void Handle(AppCommandRequest request)
        {
            var command = request.Command;
            var parameters = request.Parameters;

            if (command == "export")
            {
                Export(parameters);
            }
            else
            {
                base.Handle(request);
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
    }
}
