using System.Collections.ObjectModel;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Decorator for service object
    /// that write log.
    /// </summary>
    public class ServiceLogger : IFileCabinetService
    {
        private const string FileName = "log.txt";
        private IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogger"/> class.
        /// </summary>
        /// <param name="service">Instance of FileCabinetService class.</param>
        public ServiceLogger(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Create record from the input parameters.
        /// </summary>
        /// <param name="person">Personal data.</param>
        /// <param name="income">Person's income.</param>
        /// <param name="tax">Person's tax.</param>
        /// <param name="block">Person's living block.</param>
        /// <returns>Records id.</returns>
        public int CreateRecord(Person person, short income, decimal tax, char block)
        {
            TextWriter writer = new StreamWriter(FileName, true);
            writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Calling Create() " +
                $"with FirstName = {person.FirstName}, LastName = {person.LastName}, DateOfBirth = {person.DateOfBirth}, " +
                $"Income = {income}, Tax = {tax}, Block = {block}.");

            try
            {
                var id = this.service.CreateRecord(person, income, tax, block);
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Create() returned '{id}'.");
                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create failed: {ex.Message}");
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - {ex.Message}");
                return -1;
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Select records by input parameters.
        /// </summary>
        /// <param name="fields">Select these records fields.</param>
        /// <param name="parameters">Records parameters.</param>
        public void SelectRecord(bool[] fields, string[] parameters)
        {
            TextWriter writer = new StreamWriter(FileName, true);
            writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Calling Select.");

            try
            {
                this.service.SelectRecord(fields, parameters);
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Select completed successfully'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Select failed: {ex.Message}");
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - {ex.Message}");
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Insert record from the input parameters.
        /// </summary>
        /// <param name="id">Person's id.</param>
        /// <param name="person">Personal data.</param>
        /// <param name="income">Person's new income.</param>
        /// <param name="tax">Person's new tax.</param>
        /// <param name="block">Person's new living block.</param>
        public void InsertRecord(int id, Person person, short income, decimal tax, char block)
        {
            TextWriter writer = new StreamWriter(FileName, true);
            writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Calling Insert() for record with id = {id}. Parameters - " +
                $"FirstName = {person.FirstName}, LastName = {person.LastName}, DateOfBirth = {person.DateOfBirth}, " +
                $"Income = {income}, Tax = {tax}, Block = {block}.");

            try
            {
                this.service.InsertRecord(id, person, income, tax, block);
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Insert() completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Insert failed: {ex.Message}");
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - {ex.Message}");
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Update record by input parameters.
        /// </summary>
        /// <param name="oldRecordParameters">Person's old data.</param>
        /// <param name="newRecordParameters">Personal new data.</param>
        public void UpdateRecords(string[] oldRecordParameters, string[] newRecordParameters)
        {
            TextWriter writer = new StreamWriter(FileName, true);
            writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Calling Update().");

            try
            {
                this.service.UpdateRecords(oldRecordParameters, newRecordParameters);
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Update() completed seccessfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update failed: {ex.Message}");
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - {ex.Message}");
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Delete record by parameter name.
        /// </summary>
        /// <param name="fieldName">Record parameter.</param>
        /// <param name="value">Parameter value.</param>
        public void DeleteRecord(string fieldName, string value)
        {
            TextWriter writer = new StreamWriter(FileName, true);
            writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Calling Delete() for record with parameter field = {fieldName}," +
                $"value = {value}");

            try
            {
                this.service.DeleteRecord(fieldName, value);
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Delete() completed seccessfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete failed: {ex.Message}");
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - {ex.Message}");
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Find persons by first name.
        /// </summary>
        /// <param name="firstName">Person's first name.</param>
        /// <returns>Array of person with same first name.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            TextWriter writer = new StreamWriter(FileName, true);
            writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Calling FindByFirstName() " +
                $"for record with firstName = {firstName}");

            try
            {
                var firstNameList = this.service.FindByFirstName(firstName);
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - FindByFirstName() returned '{firstNameList}'.");
                return firstNameList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FindByFirstName failed: {ex.Message}");
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - {ex.Message}");
                return null;
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Find persons by last name.
        /// </summary>
        /// <param name="lastName">Person's last name.</param>
        /// <returns>Array of person with same last name.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            TextWriter writer = new StreamWriter(FileName, true);
            writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Calling FindByLastName() " +
                $"for record with lastName = {lastName}");

            try
            {
                var lastNameList = this.service.FindByLastName(lastName);
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - FindByLastName() returned '{lastNameList}'.");
                return lastNameList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FindByLastName failed: {ex.Message}");
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - {ex.Message}");
                return null;
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Find persons by date of birth.
        /// </summary>
        /// <param name="strDateOfBirth">Person's date.</param>
        /// <returns>Array of person with same date of birth.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateofbirth(string strDateOfBirth)
        {
            TextWriter writer = new StreamWriter(FileName, true);
            writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Calling FindByDateOfBirth() " +
                $"for record with DateOfBirth = {strDateOfBirth}");

            try
            {
                var dateOfBirthList = this.service.FindByDateofbirth(strDateOfBirth);
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - FindByDateOfBirth() returned '{dateOfBirthList}'.");
                return dateOfBirthList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FindByDateofbirth failed: {ex.Message}");
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - {ex.Message}");
                return null;
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Make snapshot of the current list of records.
        /// </summary>
        /// <returns>Array of person with same date of birth.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            TextWriter writer = new StreamWriter(FileName, true);
            writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Calling MakeSnaphot().");

            try
            {
                var snapshot = this.service.MakeSnapshot();
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - MakeSnapshot() returned {snapshot}.");
                return snapshot;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MakeSnapshot failed: {ex.Message}");
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - {ex.Message}");
                return new FileCabinetServiceSnapshot(Array.Empty<FileCabinetRecord>());
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Restore records.
        /// </summary>
        /// <param name="snapshot">Records snapshot.</param>
        /// <returns>Counts of imported records.</returns>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            TextWriter writer = new StreamWriter(FileName, true);
            writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Calling Restore() for {snapshot}.");

            try
            {
                var restore = this.service.Restore(snapshot);
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Restore() returned {restore}.");
                return restore;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Restore failed: {ex.Message}");
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - {ex.Message}");
                return 0;
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Purge records.
        /// </summary>
        /// <returns>Count of purged records.</returns>
        public int Purge()
        {
            TextWriter writer = new StreamWriter(FileName, true);
            writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Calling Purge().");

            try
            {
                var purgeCount = this.service.Purge();
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Purge() returned {purgeCount}.");
                return purgeCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Purge failed: {ex.Message}");
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - {ex.Message}");
                return 0;
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Get array of records.
        /// </summary>
        /// <returns>Collection of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            TextWriter writer = new StreamWriter(FileName, true);
            writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Calling GetRecords().");

            try
            {
                var list = this.service.GetRecords();
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - GetRecords() returned {list}.");
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetRecords failed: {ex.Message}");
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - {ex.Message}");
                return new ReadOnlyCollection<FileCabinetRecord>(Array.Empty<FileCabinetRecord>());
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Get records count.
        /// </summary>
        /// <returns>Count of record and count of removed records.</returns>
        public Tuple<int, int> GetStat()
        {
            TextWriter writer = new StreamWriter(FileName, true);
            writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Calling GetStat().");

            try
            {
                var counts = this.service.GetStat();
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - GetStat() returned {counts}.");
                return counts;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetStat failed: {ex.Message}");
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - {ex.Message}");
                return new Tuple<int, int>(0, 0);
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }
        }
    }
}
