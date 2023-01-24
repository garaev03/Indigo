using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories.Interfaces
{
    public interface ITEntityRepository<T>
        where T : class, new()
    {
        Task<List<T>> GetAllAsync(Expression<Func<T,bool>> exp,params string[]? includes);
        Task<T> GetByIdAsync(int id);
        Task CreateAsync(T model);
        Task SaveAsync();
    }
}
