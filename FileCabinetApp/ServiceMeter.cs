using System.Collections.ObjectModel;
using System.Diagnostics;

namespace FileCabinetApp
{
    /// <summary>
    /// Decorator for service object
    /// that measure execution time.
    /// </summary>
    public class ServiceMeter : IFileCabinetService
    {
        private IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMeter"/> class.
        /// </summary>
        /// <param name="service">Instance of FileCabinetService class.</param>
        public ServiceMeter(IFileCabinetService service)
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
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var id = this.service.CreateRecord(person, income, tax, block);
            stopwatch.Stop();
            Console.WriteLine($"Create method execution duration is {stopwatch.ElapsedTicks} ticks");
            return id;
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
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.service.EditRecord(id, person, income, tax, block);
            stopwatch.Stop();
            Console.WriteLine($"Edit method execution duration is {stopwatch.ElapsedTicks} ticks");
        }

        /// <summary>
        /// Remove record by id.
        /// </summary>
        /// <param name="id">Person's id.</param>
        public void RemoveRecord(int id)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.service.RemoveRecord(id);
            stopwatch.Stop();
            Console.WriteLine($"Remove method execution duration is {stopwatch.ElapsedTicks} ticks");
        }

        /// <summary>
        /// Find persons by first name.
        /// </summary>
        /// <param name="firstName">Person's first name.</param>
        /// <returns>Array of person with same first name.</returns>
        public IRecordIterator FindByFirstName(string firstName)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var firstNameList = this.service.FindByFirstName(firstName);
            stopwatch.Stop();
            Console.WriteLine($"Find by first name method execution duration is {stopwatch.ElapsedTicks} ticks");
            return firstNameList;
        }

        /// <summary>
        /// Find persons by last name.
        /// </summary>
        /// <param name="lastName">Person's last name.</param>
        /// <returns>Array of person with same last name.</returns>
        public IRecordIterator FindByLastName(string lastName)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var lastNameList = this.service.FindByLastName(lastName);
            stopwatch.Stop();
            Console.WriteLine($"Find by last name method execution duration is {stopwatch.ElapsedTicks} ticks");
            return lastNameList;
        }

        /// <summary>
        /// Find persons by date of birth.
        /// </summary>
        /// <param name="strDateOfBirth">Person's date.</param>
        /// <returns>Array of person with same date of birth.</returns>
        public IRecordIterator FindByDateofbirth(string strDateOfBirth)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var dateOfBirthList = this.service.FindByDateofbirth(strDateOfBirth);
            stopwatch.Stop();
            Console.WriteLine($"Find by date of birth method execution duration is {stopwatch.ElapsedTicks} ticks");
            return dateOfBirthList;
        }

        /// <summary>
        /// Make snapshot of the current list of records.
        /// </summary>
        /// <returns>Array of person with same date of birth.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var snapshot = this.service.MakeSnapshot();
            stopwatch.Stop();
            Console.WriteLine($"Make snapshot method execution duration is {stopwatch.ElapsedTicks} ticks");
            return snapshot;
        }

        /// <summary>
        /// Restore records.
        /// </summary>
        /// <param name="snapshot">Records snapshot.</param>
        /// <returns>Counts of imported records.</returns>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var restore = this.service.Restore(snapshot);
            stopwatch.Stop();
            Console.WriteLine($"Restore method execution duration is {stopwatch.ElapsedTicks} ticks");
            return restore;
        }

        /// <summary>
        /// Purge records.
        /// </summary>
        /// <returns>Count of purged records.</returns>
        public int Purge()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var purgeCount = this.service.Purge();
            stopwatch.Stop();
            Console.WriteLine($"Purge method execution duration is {stopwatch.ElapsedTicks} ticks");
            return purgeCount;
        }

        /// <summary>
        /// Get array of records.
        /// </summary>
        /// <returns>Collection of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var list = this.service.GetRecords();
            stopwatch.Stop();
            Console.WriteLine($"Get records method execution duration is {stopwatch.ElapsedTicks} ticks");
            return list;
        }

        /// <summary>
        /// Get records count.
        /// </summary>
        /// <returns>Count of record and count of removed records.</returns>
        public Tuple<int, int> GetStat()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var counts = this.service.GetStat();
            stopwatch.Stop();
            Console.WriteLine($"Get stat method execution duration is {stopwatch.ElapsedTicks} ticks");
            return counts;
        }
    }
}
