namespace FileCabinetApp
{
    /// <summary>
    /// Class, that working with the records(with default validate system).
    /// </summary>
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Get current mode validator.
        /// </summary>
        /// <returns>Validator with concrete mode.</returns>
        protected override DefaultValidator CreateValidator()
        {
            return new DefaultValidator();
        }
    }
}
