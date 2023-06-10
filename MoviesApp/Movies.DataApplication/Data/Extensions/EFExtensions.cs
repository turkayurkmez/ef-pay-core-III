using Microsoft.EntityFrameworkCore;
using Movies.DataApplication.Models;

namespace Movies.DataApplication.Data.Extensions
{
    public static class EFExtensions
    {
        public static IQueryable<T> Paginate<T>(this DbSet<T> entities, int currentPage, int pageSize) where T : class, IEntity
        {
            return entities.OrderBy(e => e.Id)
                     .Skip((currentPage - 1) * pageSize)
                     .Take(pageSize);
        }
    }
}
