using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<T> : IRepository<T, int> where T : class { }

    public interface IRepository<T,K> where T : class
    {
        Task<List<T>> GetAllAsync(params string[] includes);
        Task<T> GetByIdAsync(K id,params string[] include);
        List<T> Find(Func<T, bool> exp, params string[] includes);
        Task<T> AddAsync(T data); 
        Task DeleteAsync(K id);
        T Update(T data);
        Task<T> Update(K id);
        Task SaveAsync();
    }
}
