using HubWalks.Bussines.Models;
using HubWalks.Data.Context;

namespace HubWalks.Data.Repository
{
    public class NotaFiscalRepository : Repository<NotaFiscal>, INotaFiscalRepository
    {
        public NotaFiscalRepository(HubWalksDbContext context) : base(context)
        {
        }
    }
}
