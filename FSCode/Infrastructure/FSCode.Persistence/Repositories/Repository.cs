using FSCode.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FSCode.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {

        readonly DataContext _context;
        public Repository(DataContext context)
        {
            _context = context;
        }
        
        DbSet<T> Table
        {
            get => _context.Set<T>();
            set => _context.Set<T>();
        }

        public async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> expression, bool tracking = true, params string[] includes)
        {
            var query = Table.Where(expression);
            query = IsTracking(query, tracking);
            query = Includes(query, includes);
            return query;
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression, bool tracking = true, params string[] includes)
        {
            var query = Table.AsQueryable<T>();
            query = IsTracking(query, tracking);
            query = Includes(query, includes);
            return await query.FirstOrDefaultAsync(expression);
        }

        public async Task InsertAsync(T entity)
        {
            await Table.AddAsync(entity);
        }
        public async Task<bool> IsAny(Expression<Func<T, bool>> expression)
        {
            return await Table.AnyAsync(expression);
        }
        public async Task Remove(Expression<Func<T, bool>> expression, bool tracking = true, params string[] includes)
        {
            var entity = await Table.FirstOrDefaultAsync(expression);
            Table.Remove(entity);
        }
        public async Task Remove(int id)
        {
            var entity = await Table.FindAsync(id);
            Table.Remove(entity);
        }




        public IQueryable<T> IsTracking(IQueryable<T> query, bool tracking)
        {
            if (!tracking)
            {
                query = query.AsNoTracking();
            }
            return query;

        }
        public IQueryable<T> Includes(IQueryable<T> query, params string[] includes)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
