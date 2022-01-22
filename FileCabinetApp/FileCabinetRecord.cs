namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public short Income { get; set; }

        public decimal Tax { get; set; }

        public char Block { get; set; }
    }
}