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
        int CreateRecord(Person person, short income, decimal tax, char block) => throw new NotImplementedException();

        /// <summary>
        /// Select records by input parameters.
        /// </summary>
        /// <param name="fields">Select these records fields.</param>
        /// <param name="parameters">Records parameters.</param>
        /// <returns>Record collection.</returns>
        ReadOnlyCollection<FileCabinetRecord> SelectRecord(bool[] fields, string[] parameters) => throw new NotImplementedException();

        /// <summary>
        /// Insert record to the file cabinet service.
        /// </summary>
        /// <param name="id">Person's id.</param>
        /// <param name="person">Personal data.</param>
        /// <param name="income">Person's new income.</param>
        /// <param name="tax">Person's new tax.</param>
        /// <param name="block">Person's new living block.</param>
        void InsertRecord(int id, Person person, short income, decimal tax, char block) => throw new NotImplementedException();

        /// <summary>
        /// Update record by input parameters.
        /// </summary>
        /// <param name="oldRecordParameters">Person's old data.</param>
        /// <param name="newRecordParameters">Personal new data.</param>
        void UpdateRecords(string[] oldRecordParameters, string[] newRecordParameters) => throw new NotImplementedException();

        /// <summary>
        /// Delete record by parameter name.
        /// </summary>
        /// <param name="fieldName">Record parameter.</param>
        /// <param name="value">Parameter value.</param>
        public void DeleteRecord(string fieldName, string value) => throw new NotImplementedException();

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
        int Purge()
        {
            return 0;
        }

        /// <summary>
        /// Get array of records.
        /// </summary>
        /// <returns>Collection of records.</returns>
        ReadOnlyCollection<FileCabinetRecord> GetRecords() => throw new NotImplementedException();

        /// <summary>
        /// Get records count.
        /// </summary>
        /// <returns>Count of record and count of removed records.</returns>
        Tuple<int, int> GetStat() => throw new NotImplementedException();
    }
}