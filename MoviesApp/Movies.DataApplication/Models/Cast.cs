namespace Movies.DataApplication.Models
{
    public class Cast
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Info { get; set; }

        public virtual ICollection<MovieCast>? Movies { get; set; }
    }
}