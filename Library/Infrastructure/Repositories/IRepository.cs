using System;
using System.Collections.Generic;
using Music.Domain.Entities;
using Music.Infrastructure.UnitsOfWork;

namespace Music.Infrastructure.Repositories
{
    public interface IRepository<TEntity, in TKey> where TEntity : Entity
    {
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Modify(TEntity entity);
        TEntity Get(TKey id);
        IEnumerable<TEntity> GetAll();
        MainUnitOfWork UnitOfWork { get; }
    }
}