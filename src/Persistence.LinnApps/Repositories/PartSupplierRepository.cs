namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Persistence.LinnApps.Keys;

    using Microsoft.EntityFrameworkCore;

    public class PartSupplierRepository : EntityFrameworkRepository<PartSupplier, PartSupplierKey>
    {
        public PartSupplierRepository(DbSet<PartSupplier> databaseSet)
            : base(databaseSet)
        {
        }
    }
}
