namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System.Linq;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps;

    using Microsoft.EntityFrameworkCore;

    public class VendorManagerRepository : EntityFrameworkRepository<VendorManager, string>
    {
        private readonly ServiceDbContext serviceDbContext;

        public VendorManagerRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.VendorManagers)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<VendorManager> FindAll()
        {
            return this.serviceDbContext.VendorManagers.AsNoTracking().Include(x => x.Employee);
        }
    }
}
