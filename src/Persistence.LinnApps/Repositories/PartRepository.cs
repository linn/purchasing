namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using Microsoft.EntityFrameworkCore;

    public class PartRepository : IQueryRepository<Part>
    {
        private readonly ServiceDbContext serviceDbContext;

        public PartRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public Part FindBy(Expression<Func<Part, bool>> expression)
        {
            return this.serviceDbContext.Parts
                .Include(p => p.Currency)
                .Include(p => p.PreferredSupplier)
                .AsNoTracking().SingleOrDefault(expression);
        }

        public IQueryable<Part> FilterBy(Expression<Func<Part, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Part> FindAll()
        {
            throw new NotImplementedException();
        }
    }
}
