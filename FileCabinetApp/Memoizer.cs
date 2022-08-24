using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Memoize data from search methods.
    /// </summary>
    public static class Memoizer
    {
        private static Dictionary<string[], ReadOnlyCollection<FileCabinetRecord>> cache = new Dictionary<string[], ReadOnlyCollection<FileCabinetRecord>>();

        /// <summary>
        /// Memoize records from select function.
        /// </summary>
        /// <param name="fields">Fields to return.</param>
        /// <param name="input">Records parameters that we select.</param>
        /// <param name="fileCabinetService">Can get records from the current filecabinetservice object.</param>
        public static void MemoizeSelect(bool[] fields, string[] input, IFileCabinetService fileCabinetService)
        {
            ReadOnlyCollection<FileCabinetRecord>? records = null;

            foreach (var key in cache.Keys)
            {
                if (Enumerable.SequenceEqual(input, key))
                {
                    records = cache[key];
                    TabularOutput.SelectPrinter(fields, records);
                }
            }

            if (records == null)
            {
                records = fileCabinetService.SelectRecord(fields, input);
                cache.Add(input, records);
                TabularOutput.SelectPrinter(fields, records);
            }
        }
    }
}
