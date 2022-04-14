namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class ReceiptPrefSupDiffRepository : IQueryRepository<ReceiptPrefSupDiff>
    {
        private readonly ServiceDbContext serviceDbContext;

        public ReceiptPrefSupDiffRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public ReceiptPrefSupDiff FindBy(Expression<Func<ReceiptPrefSupDiff, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ReceiptPrefSupDiff> FilterBy(Expression<Func<ReceiptPrefSupDiff, bool>> expression)
        {
            return this.serviceDbContext.ReceiptPrefsupDiffs.Where(expression);
        }

        public IQueryable<ReceiptPrefSupDiff> FindAll()
        {
            throw new NotImplementedException();
        }
    }
}