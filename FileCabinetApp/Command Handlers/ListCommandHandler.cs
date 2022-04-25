using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// Class that handle list command.
    /// </summary>
    internal class ListCommandHandler : ServiceCommandHandlerBase
    {
        private Action<IEnumerable<FileCabinetRecord>> printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service object.</param>
        /// <param name="printer">Print record in some style.</param>
        public ListCommandHandler(IFileCabinetService fileCabinetService, Action<IEnumerable<FileCabinetRecord>> printer)
            : base(fileCabinetService)
        {
            this.printer = printer;
        }

        /// <summary>
        /// Execute list command.
        /// </summary>
        /// <param name="request">Request.</param>
        public override void Handle(AppCommandRequest request)
        {
            var command = request.Command;
            var parameters = request.Parameters;

            if (command == "list")
            {
                this.List(parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        /// <summary>
        /// Shows list of records.
        /// </summary>
        /// <param name="parameters">Input parameters.</param>
        private void List(string parameters)
        {
            ReadOnlyCollection<FileCabinetRecord> records = this.fileCabinetService.GetRecords();
            this.printer(records);
        }
    }
}
