namespace FileCabinetApp
{
    /// <summary>
    /// Class that handle purge command.
    /// </summary>
    internal class PurgeCommandHandler : CommandHandlerBase
    {
        private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());

        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="cabinetService">Service object.</param>
        public PurgeCommandHandler(IFileCabinetService cabinetService)
        {
            fileCabinetService = cabinetService;
        }

        /// <summary>
        /// Execute purge command.
        /// </summary>
        /// <param name="request">Request.</param>
        public override void Handle(AppCommandRequest request)
        {
            var command = request.Command;
            var parameters = request.Parameters;

            if (command == "purge")
            {
                Purge(parameters);
            }
            else
            {
                PrintMissedCommandInfo(command);
            }
        }

        /// <summary>
        /// Purge records.
        /// </summary>
        /// <param name="parameters">Input parameters.</param>
        private static void Purge(string parameters)
        {
            int count = fileCabinetService.Purge();
            var length = fileCabinetService.GetStat().Item1 + count;
            Console.WriteLine($"Data file processing is completed: {count} of {length} records were purged.");
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
