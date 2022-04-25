namespace FileCabinetApp
{
    /// <summary>
    /// Validate first name by default ruleset.
    /// </summary>
    internal class DefaultFirstNameValidator
    {
        /// <summary>
        /// Validates first name.
        /// </summary>
        /// <param name="person">Personal data.</param>
        /// <exception cref="ArgumentException">Errors with name.</exception>
        public static void ValidateParameters(Person person)
        {
            int minNameLength = 2, maxNameLength = 60;

            if (string.IsNullOrWhiteSpace(person.FirstName) || person.FirstName.Length < minNameLength || person.FirstName.Length > maxNameLength)
            {
                throw new ArgumentException("wrong first name", nameof(person));
            }
        }
    }
}
