using HubWalks.Bussines.Models;
using HubWalks.Data.Repository;

namespace HubWalks.Bussines.Services
{
    public class NotaFiscalService : Service<NotaFiscal>, INotaFiscalService
    {
        public NotaFiscalService(INotaFiscalRepository repository) : base(repository)
        {
        }
    }
}
