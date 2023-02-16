namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.Boms;

    using Microsoft.EntityFrameworkCore;

    public class BomVerificationHistoryRepository : EntityFrameworkRepository<BomVerificationHistory, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public BomVerificationHistoryRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.BomVerificationHistory)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<BomVerificationHistory> FilterBy(Expression<Func<BomVerificationHistory, bool>> expression)
        {
            return this.serviceDbContext.BomVerificationHistory
                .Where(expression);
        }
    }
}
