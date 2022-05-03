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
        /// Create record from the input parameters.
        /// </summary>
        /// <param name="id">Person's id.</param>
        /// <param name="person">Personal data.</param>
        /// <param name="income">Person's new income.</param>
        /// <param name="tax">Person's new tax.</param>
        /// <param name="block">Person's new living block.</param>
        public void EditRecord(int id, Person person, short income, decimal tax, char block)
        {
            TextWriter writer = new StreamWriter(FileName, true);
            writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Calling Edit() for record with id = {id}. New parameters - " +
                $"FirstName = {person.FirstName}, LastName = {person.LastName}, DateOfBirth = {person.DateOfBirth}, " +
                $"Income = {income}, Tax = {tax}, Block = {block}.");

            try
            {
                this.service.EditRecord(id, person, income, tax, block);
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Edit() completed seccessfully.");
            }
            catch (Exception ex)
            {
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - {ex.Message}");
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Remove record by id.
        /// </summary>
        /// <param name="id">Person's id.</param>
        public void RemoveRecord(int id)
        {
            TextWriter writer = new StreamWriter(FileName, true);
            writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Calling Remove() for record with id = {id}");

            try
            {
                this.service.RemoveRecord(id);
                writer.WriteLine($"{DateTime.Now.ToString("g", CultureInfo.InvariantCulture)} - Remove() completed seccessfully.");
            }
            catch (Exception ex)
            {
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
