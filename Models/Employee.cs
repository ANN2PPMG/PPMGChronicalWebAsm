namespace PPMGChronicalWebAsm.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; } 
        public string Category { get; set; }
        public string? AcademicYear { get; set; }
        public string? Status { get; set; }
        public string? Picture { get; set; }

    }
}
