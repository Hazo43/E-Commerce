using Domain.Contracs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence
{
    internal static class SpecificationsEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> EntryPoint,
            ISpecifications<TEntity, TKey> specifications) where TEntity : BaseEntity<TKey>
        {
            // Query بدايه ال
            var Query = EntryPoint;   // dbcontext.Products

            if (specifications is not null)
            {
                 // Criteria => Where
                if (specifications.Criteria is not null)
                    Query = Query.Where(specifications.Criteria);
                
                // OrderBy
                if(specifications.OrderBy is not null)
                    Query = Query.OrderBy(specifications.OrderBy);
             
                // OrderByDescending
                if(specifications.OrderByDescending is not null)
                    Query = Query.OrderByDescending(specifications.OrderByDescending);

                // Inclide
                // و لو فيه اي عنصر روح اعمل اللي بقولك عليه null لو مش ب 
                if (specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Count() > 0)
                {
                    // بردو صح Aggregate دي صح واللي تحت بتاع ال
                    //foreach (var expression in specifications.IncludeExpressions)
                    //{
                    //    Query = Query.Include(expression);
                    //}

                    // dbcontext.Products.Include(p => p.ProductBrand)
                    // dbcontext.Products.Include(p => p.ProductBrand).Include(P => P.productType);
                    Query = specifications.IncludeExpressions.Aggregate(Query,
                          (CurrentQuery, IncludeExp) => CurrentQuery.Include(IncludeExp));
                }

            }
            return Query;

        }
    }
}
