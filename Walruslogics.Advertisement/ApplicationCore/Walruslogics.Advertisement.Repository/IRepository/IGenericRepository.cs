using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Walruslogics.Identity.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(object id);

        /// <summary>
        /// GetAll returns IQueryable because we don’t want to return full list.
        /// However, we want to return something that caller will be able to use to further process the query.
        /// Maybe filter it by something, do paging, etc. 
        /// That’s no interest of us for now.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();

        void Add(T entity);
        void AddRange(List<T> entity);

        void Update(object id, T entity);

        void Delete<T>(T entity) where T : class;

        IQueryable<T> GetByCriteria(Expression<Func<T, bool>> filter,
             Func<IQueryable<T>, IIncludableQueryable<T, Object>>? include = null);

        int SaveChanges();
    }
}
