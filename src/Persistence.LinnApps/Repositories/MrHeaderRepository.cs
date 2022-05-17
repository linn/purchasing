namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;

    using Microsoft.EntityFrameworkCore;

    public class MrHeaderRepository : IQueryRepository<MrHeader>
    {
        private readonly ServiceDbContext serviceDbContext;

        public MrHeaderRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public IQueryable<MrHeader> FilterBy(Expression<Func<MrHeader, bool>> expression)
        {
           return this.serviceDbContext.MrHeaders
               .Include(s => s.MrDetails)
               .AsNoTracking()
               .Where(expression);
        }

        IQueryable<MrHeader> IQueryRepository<MrHeader>.FindAll()
        {
            throw new NotImplementedException();
        }

        MrHeader IQueryRepository<MrHeader>.FindBy(Expression<Func<MrHeader, bool>> expression)
        {
            throw new NotImplementedException();
        }
    }
}
