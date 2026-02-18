using Domain.Contracs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal abstract class BaseSpecifications<TEntity, TKey>
        : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        #region Criteria

        public Expression<Func<TEntity, bool>>? Criteria { get; private set; }
        // where عشان اجبر كلو يستخدم ال constractor عملتها جوا
        protected BaseSpecifications(Expression<Func<TEntity, bool>>? criteriaExpression)
        {
            Criteria = criteriaExpression;
        }

        #endregion

        #region Include

        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();

        protected void AddInclude(Expression<Func<TEntity, object>> includeExp)
        {
            IncludeExpressions.Add(includeExp);
        }

        #endregion

        #region Sorting ( OrderBy , OrderByDescending)
       
        // OrderBy
        public Expression<Func<TEntity, object>> OrderBy {get; private set;}

        protected void AddOrderBy (Expression<Func<TEntity , object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
      
        // OrderByDescending
        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }
        #endregion

        #region PAgination [ Take - Skip ]

        public int Skip {get ; private set;}

        public int Take { get; private set; }

        public bool IsPaginated { get; private set; }
        
        protected void ApplyPagination ( int pageSize, int pageIndex )
        {
            IsPaginated = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
        }

        #endregion
    }
}
