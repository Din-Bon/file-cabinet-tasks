namespace FileCabinetApp
{
    /// <summary>
    /// Class, that working with the records(with custom validate system).
    /// </summary>
    public class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>
        /// Get current mode validator.
        /// </summary>
        /// <returns>Validator with concrete mode.</returns>
        protected override CustomValidator CreateValidator()
        {
            return new CustomValidator();
        }
    }
}
