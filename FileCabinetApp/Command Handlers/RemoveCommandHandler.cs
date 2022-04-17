namespace FileCabinetApp
{
    /// <summary>
    /// Class that handle remove command.
    /// </summary>
    internal class RemoveCommandHandler : CommandHandlerBase
    {
        private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommandHandler"/> class.
        /// </summary>
        /// <param name="cabinetService">Service object.</param>
        public RemoveCommandHandler(IFileCabinetService cabinetService)
        {
            fileCabinetService = cabinetService;
        }

        /// <summary>
        /// Execute remove command.
        /// </summary>
        /// <param name="request">Request.</param>
        public override void Handle(AppCommandRequest request)
        {
            var command = request.Command;
            var parameters = request.Parameters;

            if (command == "remove")
            {
                Remove(parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        /// <summary>
        /// Remove record by id.
        /// </summary>
        /// <param name="parameters">String parameter(id).</param>
        private static void Remove(string parameters)
        {
            int id = 0;

            if (string.IsNullOrEmpty(parameters) || !int.TryParse(parameters, out id))
            {
                throw new ArgumentNullException(nameof(parameters), "empty id");
            }

            if (id <= 0)
            {
                throw new ArgumentException("wrond id (<1)", nameof(parameters));
            }

            fileCabinetService.RemoveRecord(id);
        }
    }
}
