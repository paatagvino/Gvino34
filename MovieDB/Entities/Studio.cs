namespace MovieDB.Entities
{
    public class Studio
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // Foreign Key
        public int CountryId { get; set; }

        // Navigation Property
        public Country Country { get; set; } = null!;

        // One-to-One
        public StudioDetails StudioDetails { get; set; } = null!;

        // One-to-Many
        public ICollection<Movie> Movies { get; set; }
            = new List<Movie>();
    }
}