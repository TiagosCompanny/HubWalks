using HubWalks.Bussines.Models;
using HubWalks.Data.Context;

namespace HubWalks.Data.Repository
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(HubWalksDbContext context) : base(context)
        {
        }
    }
}
