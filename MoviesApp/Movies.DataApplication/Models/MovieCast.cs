namespace Movies.DataApplication.Models
{
    public class MovieCast
    {
        public int MovieId { get; set; }
        public int CastId { get; set; }

        public string? Role { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Cast Cast { get; set; }

    }
}
