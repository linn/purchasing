namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.Boms;

    using Microsoft.EntityFrameworkCore;

    public class PcasChangeRepository : EntityFrameworkRepository<PcasChange, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public PcasChangeRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.PcasChanges)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override PcasChange FindBy(Expression<Func<PcasChange, bool>> expression)
        {
            return this.serviceDbContext.PcasChanges
                .Where(expression)
                .Include(c => c.ChangeRequest)
                .FirstOrDefault();
        }
    }
}
