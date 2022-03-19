namespace FileCabinetGenerator
{
    /// <summary>
    /// Start class for file cabinet generator.
    /// </summary>
    public static class Program
    {
        private static string type = "xml";
        private static string path = "records.xml";
        private static int amount = 0;
        private static int id = 0;

        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">Arguments of command line.</param>
        public static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string argument = string.Empty;

                if (i < args.Length - 1)
                {
                    argument = args[i + 1];
                }

                ParseCommand(args[i], argument);
            }

            if (amount < 0)
            {
                throw new ArgumentException("wrond amount", nameof(amount));
            }

            if (id < 0)
            {
                throw new ArgumentException("wrond start id", nameof(id));
            }

            string[] formats = { "CSV", "XML" };
            type = type.ToUpperInvariant();
            FileCabinetRecordGenerator generator = new FileCabinetRecordGenerator(id, amount);
            generator.GenerateRecords();

            if (!string.IsNullOrEmpty(path) && formats.Contains(type))
            {
                StreamWriter writer = new StreamWriter(path);

                if (type == "CSV")
                {
                    generator.ExportRecordsCsv(writer);
                }
                else if (type == "XML")
                {
                    generator.ExportRecordXml(writer);
                }

                writer.Flush();
                writer.Close();
            }
            Console.WriteLine($"{amount} records were written to {path}");
        }

        /// <summary>
        /// Parse command line argumet.
        /// </summary>
        /// <param name="command">CL argument.</param>
        /// <param name="argument">If input parameter has '-' type this is parameter.</param>
        private static void ParseCommand(string command, string argument)
        {
            string[][] commands = new string[][]
               {
                new string[] { "--output-type", "-t" },
                new string[] { "--output", "-o" },
                new string[] { "--records-amount", "-a" },
                new string[] { "--start-id", "-i" },
               };

            string currentCommand = string.Empty;
            string mode = string.Empty;
            argument = argument.Trim(new char[] { '\'', '"' });

            if (command.Contains("--", StringComparison.InvariantCulture))
            {
                for (int j = 0; j < commands.Length; j++)
                {
                    int breakingElementIndex = command.IndexOf('=', StringComparison.InvariantCulture);
                    currentCommand = command.Substring(0, breakingElementIndex);
                    mode = command.Substring(breakingElementIndex + 1, command.Length - breakingElementIndex - 1);

                    if (currentCommand == commands[j][0])
                    {
                        AssignInputParameters(commands[j][1], mode);
                        break;
                    }
                }
            }
            else if (command.Contains("-", StringComparison.InvariantCulture))
            {
                for (int j = 0; j < commands.Length; j++)
                {
                    if (command == commands[j][1])
                    {
                        AssignInputParameters(commands[j][1], argument);
                        break;
                    }
                }
            }
        }

        private static void AssignInputParameters(string command, string parameter)
        {
            switch (command)
            {
                case "-t":
                    type = parameter;
                    break;
                case "-o":
                    path = parameter;
                    break;
                case "-a":
                    amount = Convert.ToInt32(parameter);
                    break;
                case "-i":
                    id = Convert.ToInt32(parameter);
                    break;
            }
        }
    }
}