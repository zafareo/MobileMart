using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRepository<T> 
    {
        Task<T> CreateAsync(T entity);
        Task<T?> GetAsync(Expression<Func<T, bool>> expression);
        Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> expression);
        Task<T?> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
