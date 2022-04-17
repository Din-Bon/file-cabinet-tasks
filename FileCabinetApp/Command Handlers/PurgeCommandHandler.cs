namespace FileCabinetApp
{
    /// <summary>
    /// Class that handle purge command.
    /// </summary>
    internal class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service object.</param>
        public PurgeCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
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
                this.Purge(parameters);
            }
            else
            {
                PrintMissedCommandInfo(command);
            }
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

        /// <summary>
        /// Purge records.
        /// </summary>
        /// <param name="parameters">Input parameters.</param>
        private void Purge(string parameters)
        {
            int count = this.fileCabinetService.Purge();
            var length = this.fileCabinetService.GetStat().Item1 + count;
            Console.WriteLine($"Data file processing is completed: {count} of {length} records were purged.");
        }
    }
}
