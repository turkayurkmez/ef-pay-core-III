using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Movies.DataApplication.Data;
using Movies.DataApplication.Data.Extensions;

using Movies.DataApplication.Models;
using MoviesApp.Dto;
using MoviesApp.Services;

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
        [HttpGet("[action]/{page}")]
        public IActionResult Pagination(int page)
        {
            int perPage = 3;
            /*
             * 1. sayfa: 0 atla 2 film göster
             * 2. sayfa: 2 atla 2 göster
             * 3. sayfa: 4 atla.....
             */
            var paginated = _context.Movies.Paginate(page, perPage);
            var pageinateDirector = _context.Directors.Paginate(page, perPage).TagWith("!!!Yönetmenleri sayfalıyoruz!!!");
            //skip and take alternatifi:
            //Skip yerine Where kriteri ile hangi satırdan sonrasını alacağını belirtebilirsiniz:
            //int lastId = 3;
            //var result = _context.Movies.OrderBy(m => m.Id)
            //                            .Where(m => m.Id > lastId)
            //                            .Take(perPage)
            //                            .ToList();

            return Ok(pageinateDirector);
        }
        [HttpGet("[action]")]
        public IActionResult WithSQLQuery()
        {

            //"SELECT c.Id, c.Name,c.Info, Count(m.Id) FROM Casts c JOIN MovieCast mc On c.Id = mc.CastId JOIN Movies m ON m.Id = mc.MovieId GROUP BY c.Name,c.Id, c.Info
            var id = 2;

            var casts = _context.Casts.FromSql($"SELECT * FROM Casts Where Id = {id}");
            //FromSql alternatifi parametreyi SqlParameter olarak tanımlamanızı gerektirir
            var sqlParameter = new SqlParameter("@id", id);
            var castsAlternate = _context.Casts
                .FromSqlRaw($"SELECT * FROM Casts Where Id = @id", sqlParameter)
                .Include(c => c.Movies)
                    .ThenInclude(cm => cm.Movie).ToList();
            return Ok(castsAlternate);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewMovie()
        {
            Movie movie = new Movie
            {
                Name = "Close Encounters of the third kind ",
                Director = new Director { Name = "Stephen Spielberg" },
                Genres = new List<Genre> { new Genre { Name = "Uzaylı Bilim Kurgu" } },
                IsDeleted = false,
                MovieFormat = MovieFormat.Blueray,
                ReleaseDate = new DateTime(1977, 1, 1),
                Summary = "Uzaylılar ile yakın temas",
                Casts = new List<MovieCast> { new MovieCast { Cast = new Cast { Name = "Richard Dreyfuss" } } },
                Rating = 9.2,
                Comments = new List<Comment> { new Comment { CommentBody = "En iyi bilim-kurgulardan biri...." } }

            };
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            return Ok();

        }
        [HttpPut]
        public async Task<IActionResult> UpdateFormat()
        {
            await _context.Movies.Where(m => m.Rating > 5).ExecuteUpdateAsync(x => x.SetProperty(m => m.MovieFormat, MovieFormat.Blueray));
            return Ok();
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateRating()
        {
            //var movie = _context.Movies.AsNoTracking().Single(m => m.Id == 5);
            var movie = new Movie
            {
                Rating = 7,
                Summary = "Ne bileyim ben",
                IsDeleted = false,
                MovieFormat = MovieFormat.Stream,
                Name = "xxx"

            };
            //movie.Rating = 7;
            //movie.Summary = "Ne bileyim ben";

            _context.Movies.Update(movie);
            //_context.Entry(movie).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateRatingWithService()
        {
            var movie = _context.Movies.AsNoTracking().Single(m => m.Id == 1);
            movie.Rating = 10;
            movie.Summary = "Ne bileyim ben";

            DbOperationService service = new DbOperationService(_context);

            //_context.Entry(movie).State = EntityState.Modified;

            await service.Update(movie);
            return Ok();
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> CreateNewMovieWithTran()
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var director = new Director { Name = "Başka bir yönetmen" };
                await _context.Directors.AddAsync(director);
                await _context.SaveChangesAsync();

                ///transaction.CreateSavepoint("AfterDirectorCreated");

                await _context.Movies.AddAsync(new Movie { Name = "Başka bir film", Director = director, IsDeleted = false, MovieFormat = MovieFormat.TheatreOnly });
                await _context.SaveChangesAsync();

                var movie = _context.Movies.FirstOrDefault(m => m.Id == 1);
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();


                await transaction.CommitAsync();


            }
            catch (Exception)
            {
                transaction.Rollback();
                //transaction.RollbackToSavepoint("AfterDirectorCreated");

            }

            return Ok();
        }

        private static string getDataForRating(double? rating)
        {
            return $"Bu filmin puanı: {rating} ";
        }
    }
}
