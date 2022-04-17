namespace FileCabinetApp
{
    /// <summary>
    /// Class that handle exit command.
    /// </summary>
    internal class ExitCommandHandler : CommandHandlerBase
    {
        private Action<bool> exit;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitCommandHandler"/> class.
        /// </summary>
        /// <param name="exit">Exit action.</param>
        public ExitCommandHandler(Action<bool> exit)
        {
            this.exit = exit;
        }

        /// <summary>
        /// Execute exit command.
        /// </summary>
        /// <param name="request">Request.</param>
        public override void Handle(AppCommandRequest request)
        {
            var command = request.Command;
            var parameters = request.Parameters;

            if (command == "exit")
            {
                this.Exit(parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        /// <summary>
        /// Exit from the application.
        /// </summary>
        /// <param name="parameters">Input parameters.</param>
        private void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            this.exit(true);
        }
    }
}
