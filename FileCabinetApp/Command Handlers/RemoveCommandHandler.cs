namespace FileCabinetApp
{
    /// <summary>
    /// Class that handle remove command.
    /// </summary>
    internal class RemoveCommandHandler : CommandHandlerBase
    {
        /// <summary>
        /// Execute remove command.
        /// </summary>
        /// <param name="request">Request.</param>
        public override void Handle(AppCommandRequest request)
        {
            var command = request.Command;
            var parameters = request.Parameters;

            if (command == "remove")
            {
                Remove(parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        /// <summary>
        /// Remove record by id.
        /// </summary>
        /// <param name="parameters">String parameter(id).</param>
        private static void Remove(string parameters)
        {
            int id = 0;

            if (string.IsNullOrEmpty(parameters) || !int.TryParse(parameters, out id))
            {
                throw new ArgumentNullException(nameof(parameters), "empty id");
            }

            if (id <= 0)
            {
                throw new ArgumentException("wrond id (<1)", nameof(parameters));
            }

            Program.FileCabinetService.RemoveRecord(id);
        }
    }
}
