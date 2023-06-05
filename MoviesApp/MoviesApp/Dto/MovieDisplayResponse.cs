using Movies.DataApplication.Models;

namespace MoviesApp.Dto
{
    public class MovieDisplayResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Summary { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public double? Rating { get; set; }
        public MovieFormat MovieFormat { get; set; }
        public string DirectorName { get; set; }
        public string Casts { get; set; }
        public string Genres { get; set; }


    }
}
