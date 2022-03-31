using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Class-request.
    /// </summary>
    internal class AppCommandRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppCommandRequest"/> class.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="parameters">Parameters.</param>
        public AppCommandRequest(string command, string parameters)
        {
            this.Command = command;
            this.Parameters = parameters;
        }

        /// <summary>
        /// Gets the command value.
        /// </summary>
        /// <value>Command.</value>
        public string Command { get; }

        /// <summary>
        /// Gets the parameters value.
        /// </summary>
        /// <value>Parameters.</value>
        public string Parameters { get; }
    }
}
