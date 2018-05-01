using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        
    }
}
