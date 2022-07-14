namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System.Linq;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.AutomaticPurchaseOrders;
    using Microsoft.EntityFrameworkCore;

    public class AutomaticPurchaseOrderRepository : EntityFrameworkRepository<AutomaticPurchaseOrder, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public AutomaticPurchaseOrderRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.AutomaticPurchaseOrders)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override AutomaticPurchaseOrder FindById(int key)
        {
            return this.serviceDbContext.AutomaticPurchaseOrders
                .Include(v => v.Details)
                .SingleOrDefault(v => v.Id == key);
        }
    }
}
