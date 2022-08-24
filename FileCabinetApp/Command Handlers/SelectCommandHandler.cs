using System.Text.RegularExpressions;

namespace FileCabinetApp
{
    /// <summary>
    /// Class that handle select command.
    /// </summary>
    internal class SelectCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service object.</param>
        public SelectCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        /// <summary>
        /// Execute export command.
        /// </summary>
        /// <param name="request">Request.</param>
        public override void Handle(AppCommandRequest request)
        {
            var command = request.Command;
            var parameters = request.Parameters;

            if (command == "select")
            {
                this.Select(parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        /// <summary>
        /// Parse input string and pass parameters to
        /// the method in fileCabinetService class.
        /// </summary>
        /// <param name="parameters">Input parameters.</param>
        private void Select(string parameters)
        {
            if (string.IsNullOrEmpty(parameters))
            {
                throw new ArgumentNullException(nameof(parameters), "wrong parameters: need parameters of existing records");
            }

            string[] names = { "id", "firstname", "lastname", "dateofbirth", "income", "tax", "block" };
            string[] values = new string[7];
            bool[] fieldsToReturn = new bool[7];

            try
            {
                string stringInProcess = Regex.Replace(parameters, "[= ]", string.Empty);
                stringInProcess = stringInProcess.Replace("and", string.Empty, StringComparison.InvariantCultureIgnoreCase);
                string[] newAndOld = stringInProcess.Split("where");
                string fieldsInProcess = newAndOld[0];
                string[] fields = fieldsInProcess.Split(',');
                string valuesInProcess = string.Empty;

                if (newAndOld.Length > 1)
                {
                    valuesInProcess = newAndOld[1];
                }

                string[] valuesArray = valuesInProcess.Split('\'');

                for (int i = 0; i < names.Length; i++)
                {
                    string? newP = Array.Find(valuesArray, param => param.ToUpperInvariant() == names[i].ToUpperInvariant());

                    if (newP != null)
                    {
                        values[i] = valuesArray[Array.IndexOf(valuesArray, newP) + 1];
                    }

                    string? fieldExist = Array.Find(fields, match => match.ToUpperInvariant() == names[i].ToUpperInvariant());

                    if (fieldExist != null)
                    {
                        fieldsToReturn[i] = true;
                    }
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"wrong input parameters: {ex.Message}");
            }

            Memoizer.MemoizeSelect(fieldsToReturn, values, this.fileCabinetService);
        }
    }
}
