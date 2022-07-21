using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Class that handle update command.
    /// </summary>
    internal class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service object.</param>
        public UpdateCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        /// <summary>
        /// Execute edit command.
        /// </summary>
        /// <param name="request">Request.</param>
        public override void Handle(AppCommandRequest request)
        {
            var command = request.Command;
            var parameters = request.Parameters;

            if (command == "update")
            {
                this.Update(parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        /// <summary>
        /// Modify existing records.
        /// </summary>
        /// <param name="parameters">Existing records parameters.</param>
        private void Update(string parameters)
        {
            if (string.IsNullOrEmpty(parameters))
            {
                throw new ArgumentNullException(nameof(parameters), "wrong parameters: need parameters of existing records for update");
            }

            if (parameters[..3] != "set")
            {
                throw new ArgumentException("wrong command: expected \'update set\'", nameof(parameters));
            }

            string[] names = { "id", "firstname", "lastname", "dateofbirth", "income", "tax", "block" };
            string stringInProcess = parameters.Remove(0, 4);
            stringInProcess = Regex.Replace(stringInProcess, "[,= ]", string.Empty);
            stringInProcess = stringInProcess.Replace("and", string.Empty, StringComparison.InvariantCulture);
            string[] newAndOld = stringInProcess.Split("where");
            string newParameters = newAndOld[0];
            string oldParameters = newAndOld[1];
            string[] oldParametersArray = oldParameters.Split('\'');
            string[] newParametersArray = newParameters.Split('\'');
            string[] oldParametersResult = new string[7];
            string[] newParametersResult = new string[7];

            try
            {
                for (int i = 0; i < names.Length; i++)
                {
                    string? newP = Array.Find(newParametersArray, param => param.ToUpperInvariant() == names[i].ToUpperInvariant());

                    if (newP != null)
                    {
                        newParametersResult[i] = newParametersArray[Array.IndexOf(newParametersArray, newP) + 1];
                    }

                    string? oldP = Array.Find(oldParametersArray, param => param.ToUpperInvariant() == names[i].ToUpperInvariant());

                    if (oldP != null)
                    {
                        oldParametersResult[i] = oldParametersArray[Array.IndexOf(oldParametersArray, oldP) + 1];
                    }
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"wrong input parameters: {ex.Message}");
            }

            this.fileCabinetService.UpdateRecords(oldParametersResult, newParametersResult);
        }
    }
}
