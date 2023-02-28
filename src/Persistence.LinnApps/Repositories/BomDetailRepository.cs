namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.Boms;

    using Microsoft.EntityFrameworkCore;

    public class BomDetailRepository : EntityFrameworkRepository<BomDetail, int>
    {
        public BomDetailRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.BomDetails)
        {
        }

        public override BomDetail FindById(int key)
        {
            return this.FilterBy(x => x.DetailId == key)
                .Include(x => x.DeleteChange).Include(x => x.AddChange)
                .FirstOrDefault();
        }

        public override IQueryable<BomDetail> FilterBy(Expression<Func<BomDetail, bool>> expression)
        {
            return base.FilterBy(expression)
                .Include(d => d.Part).Include(d => d.AddChange).Include(d => d.DeleteChange);
        }

        public override IQueryable<BomDetail> FindAll()
        {
            return base.FindAll()
                .Include(d => d.Part).Include(d => d.AddChange).Include(d => d.DeleteChange);
        }
    }
}
