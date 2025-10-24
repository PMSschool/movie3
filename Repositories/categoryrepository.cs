using Microsoft.EntityFrameworkCore;
using movie.Areas.Admin.Data;
using movie.Areas.Admin.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace movie.Repositories
{
    public class Categoryrepository
    {
        // CRUD
        ApplicationDbContext _context = new();

        public async Task AddAsync( Category category, CancellationToken cancellationToken = default)
        {
          await  _context.AddAsync(category , cancellationToken);
        }
        public void Update( Category category)
        {
            _context.Update(category);
        }
        public void Delete( Category category)
        {
            _context.Remove(category);
        }

        public async Task<IEnumerable<Category>> GetAsync(Expression<Func<Category, bool>>? expression, CancellationToken cancellationToken = default)
        {
           var categories =  _context.categories.AsQueryable();

            if(expression is not null)
                  categories = categories.Where(expression);
            return await categories.ToListAsync(cancellationToken);
        }
        public async Task<Category?> GetOneAsync(Expression<Func<Category, bool>>expression, CancellationToken cancellationToken = default)
        {
            return (await GetAsync(expression, cancellationToken)).FirstOrDefault();
        }

        public async Task ComitAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }catch(Exception ex)
            {
                Console.WriteLine($"error : {ex.Message}");
            }
        }

        internal async Task GetAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
