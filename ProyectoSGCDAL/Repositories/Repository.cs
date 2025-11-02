using Microsoft.EntityFrameworkCore;
using ProyectoSGCDAL.Data;
using System.Linq.Expressions;

namespace ProyectoSGCDAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _ctx;
        protected readonly DbSet<T> _set;

        public Repository(AppDbContext ctx) { _ctx = ctx; _set = ctx.Set<T>(); }

        public Task<T?> GetByIdAsync(int id) => _set.FindAsync(id).AsTask();
        public Task<List<T>> GetAllAsync() => _set.ToListAsync();
        public Task<List<T>> FindAsync(Expression<Func<T, bool>> pred) => _set.Where(pred).ToListAsync();
        public Task AddAsync(T entity) => _set.AddAsync(entity).AsTask();
        public void Update(T entity) => _set.Update(entity);
        public void Remove(T entity) => _set.Remove(entity);
        public Task<int> SaveAsync() => _ctx.SaveChangesAsync();
    }
}
