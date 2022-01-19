namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Persistence.LinnApps.Keys;

    using Microsoft.EntityFrameworkCore;

    public class PreferredSupplierChangeRepository 
        : EntityFrameworkRepository<PreferredSupplierChange, PreferredSupplierChangeKey>
    {
        public PreferredSupplierChangeRepository(DbSet<PreferredSupplierChange> databaseSet)
            : base(databaseSet)
        {
        }
    }
}
