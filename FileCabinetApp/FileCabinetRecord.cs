namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public short Income { get; set; }

        public decimal Tax { get; set; }

        public char Block { get; set; }

        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"#{this.Id}, ");
            builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"{this.FirstName}, ");
            builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"{this.LastName}, ");
            builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"{this.DateOfBirth.ToString("yyyy-MMM-dd", System.Globalization.CultureInfo.InvariantCulture)}, ");
            builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"{this.Income}, ");
            builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"{this.Tax}, ");
            builder.Append(System.Globalization.CultureInfo.InvariantCulture, $"{this.Block}");
            return builder.ToString();
        }
    }
}