namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;

    using Microsoft.EntityFrameworkCore;

    public class MrPurchaseOrderRepository : IQueryRepository<MrPurchaseOrderDetail>
    {
        private readonly ServiceDbContext serviceDbContext;

        public MrPurchaseOrderRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public IQueryable<MrPurchaseOrderDetail> FilterBy(Expression<Func<MrPurchaseOrderDetail, bool>> expression)
        {
           return this.serviceDbContext.MrOutstandingPurchaseOrders
               .Include(s => s.Deliveries)
               .Include(s => s.PartSupplierRecord).ThenInclude(ps => ps.Part)
               .AsNoTracking()
               .Where(expression);
        }

        IQueryable<MrPurchaseOrderDetail> IQueryRepository<MrPurchaseOrderDetail>.FindAll()
        {
            throw new NotImplementedException();
        }

        MrPurchaseOrderDetail IQueryRepository<MrPurchaseOrderDetail>.FindBy(Expression<Func<MrPurchaseOrderDetail, bool>> expression)
        {
            throw new NotImplementedException();
        }
    }
}
