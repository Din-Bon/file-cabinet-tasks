﻿using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// Class that handle find command.
    /// </summary>
    internal class FindCommandHandler : CommandHandlerBase
    {
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
                Find(parameters);
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
        private static void Find(string parameters)
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
                    PrintRecords(Program.FileCabinetService.FindByFirstName(parameter));
                    break;
                case "LASTNAME":
                    PrintRecords(Program.FileCabinetService.FindByLastName(parameter));
                    break;
                case "DATEOFBIRTH":
                    PrintRecords(Program.FileCabinetService.FindByDateofbirth(parameter));
                    break;
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
    }
}
