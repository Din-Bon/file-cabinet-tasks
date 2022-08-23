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
        /// Select records by input parameters.
        /// </summary>
        /// <param name="fields">Select these records fields.</param>
        /// <param name="parameters">Records parameters.</param>
        public void SelectRecord(bool[] fields, string[] parameters)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.service.SelectRecord(fields, parameters);
            stopwatch.Stop();
            Console.WriteLine($"Select method execution duration is {stopwatch.ElapsedTicks} ticks");
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
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.service.InsertRecord(id, person, income, tax, block);
            stopwatch.Stop();
            Console.WriteLine($"Insert method execution duration is {stopwatch.ElapsedTicks} ticks");
        }

        /// <summary>
        /// Update record by input parameters.
        /// </summary>
        /// <param name="oldRecordParameters">Person's old data.</param>
        /// <param name="newRecordParameters">Personal new data.</param>
        public void UpdateRecords(string[] oldRecordParameters, string[] newRecordParameters)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.service.UpdateRecords(oldRecordParameters, newRecordParameters);
            stopwatch.Stop();
            Console.WriteLine($"Update method execution duration is {stopwatch.ElapsedTicks} ticks");
        }

        /// <summary>
        /// Delete record by parameter name.
        /// </summary>
        /// <param name="fieldName">Record parameter.</param>
        /// <param name="value">Parameter value.</param>
        public void DeleteRecord(string fieldName, string value)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.service.DeleteRecord(fieldName, value);
            stopwatch.Stop();
            Console.WriteLine($"Delete method execution duration is {stopwatch.ElapsedTicks} ticks");
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
