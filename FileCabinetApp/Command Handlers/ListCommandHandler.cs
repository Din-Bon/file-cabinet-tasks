using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// Class that handle list command.
    /// </summary>
    internal class ListCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service object.</param>
        public ListCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
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
        /// Print records data on the console.
        /// </summary>
        /// <param name="records">Array of the records.</param>
        private static void PrintRecords(ReadOnlyCollection<FileCabinetRecord> records)
        {
            for (int i = 0; i < records.Count; i++)
            {
                Console.WriteLine(records[i].ToString());
            }
        }

        /// <summary>
        /// Shows list of records.
        /// </summary>
        /// <param name="parameters">Input parameters.</param>
        private void List(string parameters)
        {
            ReadOnlyCollection<FileCabinetRecord> records = this.fileCabinetService.GetRecords();
            PrintRecords(records);
        }
    }
}
