namespace FileCabinetApp
{
    /// <summary>
    /// Iterator for file cabinet service objects.
    /// </summary>
    public interface IRecordIterator
    {
        /// <summary>
        /// Gets next element from collection.
        /// </summary>
        /// <returns>File cabinet record from collection.</returns>
        public FileCabinetRecord GetNext();

        /// <summary>
        /// Check if collection has next element.
        /// </summary>
        /// <returns>Check.</returns>
        public bool HasMore();
    }
}
