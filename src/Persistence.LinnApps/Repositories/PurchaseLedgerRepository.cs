namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;

    using Microsoft.EntityFrameworkCore;

    public class PurchaseLedgerRepository : EntityFrameworkRepository<PurchaseLedger, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public PurchaseLedgerRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.PurchaseLedgers)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<PurchaseLedger> FilterBy(Expression<Func<PurchaseLedger, bool>> expression)
        {
            return this.serviceDbContext.PurchaseLedgers.Include(pl => pl.TransactionType).Where(expression);
        }

        public override PurchaseLedger FindById(int key)
        {
            var purchaseLedger = this.serviceDbContext.PurchaseLedgers.Find(key);
            return purchaseLedger;
        }
    }
}
