namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System.Linq;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.Boms;

    using Microsoft.EntityFrameworkCore;

    public class BomDetailRepository : EntityFrameworkRepository<BomDetail, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public BomDetailRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.BomDetails)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override BomDetail FindById(int key)
        {
            return this.FilterBy(x => x.DetailId == key)
                .Include(x => x.DeleteChange).Include(x => x.AddChange)
                .FirstOrDefault();
        }
    }
}
