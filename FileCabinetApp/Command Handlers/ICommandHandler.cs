namespace FileCabinetApp
{
    /// <summary>
    /// Interface of command handlers.
    /// </summary>
    internal interface ICommandHandler
    {
        /// <summary>
        /// Set next command handler.
        /// </summary>
        /// <param name="handler">Next handler.</param>
        public void SetNext(ICommandHandler handler);

        /// <summary>
        /// Handle with request.
        /// </summary>
        /// <param name="request">Request.</param>
        public void Handle(AppCommandRequest request);
    }
}
