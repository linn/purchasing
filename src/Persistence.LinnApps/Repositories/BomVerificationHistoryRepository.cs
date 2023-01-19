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
       
        private readonly DbSet<BomVerificationHistory> bomVerificationHistory;

        public BomVerificationHistoryRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext.BomVerificationHistory)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override BomVerificationHistory FindById(int key)
        {
            return this.serviceDbContext.BomVerificationHistory.FirstOrDefault(b => b.TRef == key);
        }

        public override IQueryable<BomVerificationHistory> FindAll()
        {
            return this.bomVerificationHistory
                .AsNoTracking();
        }

        public override void Add(BomVerificationHistory entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(BomVerificationHistory entity)
        {
            throw new NotImplementedException();
        }

        public override BomVerificationHistory FindBy(Expression<Func<BomVerificationHistory, bool>> expression)
        {
            return this.serviceDbContext.BomVerificationHistory.Include(b => b.PartNumber).SingleOrDefault(expression);
        }

        public override IQueryable<BomVerificationHistory> FilterBy(Expression<Func<BomVerificationHistory, bool>> expression)
        {
            return this.serviceDbContext.BomVerificationHistory.Where(expression)
                .AsNoTracking();
        }
    }
}
