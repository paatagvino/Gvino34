namespace MovieDB.Entities
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // One-to-Many
        public ICollection<Studio> Studios { get; set; }
            = new List<Studio>();
    }
}