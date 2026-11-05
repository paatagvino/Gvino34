namespace MovieDB.DTOs
{
    public class UpdateMovieDto
    {
        public string Title { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public int StudioId { get; set; }
    }
}