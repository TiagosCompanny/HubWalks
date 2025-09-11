using HubWalks.Bussines.Models;
using HubWalks.Data.Repository;

namespace HubWalks.Bussines.Services
{
    public class JobOrderService : Service<JobOrder>, IJobOrderService
    {
        public JobOrderService(IJobOrderRepository repository) : base(repository)
        {
        }
    }
}
