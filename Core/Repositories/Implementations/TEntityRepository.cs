using Core.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Repositories.Implementations
{
    public class TEntityRepository<T, Context> : ITEntityRepository<T>
        where T : class, new()
        where Context : IdentityDbContext<TUser>
    {
        private readonly Context _db;
        public TEntityRepository(Context db)
        {
            _db = db;
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> exp, params string[]? includes)
        {
            IQueryable<T> Querry = _db.Set<T>().Where(exp);
            if (includes is not null)
            {
                foreach (string name in includes)
                {
                    Querry.Include(name);
                }
            }
            return await Querry.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        => await _db.Set<T>().FindAsync(id);



        public async Task CreateAsync(T model)
        {
            await _db.Set<T>().AddAsync(model);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
