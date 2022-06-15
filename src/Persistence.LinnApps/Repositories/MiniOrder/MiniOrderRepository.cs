namespace Linn.Purchasing.Persistence.LinnApps.Repositories.MiniOrder
{
    using System.Linq;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;

    using Microsoft.EntityFrameworkCore;

    public class MiniOrderRepository : EntityFrameworkRepository<MiniOrder, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public MiniOrderRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.MiniOrders)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override MiniOrder FindById(int key)
        {
            return this.serviceDbContext.MiniOrders
                .Where(m => m.OrderNumber == key).Include(m => m.Deliveries).ToList()
                .FirstOrDefault();
        }
    }
}
