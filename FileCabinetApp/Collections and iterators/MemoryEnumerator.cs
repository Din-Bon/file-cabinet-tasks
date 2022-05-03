using System.Collections;
using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// Enumerator for memory service record collections.
    /// </summary>
    public class MemoryEnumerator : IEnumerator<FileCabinetRecord>
    {
        private ReadOnlyCollection<FileCabinetRecord> memoryCollection;
        private int position = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryEnumerator"/> class.
        /// </summary>
        /// <param name="collection">Record collection.</param>
        public MemoryEnumerator(ReadOnlyCollection<FileCabinetRecord> collection)
        {
            this.memoryCollection = collection;
        }

        /// <summary>
        /// Gets FileCabinetRecord from collection
        /// by current position.
        /// </summary>
        /// <value>Record.</value>
        public FileCabinetRecord Current
        {
            get
            {
                if (this.position >= this.memoryCollection.Count || this.position == -1)
                {
                    throw new InvalidOperationException();
                }

                return this.memoryCollection[this.position];
            }
        }

        /// <summary>
        /// Gets by default object from
        /// IEnumerator FileCabinetRecord.
        /// </summary>
        /// <value>Record.</value>
        object IEnumerator.Current => this.Current;

        /// <summary>
        /// Increment positions.
        /// </summary>
        /// <returns>Success of operation.</returns>
        public bool MoveNext()
        {
            if (this.position < this.memoryCollection.Count - 1 && this.memoryCollection.Count > 0)
            {
                this.position++;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set position at start.
        /// </summary>
        public void Reset() => this.position = -1;

        /// <summary>
        /// Dispose enumerator.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
