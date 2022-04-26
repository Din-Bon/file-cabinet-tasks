namespace FileCabinetApp
{
    /// <summary>
    /// Base class for all classes that
    /// use fileCabinetService.
    /// </summary>
    internal abstract class ServiceCommandHandlerBase : CommandHandlerBase
    {
        /// <summary>
        /// Base file cabinet service.
        /// </summary>
#pragma warning disable SA1401 // Fields should be private
        protected IFileCabinetService fileCabinetService = new FileCabinetMemoryService(new ValidatorBuilder().CreateDefault());
#pragma warning restore SA1401 // Fields should be private

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
