using Core.Repositories.Implementations;
using DAL.Repositories.Interfaces;
using Indigo.Entities.Concrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementations
{
    public class PostRepository : TEntityRepository<Post, AppDbContext>,IPostRepository
    {
        public PostRepository(AppDbContext db) : base(db) { }
    }
}
