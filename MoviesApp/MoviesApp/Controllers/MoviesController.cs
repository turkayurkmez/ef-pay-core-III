using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.DataApplication.Data;
using Movies.DataApplication.Models;
using MoviesApp.Dto;

namespace MoviesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        private readonly MoviesDbContext _context;

        public MoviesController(MoviesDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetMovies()
        {
            var movies = _context.Movies.ToList();
            //movies[0].MovieFormat = MovieFormat.TheatreOnly;
            //await _context.SaveChangesAsync();
            return Ok(movies);

        }
        [HttpGet("[action]")]
        public async Task<IActionResult> ChangeMovieFormat()
        {
            //Change Tracking'i kapatmak için AsNoTracking() extension metodu kullanılır
            //var movie = _context.Movies.AsNoTracking().FirstOrDefault(m => m.Id == 1);
            var movie = _context.Movies.FirstOrDefault(m => m.Id == 1);
            movie.MovieFormat = MovieFormat.TheatreOnly;
            await _context.SaveChangesAsync();
            return Ok(movie);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> IdentityResolution()
        {
            //Change Tracking'i kapatmak için AsNoTracking() extension metodu kullanılır
            //var movie = _context.Movies.AsNoTracking().FirstOrDefault(m => m.Id == 1);
            var movie = _context.Movies.AsNoTrackingWithIdentityResolution();
            //movie.MovieFormat = MovieFormat.TheatreOnly;
            await _context.SaveChangesAsync();
            return Ok(movie);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ChangeTrackingBehaviour()
        {
            //Tüm ChangeTracking davranışı NoTracking olarak konfigüre edilmiş ise

            var movies = _context.Movies.AsTracking().ToList();
            movies[0].Rating = 8.6;
            await _context.SaveChangesAsync();
            return Ok(movies);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ClientAndServer()
        {
            //Tüm ChangeTracking davranışı NoTracking olarak konfigüre edilmiş ise

            var movies = _context.Movies.Select(m => new
            {
                Name = m.Name.ToUpper(),
                m.Summary,
                m.ReleaseDate,
                Puan = getDataForRating(m.Rating)
            })
            .AsEnumerable()
            .Where(m => m.Puan.Contains("puan"))
            .ToList();



            return Ok(movies);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> EagerLoding()
        {
            var movies = _context.Movies.AsTracking()
                                        .Include(m => m.Genres)
                                        .Include(m => m.Director)
                                        .Include(m => m.Casts)
                                           .ThenInclude(c => c.Cast)
                                        .ToList();
            return Ok(movies);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ExplicitLoading()
        {
            var movie = _context.Movies.AsTracking().Single(m => m.Id == 2);

            //Explicit Loading ile objeyi yükledikten SONRA İLGİLİ ÖZELLİKLERİ ilişkili tablodan çekebilirsiniz.
            if (movie != null)
            {
                _context.Entry(movie).Collection(m => m.Genres).Load();
                _context.Entry(movie).Reference(m => m.Director).Load();
            }



            return Ok(movie);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> LazyLoading()
        {
            var movies = _context.Movies.AsTracking().ToList();
            return Ok(movies);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetMoviesWithDto()
        {
            var movies = _context.Movies.Include(m => m.Director)
                                        .Include(m => m.Genres)
                                        .Include(m => m.Casts)
                                        .ThenInclude(mc => mc.Cast)
                                        .Select(m => new MovieDisplayResponse
                                        {
                                            Id = m.Id,
                                            Name = m.Name,
                                            MovieFormat = m.MovieFormat,
                                            ReleaseDate = m.ReleaseDate,
                                            Summary = m.Summary,
                                            Rating = m.Rating,
                                            DirectorName = $"{m.Director.Name}",
                                            Genres = string.Join('-', m.Genres.Select(g => g.Name)),
                                            Casts = string.Join(',', m.Casts.Select(c => $"{c.Cast.Name} ({c.Role}) "))

                                        });
            return Ok(movies);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> SplittingQuery()
        {
            var result = _context.Movies.Include(m => m.Comments)
                                         .AsSplitQuery()
                                         .Include(m => m.Genres)
                                         .AsSplitQuery()
                                         .ToList();

            return Ok(result);

        }

        [HttpGet("[action]")]
        public async Task<IActionResult> JoinWithLinq()
        {
            //var query = from movie in _context.Movies
            //            join comment in _context.Set<Comment>()
            //            on movie.Id equals comment.MovieId
            //            select new { movie.Name, comment.CommentBody };

            // var comments = _context.Set<Comment>();
            //var query = _context.Movies.Join(
            //     _context.Directors,
            //     movie => movie.DirectorId,
            //     director => director.Id,
            //     (movie, director) => new { MovieTitle = movie.Name, DirectorName = director.Name }
            //    );

            var query = from movie in _context.Movies
                        join comments in _context.Comment
                        on movie.Id equals comments.MovieId into grouping
                        select new { Name = movie.Name, CommentsCount = grouping.Count() };


            return Ok(query);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GroupByWithLinq()
        {
            //var query = from movie in _context.Movies
            //            join comment in _context.Set<Comment>()
            //            on movie.Id equals comment.MovieId
            //            select new { movie.Name, comment.CommentBody };

            // var comments = _context.Set<Comment>();
            //var query = _context.Movies.Join(
            //     _context.Directors,
            //     movie => movie.DirectorId,
            //     director => director.Id,
            //     (movie, director) => new { MovieTitle = movie.Name, DirectorName = director.Name }
            //    );

            var query = _context.Movies.Join(_context.Comment, m => m.Id, c => c.MovieId, (movie, comment) => new { movie.Name, comment.CommentBody })

                .GroupBy(
                 gr => gr.Name,
                 movie => new { Name = movie.Name, movie.CommentBody },
                 (key, collection) => new { key, TotalCount = collection.Count() }

                );

            return Ok(query);
        }

        private static string getDataForRating(double? rating)
        {
            return $"Bu filmin puanı: {rating} ";
        }
    }
}
