namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.Boms;

    using Microsoft.EntityFrameworkCore;

    public class BomDetailComponentRepository : EntityFrameworkQueryRepository<BomDetailComponent>
    {
        public BomDetailComponentRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.BomDetailComponents)
        {
        }

        public override IQueryable<BomDetailComponent> FilterBy(Expression<Func<BomDetailComponent, bool>> expression)
        {
            return base.FilterBy(expression);
        }
    }
}
