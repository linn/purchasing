namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using Microsoft.EntityFrameworkCore;

    public class PurchaseOrderRepository : EntityFrameworkRepository<PurchaseOrder, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public PurchaseOrderRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.PurchaseOrders)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<PurchaseOrder> FilterBy(Expression<Func<PurchaseOrder, bool>> expression)
        {
            return this.serviceDbContext.PurchaseOrders.Where(expression)
                .Include(o => o.Supplier)
                .Include(o => o.Details).ThenInclude(d => d.Part)
                .Include(o => o.Details).ThenInclude(d => d.PurchaseDeliveries).Include(x => x.Supplier)
                .Include(x => x.Currency)
                .OrderByDescending(x => x.OrderNumber)
                .AsNoTracking();
        }

        public override IQueryable<PurchaseOrder> FindAll()
        {
            return this.serviceDbContext.PurchaseOrders
                .Include(o => o.Supplier)
                .Include(o => o.Details).ThenInclude(d => d.PurchaseDeliveries)
                .Include(o => o.Details).ThenInclude(d => d.Part)
                .Include(o => o.Supplier);
        }

        public override PurchaseOrder FindById(int key)
        {
            return this.serviceDbContext
                .PurchaseOrders
                .Include(o => o.Supplier).ThenInclude(s => s.SupplierContacts)
                .Include(o => o.RequestedBy)
                .Include(o => o.EnteredBy)
                .Include(o => o.AuthorisedBy)
                .Include(o => o.DeliveryAddress).ThenInclude(a => a.FullAddress)
                .Include(o => o.DocumentType)
                .Include(o => o.OrderMethod)
                .Include(o => o.Currency)
                .Include(o => o.Details).ThenInclude(d => d.Part)
                .Include(o => o.Details).ThenInclude(d => d.PurchaseDeliveries).ThenInclude(d => d.DeliveryHistories)
                .Include(o => o.Details).ThenInclude(d => d.OrderPosting)
                .ThenInclude(p => p.NominalAccount).ThenInclude(n => n.Nominal)
                .Include(o => o.Details).ThenInclude(d => d.OrderPosting)
                .ThenInclude(p => p.NominalAccount).ThenInclude(n => n.Department)
                .Include(p => p.OrderAddress).ThenInclude(x => x.FullAddress)
                .Include(p => p.OrderAddress).ThenInclude(x => x.Country)
                .Include(o => o.Details).ThenInclude(d => d.DeliveryConfirmedBy)
                .First(o => o.OrderNumber == key);
        }
    }
}
