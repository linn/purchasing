namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using Microsoft.EntityFrameworkCore;

    public class BomDetailRepository : IBomDetailRepository
    {
        private readonly DbSet<BomDetail> bomDetails;

        public BomDetailRepository(ServiceDbContext serviceDbContext)
        {
            this.bomDetails = serviceDbContext.BomDetails;
        }

        public BomDetail FindBy(Expression<Func<BomDetail, bool>> expression)
        {
            return this.bomDetails.Include(b => b.Part).SingleOrDefault(expression);
        }

        public IQueryable<BomDetail> FilterBy(Expression<Func<BomDetail, bool>> expression)
        {
            return this.bomDetails.Include(b => b.Part)
                .ThenInclude(p => p.PartSuppliers.Where(ps => ps.SupplierRanking == 1))
                .Include(d => d.PartRequirement)
                .Include(d => d.BomPart).Where(expression)
                .AsNoTracking();
        }

        public IQueryable<BomDetail> FindAll()
        {
            return this.bomDetails
                .Include(d => d.Part)
                .Include(d => d.Components)
                .AsNoTracking();
        }

        public IEnumerable<BomDetail> GetLiveBomDetails(string bomName)
        {
            return this.bomDetails.Include(b => b.Part).Where(b => b.BomName == bomName && b.ChangeState == "LIVE");
        }
    }
}
