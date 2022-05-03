using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// Class that handle find command.
    /// </summary>
    internal class FindCommandHandler : ServiceCommandHandlerBase
    {
        private Action<IEnumerable<FileCabinetRecord>> printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service object.</param>
        /// <param name="printer">Print record in some style.</param>
        public FindCommandHandler(IFileCabinetService fileCabinetService, Action<IEnumerable<FileCabinetRecord>> printer)
            : base(fileCabinetService)
        {
            this.printer = printer;
        }

        /// <summary>
        /// Execute find command.
        /// </summary>
        /// <param name="request">Request.</param>
        public override void Handle(AppCommandRequest request)
        {
            var command = request.Command;
            var parameters = request.Parameters;

            if (command == "find")
            {
                this.Find(parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        /// <summary>
        /// Find record.
        /// </summary>
        /// <param name="parameters">Array from property and value.</param>
        private void Find(string parameters)
        {
            if (string.IsNullOrEmpty(parameters))
            {
                throw new ArgumentNullException(nameof(parameters), "wrong find command");
            }

            var commands = parameters.Split(' ', 2);
            string property = commands[0].ToUpperInvariant();
            string parameter = commands[1].Trim('"');

            switch (property)
            {
                case "FIRSTNAME":
                    this.printer(this.fileCabinetService.FindByFirstName(parameter));
                    break;
                case "LASTNAME":
                    this.printer(this.fileCabinetService.FindByLastName(parameter));
                    break;
                case "DATEOFBIRTH":
                    this.printer(this.fileCabinetService.FindByDateofbirth(parameter));
                    break;
            }
        }
    }
}
