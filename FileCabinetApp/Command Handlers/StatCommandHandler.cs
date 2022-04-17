namespace FileCabinetApp
{
    /// <summary>
    /// Class that handle stat command.
    /// </summary>
    internal class StatCommandHandler : CommandHandlerBase
    {
        private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());

        /// <summary>
        /// Initializes a new instance of the <see cref="StatCommandHandler"/> class.
        /// </summary>
        /// <param name="cabinetService">Service object.</param>
        public StatCommandHandler(IFileCabinetService cabinetService)
        {
            fileCabinetService = cabinetService;
        }

        /// <summary>
        /// Execute stat command.
        /// </summary>
        /// <param name="request">Request.</param>
        public override void Handle(AppCommandRequest request)
        {
            var command = request.Command;
            var parameters = request.Parameters;

            if (command == "stat")
            {
                Stat(parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        /// <summary>
        /// Shows count of existing records.
        /// </summary>
        /// <param name="parameters">Input prameters.</param>
        private static void Stat(string parameters)
        {
            var recordsCount = fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount.Item1} record(s). Removed - {recordsCount.Item2}");
        }
    }
}
