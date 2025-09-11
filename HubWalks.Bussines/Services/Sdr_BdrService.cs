using HubWalks.Bussines.Models;
using HubWalks.Data.Repository;

namespace HubWalks.Bussines.Services
{
    public class Sdr_BdrService : Service<Sdr_Bdr>, ISdr_BdrService
    {
        public Sdr_BdrService(ISdr_BdrRepository repository) : base(repository)
        {
        }
    }
}
