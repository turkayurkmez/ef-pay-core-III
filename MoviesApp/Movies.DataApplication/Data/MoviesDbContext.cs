using Microsoft.EntityFrameworkCore;
using Movies.DataApplication.Models;
using Movies.DataApplication.Models.Configurations;

namespace Movies.DataApplication.Data
{
    public class MoviesDbContext : DbContext
    {
        public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options)
        {

        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Cast> Casts { get; set; }
        public DbSet<Genre> Genres { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().Property(p => p.Name).IsRequired()
                                                              .HasMaxLength(100);

            modelBuilder.Entity<Movie>()
                        .Property(m => m.MovieFormat)
                        .HasConversion(
                           x => x.ToString(),
                           x => (MovieFormat)Enum.Parse(typeof(MovieFormat), x)
                           );

            new GenreEntityTypeConfiguration().Configure(modelBuilder.Entity<Genre>());
            //modelBuilder.ApplyConfiguration<Genre>(new GenreEntityTypeConfiguration()); 



            modelBuilder.Entity<MovieCast>().HasKey("MovieId", "CastId");
            modelBuilder.Entity<Movie>().HasMany(m => m.Casts)
                                        .WithOne(cm => cm.Movie)
                                        .HasForeignKey(m => m.MovieId)
                                        .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Cast>().HasMany(c => c.Movies)
                                       .WithOne(cm => cm.Cast)
                                       .HasForeignKey(cm => cm.CastId)
                                       .OnDelete(DeleteBehavior.NoAction);


            var bradPitt = new Cast { Id = 1, Name = "Brad Pitt" };
            modelBuilder.Entity<Cast>().HasData(
                   bradPitt
                );

            var movieCast = new MovieCast { CastId = 1, MovieId = 1, Role = "Akıl hastanesindeki aktivist" };

            modelBuilder.Entity<MovieCast>().HasData(movieCast);

            //var casts = new List<MovieCast>() { movieCast };

            modelBuilder.Entity<Movie>().HasData(new Movie { Id = 1, Name = "12 Maymun" });


        }
    }
}
