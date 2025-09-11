using System.Collections.Generic;
using System.Threading.Tasks;

namespace HubWalks.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(object id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(object id);
    }
}
