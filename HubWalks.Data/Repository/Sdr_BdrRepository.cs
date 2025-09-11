using HubWalks.Bussines.Models;
using HubWalks.Data.Context;

namespace HubWalks.Data.Repository
{
    public class Sdr_BdrRepository : Repository<Sdr_Bdr>, ISdr_BdrRepository
    {
        public Sdr_BdrRepository(HubWalksDbContext context) : base(context)
        {
        }
    }
}
