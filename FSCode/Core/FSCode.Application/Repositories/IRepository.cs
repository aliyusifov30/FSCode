using Microsoft.AspNetCore.Http.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FSCode.Application.Repositories
{
    public interface IRepository<T>
    {

        Task<T> GetAsync(Expression<Func<T, bool>> expression, bool tracking = true, params string[] includes);
        Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> expression, bool tracking = true, params string[] includes);
        Task<bool> IsAny(Expression<Func<T, bool>> expression);


        Task InsertAsync(T entity);

        Task Remove(Expression<Func<T, bool>> expression, bool tracking = true, params string[] includes);
        Task Remove(int id);

        Task<int> CommitAsync();
    }
}
