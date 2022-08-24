using System.Collections.ObjectModel;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Print records in tabular style.
    /// </summary>
    internal static class TabularOutput
    {
        /// <summary>
        /// Print records.
        /// </summary>
        /// <param name="fields">Designates output fields.</param>
        /// <param name="records">Records to output.</param>
        public static void SelectPrinter(bool[] fields, ReadOnlyCollection<FileCabinetRecord> records)
        {
            // Need to optimize
            Func<FileCabinetRecord, string>[] printRecordsPart = new Func<FileCabinetRecord, string>[]
            {
                record => record.Id.ToString(CultureInfo.InvariantCulture),
                record => record.FirstName,
                record => record.LastName,
                record => record.DateString,
                record => record.Income.ToString(CultureInfo.InvariantCulture),
                record => record.Tax.ToString(CultureInfo.InvariantCulture),
                record => record.StringBlock,
            };

            int[] lengths = PrintHeader(fields, records);
            int[] numericTypesIndexes = { 0, 4, 5 };

            foreach (var record in records)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    bool leftAlignment = fields[i];
                    bool rightAlignment = fields[i] && numericTypesIndexes.Contains(i);

                    if (rightAlignment)
                    {
                        string field = printRecordsPart[i](record).PadLeft(lengths[i]);
                        Console.Write(" | " + field);
                    }
                    else if (leftAlignment)
                    {
                        string field = printRecordsPart[i](record).PadRight(lengths[i]);
                        Console.Write(" | " + field);
                    }
                }

                Console.Write(" |" + Environment.NewLine);
            }

            PrintDividingLine(lengths, fields);
        }

        /// <summary>
        /// Print head of the table for select command.
        /// </summary>
        /// <param name="fields">Array of bool value, index mean records field.</param>
        /// <param name="records">Help to find the optimal column size.</param>
        private static int[] PrintHeader(bool[] fields, ReadOnlyCollection<FileCabinetRecord> records)
        {
            int[] lengths = new int[]
            {
                records.Max(rec => rec.Id.ToString(CultureInfo.InvariantCulture).Length),
                records.Max(rec => rec.FirstName.Length),
                records.Max(rec => rec.LastName.Length),
                records[0].DateString.Length,
                records.Max(rec => rec.Income.ToString(CultureInfo.InvariantCulture).Length),
                records.Max(rec => rec.Tax.ToString(CultureInfo.InvariantCulture).Length),
                records[0].StringBlock.Length,
            };

            string[] names = { "Id", "Firstname", "Lastname", "DateOfBirth", "Income", "Tax", "Block" };

            for (int i = 0; i < lengths.Length; i++)
            {
                lengths[i] = lengths[i] > names[i].Length ? lengths[i] : names[i].Length;
            }

            PrintDividingLine(lengths, fields);

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i])
                {
                    int space = (lengths[i] - names[i].Length) / 2;
                    int leftPadding = space + names[i].Length;
                    int rightPadding = leftPadding + space;
                    rightPadding = lengths[i] - names[i].Length == space * 2 ? rightPadding : rightPadding + 1;
                    Console.Write(" | " + names[i].PadLeft(leftPadding).PadRight(rightPadding));
                }
            }

            Console.Write(" |" + Environment.NewLine);
            PrintDividingLine(lengths, fields);
            return lengths;
        }

        /// <summary>
        /// Print dividing line between rows in tabular output.
        /// </summary>
        /// <param name="lengths">Column widths.</param>
        private static void PrintDividingLine(int[] lengths, bool[] fields)
        {
            Console.Write(" +");

            for (int i = 0; i < lengths.Length; i++)
            {
                if (fields[i])
                {
                    int separatorLength = 3;
                    int length = lengths[i] + separatorLength;
                    Console.Write("+".PadLeft(length, '-'));
                }
            }

            Console.WriteLine();
        }
    }
}
