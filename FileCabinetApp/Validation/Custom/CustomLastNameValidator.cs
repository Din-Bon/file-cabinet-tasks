namespace FileCabinetApp
{
    /// <summary>
    /// Validate last name by custom ruleset.
    /// </summary>
    internal class CustomLastNameValidator
    {
        /// <summary>
        /// Validate last name.
        /// </summary>
        /// <param name="person">Personal data.</param>
        /// <exception cref="ArgumentException">Errors with name.</exception>
        public static void ValidateParameters(Person person)
        {
            int minNameLength = 2, maxNameLength = 60;

            if (string.IsNullOrWhiteSpace(person.LastName) || person.LastName.Length < minNameLength || person.LastName.Length > maxNameLength)
            {
                throw new ArgumentException("wrong last name", nameof(person));
            }
        }
    }
}
