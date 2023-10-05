using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Walruslogics.Advertisement.Database.Models;

namespace Walruslogics.Identity.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        internal readonly AdvertisementDBContext _Context;
        internal DbSet<T> _dbEntity;
        
        public GenericRepository(AdvertisementDBContext Context)
        {
            _Context = Context;
            _dbEntity = _Context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbEntity.Add(entity);
        }
        public void AddRange(List<T> entity)
        {
            _dbEntity.AddRange(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            entity.GetType().GetProperty("IsDeleted").SetValue(entity, true);
        }

        public IQueryable<T> GetAll()
        {
            return _dbEntity;
            //return _dbEntity.Where(x => (bool)x.GetType().GetProperty("IsDeleted").GetValue(_dbEntity, null) == false);
        }

        public T GetById(object id)
        {
            return _dbEntity.Find(id);
            //return _dbEntity.Where(x=> (bool)x.GetType().GetProperty("IsDeleted").GetValue(_dbEntity, null) == false &&
            //                           (long)x.GetType().GetProperty("Id").GetValue(_dbEntity, null) == id).FirstOrDefault();
        }

        public void Update(object id, T data)
        {
            _dbEntity.Update(data);
        }

        public int SaveChanges()
        {
            return _Context.SaveChanges();
        }

        public IQueryable<T> GetByCriteria(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _dbEntity;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }
    }
}
