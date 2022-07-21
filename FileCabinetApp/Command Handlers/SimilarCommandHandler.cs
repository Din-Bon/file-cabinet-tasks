using System.Text.RegularExpressions;

namespace FileCabinetApp
{
    /// <summary>
    /// The last handler, that give user
    /// similar commands with input.
    /// </summary>
    internal class SimilarCommandHandler : ServiceCommandHandlerBase
    {
        private static readonly string[] Commands = new string[]
        {
            "help",
            "exit",
            "stat",
            "create",
            "insert",
            "list",
            "update",
            "find",
            "export",
            "import",
            "delete",
            "purge",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="SimilarCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service object.</param>
        public SimilarCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        /// <summary>
        /// Execute purge command.
        /// </summary>
        /// <param name="request">Request.</param>
        public override void Handle(AppCommandRequest request)
        {
            var command = request.Command;
            var parameters = request.Parameters;
            Similar(command);
        }

        /// <summary>
        /// Write on the console similar command with input string.
        /// </summary>
        /// <param name="input">Input string.</param>
        private static void Similar(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input), "input command is empty");
            }

            Console.WriteLine($"There is no '{input}' command. See \'help\'");
            Console.WriteLine();
            Console.WriteLine("The most similar commands are");
            List<string> similarCommands = new List<string>();

            for (int i = 0, count = 0; i < Commands.Length; i++)
            {
                string comm = new string(Commands[i].Distinct().ToArray());
                comm = string.Concat(comm.OrderBy(x => x).ToArray());
                string inp = new string(input.Distinct().ToArray());
                inp = string.Concat(inp.OrderBy(x => x).ToArray());
                bool hasChars = inp.All(x => comm.Contains(x, StringComparison.InvariantCultureIgnoreCase));
                if (hasChars && count < 5)
                {
                    Console.WriteLine("\t" + Commands[i]);
                    count++;
                }
            }
        }
    }
}
