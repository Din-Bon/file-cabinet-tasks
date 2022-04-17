using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Class that handle exit command.
    /// </summary>
    internal class ExitCommandHandler : CommandHandlerBase
    {
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
                Exit(parameters);
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
        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            Program.IsRunning = false;
        }
    }
}
