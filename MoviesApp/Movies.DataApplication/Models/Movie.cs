using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.DataApplication.Models
{
    //[Table("Filmler")]
    public class Movie : IEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public string? Summary { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }
        public double? Rating { get; set; }

        public bool IsDeleted { get; set; }

        public MovieFormat MovieFormat { get; set; }

        public int? DirectorId { get; set; }

        //Navigation Property:
        public virtual Director? Director { get; set; }

        public virtual ICollection<MovieCast>? Casts { get; set; }

        public virtual ICollection<Genre>? Genres { get; set; }

        //[NotMapped]
        //public bool IsAvailable { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        [NotMapped]
        public byte[]? RowVersion { get; set; }

    }


    public enum MovieFormat
    {
        DVD,
        Stream,
        Blueray,
        TheatreOnly
    }
}
