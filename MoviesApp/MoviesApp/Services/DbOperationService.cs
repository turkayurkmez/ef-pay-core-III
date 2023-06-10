using Microsoft.EntityFrameworkCore;

namespace MoviesApp.Services
{
    public class DbOperationService
    {
        private readonly DbContext dbContext;


        public DbOperationService(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Create<T>(T entity) where T : class
        {
            DbSet<T> entities = dbContext.Set<T>();
            await entities.AddAsync(entity);
            await dbContext.SaveChangesAsync();


        }

        public async Task Update<T>(T entity) where T : class
        {
            DbSet<T> entities = dbContext.Set<T>();

            entities.Entry(entity).State = EntityState.Modified;

            await dbContext.SaveChangesAsync();

            //Multi tenancy bir uygulamada; kullanılabilir.
        }

    }
}
