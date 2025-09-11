using System.Collections.Generic;
using System.Threading.Tasks;

namespace HubWalks.Bussines.Interfaces
{
    public interface IService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(object id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(object id);
    }
}
