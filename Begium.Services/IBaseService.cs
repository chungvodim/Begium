using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Begium.Services
{
    public interface IBaseService<TEntity> : IDisposable where TEntity : class
    {
        bool Save();
        Task<bool> SaveAsync();
        void Insert(TEntity entity);
        void Update(TEntity entity);
        //void Delete(object id);
        //void Delete(TEntity entity);
        //Task DeleteAsync(object id);
        //Task DeleteAsync(TEntity entity);
        Task<TEntity> FindAsync(params object[] keyValues);
        TEntity Find(params object[] keyValues);
    }
}