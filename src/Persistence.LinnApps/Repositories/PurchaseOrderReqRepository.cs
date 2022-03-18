namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using Microsoft.EntityFrameworkCore;

    public class PurchaseOrderReqRepository : EntityFrameworkRepository<PurchaseOrderReq, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public PurchaseOrderReqRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.PurchaseOrderReqs)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override PurchaseOrderReq FindById(int key)
        {
            var purchaseOrderReq = this.serviceDbContext.PurchaseOrderReqs
                .Include(r => r.Currency)
                .Include(r => r.Country)
                .Include(r => r.RequestedBy)
                .Include(r => r.AuthorisedBy)
                .Include(r => r.SecondAuthBy)
                .Include(r => r.TurnedIntoOrderBy)
                .Include(r => r.FinanceCheckBy)
                .Include(r => r.Nominal)
                .Include(r => r.Department)
                .Include(r => r.Supplier).ThenInclude(s => s.OrderAddress)
                .FirstOrDefault(x => x.ReqNumber == key);
            return purchaseOrderReq;
        }

        public override IQueryable<PurchaseOrderReq> FilterBy(Expression<Func<PurchaseOrderReq, bool>> expression)
        {
            return base.FilterBy(expression).AsNoTracking().Include(r => r.Supplier);
        }
    }
}
