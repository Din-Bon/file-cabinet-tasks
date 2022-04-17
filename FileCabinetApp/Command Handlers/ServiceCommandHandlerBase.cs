namespace FileCabinetApp
{
    /// <summary>
    /// Base class for all classes that
    /// use fileCabinetService.
    /// </summary>
    abstract internal class ServiceCommandHandlerBase : CommandHandlerBase
    {
        /// <summary>
        /// Base file cabinet service.
        /// </summary>
        protected IFileCabinetService fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCommandHandlerBase"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service object.</param>
        public ServiceCommandHandlerBase(IFileCabinetService fileCabinetService)
        {
            this.fileCabinetService = fileCabinetService;
        }
    }
}
