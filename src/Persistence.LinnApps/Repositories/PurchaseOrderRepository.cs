namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps;

    using Microsoft.EntityFrameworkCore;

    public class PurchaseOrderRepository : EntityFrameworkRepository<PurchaseOrder, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public PurchaseOrderRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.PurchaseOrders)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override PurchaseOrder FindById(int key)
        {
            var thing = this.serviceDbContext.PurchaseOrders.Find(key);
            this.serviceDbContext.Entry(thing).Collection(p => p.Details).Load();
            return thing;
        }

        public override IQueryable<PurchaseOrder> FindAll()
        {
            return this.serviceDbContext.PurchaseOrders
                .Include(a => a.Details);
        }

        public override IQueryable<PurchaseOrder> FilterBy(Expression<Func<PurchaseOrder, bool>> expression)
        {
            return this.serviceDbContext.PurchaseOrders.Where(expression)
                .Include(o => o.Details).AsNoTracking().AsQueryable();
        }
    }
}
