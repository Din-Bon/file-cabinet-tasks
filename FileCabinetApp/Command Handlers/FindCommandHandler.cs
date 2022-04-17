using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// Class that handle find command.
    /// </summary>
    internal class FindCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service object.</param>
        public FindCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
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
                    PrintRecords(this.fileCabinetService.FindByFirstName(parameter));
                    break;
                case "LASTNAME":
                    PrintRecords(this.fileCabinetService.FindByLastName(parameter));
                    break;
                case "DATEOFBIRTH":
                    PrintRecords(this.fileCabinetService.FindByDateofbirth(parameter));
                    break;
            }
        }
    }
}
