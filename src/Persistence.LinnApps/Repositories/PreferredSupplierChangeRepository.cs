namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Persistence.LinnApps.Keys;

    using Microsoft.EntityFrameworkCore;

    public class PreferredSupplierChangeRepository 
        : EntityFrameworkRepository<PreferredSupplierChange, PreferredSupplierChangeKey>
    {
        private readonly DbSet<PreferredSupplierChange> databaseSet;

        public PreferredSupplierChangeRepository(DbSet<PreferredSupplierChange> databaseSet)
            : base(databaseSet)
        {
        }

        public override PreferredSupplierChange FindById(PreferredSupplierChangeKey key)
        {
            return this.databaseSet
                .Include(x => x.ChangeReason)
                .Include(x => x.ChangedBy)
                .Include(x => x.NewCurrency)
                .Include(x => x.OldSupplier)
                .Include(x => x.NewSupplier)
                .SingleOrDefault(x => x.PartNumber == key.PartNumber && x.Seq == key.Seq);
        }

        public override PreferredSupplierChange FindBy(Expression<Func<PreferredSupplierChange, bool>> expression)
        {
            return this.databaseSet
                .Include(x => x.ChangeReason)
                .Include(x => x.ChangedBy)
                .Include(x => x.NewCurrency)
                .Include(x => x.OldSupplier)
                .Include(x => x.NewSupplier)
                .SingleOrDefault(expression);
        }

        public override IQueryable<PreferredSupplierChange> FilterBy(Expression<Func<PreferredSupplierChange, bool>> expression)
        {
            return this.databaseSet
                .Include(x => x.ChangeReason)
                .Include(x => x.ChangedBy)
                .Include(x => x.NewCurrency)
                .Include(x => x.OldSupplier)
                .Include(x => x.NewSupplier)
                .Where(expression);
        }
    }
}
