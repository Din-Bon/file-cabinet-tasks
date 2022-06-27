namespace FileCabinetApp
{
    /// <summary>
    /// Class that handle help command.
    /// </summary>
    internal class HelpCommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static readonly string[][] HelpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "show stats", "The 'stat' show stats." },
            new string[] { "create", "create new record", "The 'create' create new record." },
            new string[] { "insert", "insert new record", "The 'insert' insert new record to the list with all records." },
            new string[] { "list", "show list of records", "The 'list' show list of records." },
            new string[] { "edit", "edit existing record", "The 'edit' edit existing record." },
            new string[] { "find", "finds a record by its property", "The 'find' finds record by property." },
            new string[] { "export", "export records in file(csv/xml))", "The 'export' export records in file(csv/xml)." },
            new string[] { "import", "import records from file(csv/xml))", "The 'import' import records in file(csv/xml)." },
            new string[] { "remove", "remove record", "The 'remove' remove record by id." },
            new string[] { "delete", "delete record", "The 'delete' delete record by any parameter." },
            new string[] { "purge", "purge records", "The 'purge' purge removed records." },
        };

        /// <summary>
        /// Execute help command.
        /// </summary>
        /// <param name="request">Request.</param>
        public override void Handle(AppCommandRequest request)
        {
            var command = request.Command;
            var parameters = request.Parameters;

            if (command == "help")
            {
                PrintHelp(parameters);
            }
            else
            {
                base.Handle(request);
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
                var index = Array.FindIndex(HelpMessages, 0, HelpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
#pragma warning restore CA1309 // Использование порядкового сравнения строк

                if (index >= 0)
                {
                    Console.WriteLine(HelpMessages[index][ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in HelpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }
    }
}
