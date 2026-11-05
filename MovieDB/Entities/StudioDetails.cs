namespace MovieDB.Entities
{
    public class StudioDetails
    {
        public int Id { get; set; }

        public string LicenseNumber { get; set; } = string.Empty;

        // Foreign Key
        public int StudioId { get; set; }

        // Navigation Property
        public Studio Studio { get; set; } = null!;
    }
}