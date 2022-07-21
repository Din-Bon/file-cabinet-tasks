namespace FileCabinetApp
{
    /// <summary>
    /// Class that handle delete command.
    /// </summary>
    internal class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service object.</param>
        public DeleteCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        /// <summary>
        /// Execute insert command.
        /// </summary>
        /// <param name="request">Request.</param>
        public override void Handle(AppCommandRequest request)
        {
            var command = request.Command;
            var parameters = request.Parameters;

            if (command == "delete")
            {
                this.Delete(parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        /// <summary>
        /// Check if file cabinet record have
        /// parameter with this name.
        /// </summary>
        /// <param name="parameter">Input parameter name.</param>
        /// <exception cref="ArgumentException">File cabinet record doesn't have parameter with that name.</exception>
        private static void ParameterValidation(string parameter)
        {
            string[] fieldNames = { "id", "firstname", "lastname", "dateofbirth", "income", "tax", "block" };

            if (!fieldNames.Contains(parameter))
            {
                throw new ArgumentException($"wrong parameter name: record doesn't contain \'{parameter}\' field", parameter);
            }
        }

        /// <summary>
        /// Delete record from list.
        /// </summary>
        /// <param name="parameters">Input parameters.</param>
        private void Delete(string parameters)
        {
            string[] args = parameters.Split(' ');
            string fieldName = string.Empty;
            string value = string.Empty;

            if (args[0] != "where")
            {
                throw new ArgumentException("wrong input arguments: required \"delete where fieldName=\'param\'\"", parameters);
            }
            else if (args.Length == 2)
            {
                string[] commandValueArray = args[1].Split('=');
                fieldName = commandValueArray[0];
                value = commandValueArray[1].Trim('\'');
            }
            else if (args.Length == 4)
            {
                fieldName = args[1];
                value = args[^1].Trim('\'');
            }
            else
            {
                throw new ArgumentException("wrong input arguments: required \"delete where fieldName=\'param\'\"", parameters);
            }

            ParameterValidation(fieldName);
            this.fileCabinetService.DeleteRecord(fieldName, value);
        }
    }
}
