using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using movie.Areas.Admin.Data;
using System.Linq.Expressions;

namespace ECommerce.Repositories
{
    public class Repository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>>? filter = null,
            bool tracked = false,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbSet;

            if (filter is not null)
                query = query.Where(filter);

            if (include is not null)
                query = include(query);

            if (!tracked)
                query = query.AsNoTracking();

            return await query.ToListAsync(cancellationToken);
        }


        public async Task<T?> GetOneAsync(
            Expression<Func<T, bool>> filter,
            bool tracked = false,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbSet;

            if (include is not null)
                query = include(query);

            if (!tracked)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(filter, cancellationToken);
        }

        
        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        
        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }


        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }

        internal async Task AddAsync(MovieSubImages movieSubImages, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
