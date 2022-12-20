namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System.Linq;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.Boms;

    using Microsoft.EntityFrameworkCore;

    public class BomDetailRepository : EntityFrameworkRepository<BomDetail, int>
    {
        public BomDetailRepository(DbSet<BomDetail> databaseSet)
            : base(databaseSet)
        {
        }

        public override BomDetail FindById(int key)
        {
            return this.FindAll().Include(x => x.DeleteChange)
                .FirstOrDefault(x => x.DetailId == key);
        }
    }
}
