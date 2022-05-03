using System.Collections;

namespace FileCabinetApp
{
    /// <summary>
    /// Collection for filesystem service.
    /// </summary>
    public class FilesystemEnumerable : IEnumerable<FileCabinetRecord>
    {
        private readonly List<long> positions;
        private readonly FileStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesystemEnumerable"/> class.
        /// </summary>
        /// <param name="positions">List of record positions in file.</param>
        /// <param name="stream">File.</param>
        public FilesystemEnumerable(List<long> positions, FileStream stream)
        {
            this.positions = positions;
            this.stream = stream;
        }

        /// <summary>
        /// Get enumerator for filesystem collection.
        /// </summary>
        /// <returns>Enumerator.</returns>
        public IEnumerator<FileCabinetRecord> GetEnumerator() => new FilesystemEnumerator(this.positions, this.stream);

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
