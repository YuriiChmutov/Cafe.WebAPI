using System.Collections.Generic;

namespace Cafe.API.Models.Repository
{
    public interface IDataRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);
        void Add(TEntity entity);
        void Update(TEntity entityToUpdate, TEntity entity);
        void Delete(TEntity entity);
        IEnumerable<TEntity> GetSales(int clientId);
    }
}
