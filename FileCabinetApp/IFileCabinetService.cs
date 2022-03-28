using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// File cabinet service interface.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Create record from the input parameters.
        /// </summary>
        /// <param name="person">Personal data.</param>
        /// <param name="income">Person's income.</param>
        /// <param name="tax">Person's tax.</param>
        /// <param name="block">Person's living block.</param>
        /// <returns>Records id.</returns>
        int CreateRecord(Person person, short income, decimal tax, char block);

        /// <summary>
        /// Create record from the input parameters.
        /// </summary>
        /// <param name="id">Person's id.</param>
        /// <param name="person">Personal data.</param>
        /// <param name="income">Person's new income.</param>
        /// <param name="tax">Person's new tax.</param>
        /// <param name="block">Person's new living block.</param>
        void EditRecord(int id, Person person, short income, decimal tax, char block);

        /// <summary>
        /// Remove record by id.
        /// </summary>
        /// <param name="id">Person's id.</param>
        void RemoveRecord(int id);

        /// <summary>
        /// Find persons by first name.
        /// </summary>
        /// <param name="firstName">Person's first name.</param>
        /// <returns>Array of person with same first name.</returns>
        ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Find persons by last name.
        /// </summary>
        /// <param name="lastName">Person's last name.</param>
        /// <returns>Array of person with same last name.</returns>
        ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Find persons by date of birth.
        /// </summary>
        /// <param name="strDateOfBirth">Person's date.</param>
        /// <returns>Array of person with same date of birth.</returns>
        ReadOnlyCollection<FileCabinetRecord> FindByDateofbirth(string strDateOfBirth);

        /// <summary>
        /// Make snapshot of the current list of records.
        /// </summary>
        /// <returns>Array of person with same date of birth.</returns>
        FileCabinetServiceSnapshot MakeSnapshot() => throw new NotImplementedException();

        /// <summary>
        /// Restore records.
        /// </summary>
        /// <param name="snapshot">Records snapshot.</param>
        /// <returns>Counts of imported records.</returns>
        int Restore(FileCabinetServiceSnapshot snapshot) => throw new NotImplementedException();

        /// <summary>
        /// Purge records.
        /// </summary>
        /// <returns>Count of purged records.</returns>
        int Purge() => throw new NotImplementedException();

        /// <summary>
        /// Get array of records.
        /// </summary>
        /// <returns>Collection of records.</returns>
        ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Get records count.
        /// </summary>
        /// <returns>Count of record and count of removed records.</returns>
        Tuple<int, int> GetStat();
    }
}