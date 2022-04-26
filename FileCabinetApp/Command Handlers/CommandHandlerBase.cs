using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Base command handler class.
    /// </summary>
    internal abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler? nextHandler;

        /// <summary>
        /// Set next command handler.
        /// </summary>
        /// <param name="handler">Command handler.</param>
        public void SetNext(ICommandHandler handler)
        {
            this.nextHandler = handler;
        }

        /// <summary>
        /// Passes the request further if
        /// the next handler is specified.
        /// </summary>
        /// <param name="request">Request.</param>
        public virtual void Handle(AppCommandRequest request)
        {
            if (this.nextHandler != null)
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}
