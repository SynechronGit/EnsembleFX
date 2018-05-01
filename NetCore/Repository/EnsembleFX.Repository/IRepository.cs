using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
//using EnsembleFX.Filters;

namespace EnsembleFX.Repository
{
    public interface IRepository<T> : IDisposable where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        T GetById(long id);
        T GetById(string id);
        T GetById(Guid id);
        T Get(Expression<Func<T, bool>> where);
        Task<T> GetAsync(Expression<Func<T, bool>> where);
        IList<T> GetAll();
        IList<T> GetMany(Expression<Func<T, bool>> where);
        Task<IList<T>> GetManyAsync(Expression<Func<T, bool>> where);
        IList<T> FindAllWithIncludes(params string[] associations);
        IList<T> FindAllWithIncludes(Expression<Func<T, bool>> where, params string[] associations);
        Task<T> FindWithIncludesAsync(Expression<Func<T, bool>> where, params string[] associations);
        Task<IList<T>> FindAllWithIncludesAsync(Expression<Func<T, bool>> where, params string[] associations);
        T FindWithIncludes(Expression<Func<T, bool>> where, params string[] associations);
        IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate);

        void Save();
        Task<int> SaveAsync();
        IList<T> ExecuteQuery(string query);
        
        //TODO Find System.Data.Entity replacement
        //IDbSet<T> DBSet { get; }

        //TODO Add after filter project is converted
        /*
        IList<T> GetSearchedData(PagingFiltering pagingFilters, out int total, string orderByPredicate, params string[] associations);
        void AddFilter(PagingFiltering pagingFilters, string field, string filterOperator, string variable);
        */
    }
}
