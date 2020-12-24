using System.Collections.Generic;

namespace Cafe.API.Models.Repository
{
    public interface IDataRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);
        TEntity GetLast();
        void Add(TEntity entity);
        void Update(TEntity entityToUpdate, TEntity entity);
        void Delete(TEntity entity);
        int Count();
        IEnumerable<TEntity> GetSales(int clientId);
    }
}
