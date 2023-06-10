namespace Movies.DataApplication.Models
{
    //İsterseniz; model üzerinden de konfigürasyon dosyasına yönlendirebilirsiniz.
    //[EntityTypeConfiguration(typeof(GenreEntityTypeConfiguration))]
    public class Genre : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
    }
}