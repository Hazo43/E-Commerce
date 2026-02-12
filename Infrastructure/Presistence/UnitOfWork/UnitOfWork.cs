using Domain.Contracs;
using Domain.Entities;
using Presistence.Data.Dbcontexts;
using Presistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private Dictionary<string, Object> _repositories;
        // Dictionary => [Key(string) , Value(object) ]
        // Key : NameOfEntity [Product] ==> String
        // Value : object Of Generic Repository
        public UnitOfWork( StoreDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new();
        }
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            // 1/  Product [string] مثلا هتكون TEntity دا كدا بيجيب نوع ال
            var EntityTypeOfKey = typeof(TEntity).Name;

            // 2   GenericRepository جديد من Object لا روح اعملو Product اللي هو مثلا EntityTypeOfKey مش بتحتوي علي _repositories لو ال
            if ( ! _repositories.ContainsKey(EntityTypeOfKey) )
                _repositories[EntityTypeOfKey] = new GenericRepository<TEntity , TKey>(_dbContext);

            // 3  هنرجعهالو EntityTypeOfKey لاكن لو بتحتوي علي
            return (IGenericRepository<TEntity, TKey>) _repositories[EntityTypeOfKey];
        }

        public async Task<int> SaveChangesAsync()
            => await _dbContext.SaveChangesAsync();
    }
}
