using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// Iterator for file cabinet memory service.
    /// </summary>
    public class MemoryIterator : IRecordIterator
    {
        private ReadOnlyCollection<FileCabinetRecord> records;
        private int currentPosition = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryIterator"/> class.
        /// </summary>
        /// <param name="records">Collection.</param>
        public MemoryIterator(ReadOnlyCollection<FileCabinetRecord> records)
        {
            this.records = records;
        }

        /// <summary>
        /// Gets next element if it exists.
        /// </summary>
        /// <returns>Next element.</returns>
        public FileCabinetRecord GetNext()
        {
            if (this.HasMore())
            {
                this.currentPosition++;
            }

            return this.records[this.currentPosition];
        }

        /// <summary>
        /// Gets true if collection still has elements.
        /// </summary>
        /// <returns>true/false.</returns>
        public bool HasMore()
        {
            if (this.currentPosition + 1 < this.records.Count)
            {
                return true;
            }

            return false;
        }
    }
}
