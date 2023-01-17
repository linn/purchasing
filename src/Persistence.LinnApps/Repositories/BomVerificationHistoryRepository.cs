namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;

    using Microsoft.EntityFrameworkCore;

    public class BomVerificationHistoryRepository : IRepository<BomVerificationHistory, int>
    {
        private readonly ServiceDbContext serviceDbContext;
       
        private readonly DbSet<BomVerificationHistory> bomVerificationHistory;

        public BomVerificationHistoryRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public BomVerificationHistory FindById(int key)
        {
            throw new NotImplementedException();
        }

        public IQueryable<BomVerificationHistory> FindAll()
        {
            return this.bomVerificationHistory.Include(b => b.TRef)
                .Include(b => b.PartNumber)
                .Include(b => b.DateVerified)
                .Include(b => b.DateVerified)
                .Include(b => b.VerifiedBy)
                .Include(b => b.DocumentType)
                .Include(b => b.DocumentNumber)
                .Include(b => b.Remarks)
                .AsNoTracking();
        }

        public void Add(BomVerificationHistory entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(BomVerificationHistory entity)
        {
            throw new NotImplementedException();
        }

        public BomVerificationHistory FindBy(Expression<Func<BomVerificationHistory, bool>> expression)
        {
            return this.bomVerificationHistory.Include(b => b.PartNumber).SingleOrDefault(expression);
        }

        public IQueryable<BomVerificationHistory> FilterBy(Expression<Func<BomVerificationHistory, bool>> expression)
        {
            return this.bomVerificationHistory.Include(b => b.TRef)
                .Include(b => b.PartNumber)
                .Include(b => b.DateVerified)
                .Include(b => b.DateVerified)
                .Include(b => b.VerifiedBy)
                .Include(b => b.DocumentType)
                .Include(b => b.DocumentNumber)
                .Include(b => b.Remarks).Where(expression)
                .AsNoTracking();
        }
    }
}
