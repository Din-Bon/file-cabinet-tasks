namespace FileCabinetApp
{
    /// <summary>
    /// Print records in classic style.
    /// </summary>
    internal class DefaultRecordPrinter : IRecordPrinter
    {
        /// <summary>
        /// Default record printer.
        /// </summary>
        /// <param name="records">Source.</param>
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            foreach (var record in records)
            {
                Console.WriteLine(record.ToString());
            }
        }
    }
}
