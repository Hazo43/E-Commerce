

using Domain.Entities;

namespace Domain.Contracs
{
    public interface IUnitOfWork
    {
        // 1 Savechanges
        Task<int> SaveChangesAsync();

        // Method To Return object from ( Generict Repository )  [ Entity ]
        // New GenericRepo<Product,int> ممكن ترجع 
        // New GenericRepo<Order,Guid> ممكن ترجع 

        // GetRepository و دا اسمها  IGenericRepository هترجع 
        IGenericRepository<TEntity , TKey> GetRepository<TEntity , TKey>() where TEntity : BaseEntity<TKey>;
    
    }
}
