using System.Collections.Generic;
using System.Threading.Tasks;
using HubWalks.Bussines.Interfaces;


namespace HubWalks.Bussines.Services
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<T>> GetAllAsync() => _repository.GetAllAsync();

        public Task<T?> GetByIdAsync(object id) => _repository.GetByIdAsync(id);

        public Task AddAsync(T entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(T entity) => _repository.UpdateAsync(entity);

        public Task RemoveAsync(object id) => _repository.RemoveAsync(id);
    }
}
