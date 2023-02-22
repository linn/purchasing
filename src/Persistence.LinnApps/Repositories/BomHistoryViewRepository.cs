namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    
    public class BomHistoryViewRepository : EntityFrameworkQueryRepository<BomHistoryViewEntry>
    {
        public BomHistoryViewRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.BomHistoryView)
        {
        }
    }
}
