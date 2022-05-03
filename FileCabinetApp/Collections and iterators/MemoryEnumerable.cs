using System.Collections;
using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// Collection for memory service.
    /// </summary>
    public class MemoryEnumerable : IEnumerable<FileCabinetRecord>
    {
        private ReadOnlyCollection<FileCabinetRecord> records;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryEnumerable"/> class.
        /// </summary>
        /// <param name="records">Record collection.</param>
        public MemoryEnumerable(ReadOnlyCollection<FileCabinetRecord> records)
        {
            this.records = records;
        }

        /// <summary>
        /// Get enumerator for memory collection.
        /// </summary>
        /// <returns>Enumerator.</returns>
        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            for (int i = 0; i < this.records.Count; i++)
            {
                yield return this.records[i];
            }
        }

        /// <summary>
        /// Gets default IEnumerable.
        /// </summary>
        /// <returns>none.</returns>
        /// <exception cref="NotImplementedException">Nothing default should return.</exception>
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
