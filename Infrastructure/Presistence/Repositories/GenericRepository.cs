using Domain.Contracs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Presistence.Data.Dbcontexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _dbContext;
        public GenericRepository( StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(TEntity entity)
           => await _dbContext.AddAsync(entity);

        public void Delete(TEntity entity)
             =>  _dbContext.Set<TEntity>().Remove(entity);

        // Delete OR Update OR Add دي مش بتخليها تتبع التغيرات ولا تتبع البيانات اللي جايه من الداتا بيز ومش هيعدل من هنا لازم لو هيعدل يعدل من  asNoTracking
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false)
          => asNoTracking ? await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync() 
            : await _dbContext.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(TKey id)
              => await _dbContext.Set<TEntity>().FindAsync(id);

        public void Update(TEntity entity)
             => _dbContext.Set<TEntity>().Update(entity);
    }
}
