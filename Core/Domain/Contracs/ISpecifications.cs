using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracs
{
    public interface ISpecifications<TEntity , TKey> where TEntity : BaseEntity<TKey>
    {
        // Signature for Property [ Expression ==> Where ]
        public Expression<Func<TEntity , bool>>? Criteria { get; }

        // Signature for Property [ Expression ==> Include ]
        //-> Include(P => p.ProductType).Include( p => p.ProductBrand);
        public List<Expression<Func<TEntity , object>>> IncludeExpressions {  get; }

        // OrderBy , OrderByDescending
        public Expression<Func<TEntity,object>> OrderBy { get;  }
        public Expression<Func<TEntity,object>> OrderByDescending { get;  }
    }
}
