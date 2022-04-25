namespace FileCabinetApp
{
    /// <summary>
    /// Interface that print records.
    /// </summary>
    internal interface IRecordPrinter
    {
        /// <summary>
        /// Print records in some style.
        /// </summary>
        /// <param name="records">Record collection.</param>
        public void Print(IEnumerable<FileCabinetRecord> records);
    }
}
