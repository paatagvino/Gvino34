namespace MovieDB.Entities
{
    public class Actor
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        // Many-to-Many
        public ICollection<Movie> Movies { get; set; }
            = new List<Movie>();
    }
}