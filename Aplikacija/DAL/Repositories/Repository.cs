using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class Repository<T> : Repository<T,int> where T : class, IEntity<int>
    {
        public Repository(ApplicationDbContext ctx) : base(ctx) { }
    }

    public class Repository<T,K> : Interfaces.IRepository<T,K> where T : class, IEntity<K> 
    {
        private DbSet<T> Set => context.Set<T>();
        private readonly ApplicationDbContext context;

        public Repository(ApplicationDbContext ctx) => context = ctx;

        public async Task<T> AddAsync(T data) => (await Set.AddAsync(data)).Entity;
        public async Task DeleteAsync(K id) => Set.Remove(await GetByIdAsync(id));
        public Task<List<T>> GetAllAsync(params string[] include) => Include(include).ToListAsync();
        public List<T> Find(Func<T, bool> exp, params string[] include) => Include(include).Where(exp).ToList();
        public async Task<T> GetByIdAsync(K id, params string[] include) => await Include(include).FirstOrDefaultAsync(x => x.Id.Equals(id));
        public async Task SaveAsync() => await context.SaveChangesAsync();
        public T Update(T data) => context.Attach(data)?.Entity;
        public async Task<T> Update(K id) => context.Attach(await GetByIdAsync(id))?.Entity;

        private IQueryable<T> Include(string[] includes)
        {
            var x = Set.AsQueryable();

            if (includes.Length == 0)
                return x;

            for (int i = 0; i < includes.Length; ++i)
                    x = x.Include(includes[i]);
            return x;
        }
    }
}
