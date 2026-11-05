namespace MovieDB.Entities
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public int ReleaseYear { get; set; }

        // Foreign Key
        public int StudioId { get; set; }

        // Navigation Property
        public Studio Studio { get; set; } = null!;

        // Many-to-Many
        public ICollection<Actor> Actors { get; set; }
            = new List<Actor>();
    }
}