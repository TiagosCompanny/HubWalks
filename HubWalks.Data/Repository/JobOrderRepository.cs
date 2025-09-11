using HubWalks.Bussines.Models;
using HubWalks.Data.Context;

namespace HubWalks.Data.Repository
{
    public class JobOrderRepository : Repository<JobOrder>, IJobOrderRepository
    {
        public JobOrderRepository(HubWalksDbContext context) : base(context)
        {
        }
    }
}
