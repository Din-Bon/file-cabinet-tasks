namespace FileCabinetApp
{
    /// <summary>
    /// Class, that working with the records(with custom validate system).
    /// </summary>
    public class FileCabinetCustomService : FileCabinetMemoryService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetCustomService"/> class.
        /// </summary>
        public FileCabinetCustomService()
            : base(new CustomValidator())
        {
        }
    }
}
