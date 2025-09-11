using HubWalks.Bussines.Models;
using HubWalks.Data.Repository;

namespace HubWalks.Bussines.Services
{
    public class ClienteService : Service<Cliente>, IClienteService
    {
        public ClienteService(IClienteRepository repository) : base(repository)
        {
        }
    }
}
