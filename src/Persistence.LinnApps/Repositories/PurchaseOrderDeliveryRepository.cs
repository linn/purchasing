namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using Microsoft.EntityFrameworkCore;

    public class PurchaseOrderDeliveryRepository : EntityFrameworkRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey>, IPurchaseOrderDeliveryRepository
    {
        private readonly ServiceDbContext serviceDbContext;

        public PurchaseOrderDeliveryRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.PurchaseOrderDeliveries)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<PurchaseOrderDelivery> FilterBy(Expression<Func<PurchaseOrderDelivery, bool>> expression)
        {
            return this.serviceDbContext.PurchaseOrderDeliveries
               .Include(d => d.PurchaseOrderDetail).ThenInclude(plod => plod.Part)
               .Include(d => d.PurchaseOrderDetail).ThenInclude(plod => plod.PurchaseOrder)
               .ThenInclude(plo => plo.Supplier).ThenInclude(s => s.VendorManager).Where(expression);
        }

        public IEnumerable<PurchaseOrderDelivery> SearchByOrderWithWildcard(string expression)
        {
            return this.serviceDbContext.PurchaseOrderDeliveries.Where(
                x => EF.Functions.Like(x.OrderNumber.ToString(), expression))
                    .Include(d => d.PurchaseOrderDetail).ThenInclude(plod => plod.Part)
                    .Include(d => d.PurchaseOrderDetail).ThenInclude(plod => plod.PurchaseOrder)
                    .ThenInclude(plo => plo.Supplier).ThenInclude(s => s.VendorManager);
        }

        public override IQueryable<PurchaseOrderDelivery> FindAll()
        {
            return this.serviceDbContext.PurchaseOrderDeliveries.AsNoTracking()
                .Include(d => d.PurchaseOrderDetail).ThenInclude(plod => plod.Part)
                .Include(d => d.PurchaseOrderDetail).ThenInclude(plod => plod.PurchaseOrder)
                .ThenInclude(plo => plo.Supplier).ThenInclude(s => s.VendorManager);
        }

        public override PurchaseOrderDelivery FindById(PurchaseOrderDeliveryKey key)
        {
            return this.serviceDbContext.PurchaseOrderDeliveries
                .Where(x => x.OrderNumber == key.OrderNumber
                            && x.OrderLine == key.OrderLine
                            && x.DeliverySeq == key.DeliverySequence)
                .Include(d => d.PurchaseOrderDetail).ThenInclude(plod => plod.Part)
                .Include(d => d.PurchaseOrderDetail).ThenInclude(plod => plod.PurchaseOrder)
                .ThenInclude(plo => plo.Supplier).FirstOrDefault();
        }
    }
}
