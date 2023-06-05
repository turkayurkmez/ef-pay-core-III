namespace Movies.DataApplication.Models
{
    public class Director
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Info { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
        //spielberg.Movies.Add(et);
    }
}