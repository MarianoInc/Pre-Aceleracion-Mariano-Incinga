using AlkemyChallenge.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlkemyChallenge.API.Dal.Repositories
{
    public class BaseRepository<TEntity, TContext> : IRepository<TEntity> where TEntity : class where TContext : DbContext
    {
        protected readonly TContext _dbContext;
        public BaseRepository(TContext dbContext)
        {
            this._dbContext = dbContext;
        }


        public List<TEntity> GetAllEntities()
        {
            return _dbContext.Set<TEntity>().ToList();
        }
        public TEntity Get(int id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }
        public TEntity Post(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }
        public virtual TEntity Update(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity), "La entidad no puede ser nula");
            _dbContext.Set<TEntity>().Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return entity;
        }
        public virtual TEntity Delete(int? id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id), "El id no puede ser nulo.");
            var internalContinent = _dbContext.Set<TEntity>().Find(id);
            _dbContext.Set<TEntity>().Remove(internalContinent);
            _dbContext.SaveChanges();
            return internalContinent;
        }

    }
}
